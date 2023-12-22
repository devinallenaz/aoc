using System.Text;
using AocHelpers;

namespace Aoc2023.Models;

public class PulseModuleSystem
{
    private Dictionary<string, PulseModuleBase> PulseModules { get; }
    private Dictionary<string, (long lowPulses, long highPulses, string nextState)> PulsesAndNextStateByState { get; } = new();
    private Queue<(string source, string destination, bool highPulse)> PulseQueue { get; } = new();

    public PulseModuleSystem(string init)
    {
        this.PulseModules = init.SplitLines().Select(CreatePulseModule).ToDictionary(m => m.Label);
        var outputIndex = PulseModules.Keys.Count;
        List<OutputModule> outputModules = new();
        foreach (var module in PulseModules.Values)
        {
            foreach (var destination in module.Destinations)
            {
                if (!PulseModules.ContainsKey(destination))
                {
                    var output = new OutputModule(destination, outputIndex++);
                    output.AddSource(module.Label);
                    outputModules.Add(output);
                }
                else
                {
                    PulseModules[destination].AddSource(module.Label);
                }
            }
        }

        foreach (var outputModule in outputModules)
        {
            PulseModules[outputModule.Label] = outputModule;
        }
    }

    public (long lowCount, long highCount) PushButton(long times)
    {
        var state = StateSnapshot;
        var totalLowCount = 0L;
        var totalHighCount = 0L;
        for (int i = 0; i < times; i++)
        {
            if (PulsesAndNextStateByState.ContainsKey(state))
            {
                var (lowCount, highCount, nextState) = PulsesAndNextStateByState[state];
                totalLowCount += lowCount;
                totalHighCount += highCount;
                state = nextState;
            }
            else
            {
                var (lowCount, highCount) = PushButtonOnce(i);
                var nextState = StateSnapshot;

                PulsesAndNextStateByState.Add(state, (lowCount, highCount, nextState));
                totalLowCount += lowCount;
                totalHighCount += highCount;
                state = nextState;
            }
        }

        return (totalLowCount, totalHighCount);
    }

    public long FindFirstOutput()
    {
        var output = PulseModules.Values.OfType<OutputModule>().First();
        var outputSource = output.Sources.Single();
        var sourceSources = PulseModules[outputSource].Sources.Select(s => (ConjunctionModule)PulseModules[s]).ToList();
        var i = 0;
        while (sourceSources.Any(s => s.FirstHighPulse == null))
        {
            i++;
            PushButtonOnce(i);
            if (output.HasReceivedLowPulse)
            {
                return i;
            }
        }

        return sourceSources.Select(s => s.FirstHighPulse!.Value).Aggregate(1L, Data.Lcm);
    }

    private (long lowCount, long highCount) PushButtonOnce(long buttonCount)
    {
        var lowCount = 0;
        var highCount = 0;
        PulseQueue.Enqueue(("", "broadcaster", false));
        while (PulseQueue.TryDequeue(out var pulse))
        {
            var (source, destination, isHighPulse) = pulse;
            if (isHighPulse)
            {
                highCount++;
            }
            else
            {
                lowCount++;
            }

            PulseModules[destination].ReceivePulse(isHighPulse, source, buttonCount, PulseQueue);
        }

        return (lowCount, highCount);
    }

    private static PulseModuleBase CreatePulseModule(string init, int id)
    {
        var parts = init.SplitAndTrim("->");
        var recipients = parts.Last().SplitCommas().ToList();
        if (parts.First() == "broadcaster")
        {
            return new BroadcastModule("broadcaster", id, recipients);
        }

        var (type, label) = parts.First().HeadAndTail();
        if (type == '%')
        {
            return new FlipFlopModule(string.Join("", label), id, recipients);
        }
        else if (type == '&')
        {
            return new ConjunctionModule(string.Join("", label), id, recipients);
        }

        throw new InvalidOperationException();
    }

    private string StateSnapshot
    {
        get
        {
            var sb = new StringBuilder();
            foreach (var module in PulseModules.Values.OrderBy(m => m.Id))
            {
                sb.Append(module.StateChar);
            }

            return sb.ToString();
        }
    }
}

public abstract class PulseModuleBase
{
    public PulseModuleBase(string label, int id, List<string> recipients)
    {
        this.Id = id;
        this.Label = label;
        this.Destinations = recipients;
    }

    public int Id { get; }
    public string Label { get; }
    public abstract void ReceivePulse(bool isHighPulse, string source, long buttonCount, Queue<(string, string, bool)> pulseQueue);
    public abstract char StateChar { get; }
    public List<string> Destinations { get; }
    public List<string> Sources { get; } = new();

    public virtual void AddSource(string source)
    {
        this.Sources.Add(source);
        if (Sources.Count > 16)
        {
            throw new InvalidOperationException();
        }
    }
}

public class BroadcastModule : PulseModuleBase
{
    public BroadcastModule(string label, int id, List<string> recipients) : base(label, id, recipients)
    {
    }

    public override void ReceivePulse(bool isHighPulse, string source, long buttonCount, Queue<(string, string, bool)> pulseQueue)
    {
        if (isHighPulse)
        {
            throw new NotImplementedException();
        }

        foreach (var recipient in this.Destinations)
        {
            pulseQueue.Enqueue((this.Label, recipient, false));
        }
    }

    public override char StateChar => 'B';
}

public class FlipFlopModule : PulseModuleBase
{
    private bool On { get; set; } = false;

    public FlipFlopModule(string label, int id, List<string> recipients) : base(label, id, recipients)
    {
    }

    public override void ReceivePulse(bool isHighPulse, string from, long buttonCount, Queue<(string, string, bool)> pulseQueue)
    {
        if (!isHighPulse)
        {
            On = !On;
            foreach (var recipient in this.Destinations)
            {
                pulseQueue.Enqueue((this.Label, recipient, On));
            }
        }
    }

    public override char StateChar => On ? '+' : '-';
}

public class ConjunctionModule : PulseModuleBase
{
    private Dictionary<string, bool> SourceStates { get; } = new();
    public long? FirstHighPulse { get; private set; } = null;

    public ConjunctionModule(string label, int id, List<string> recipients) : base(label, id, recipients)
    {
    }


    public override void AddSource(string source)
    {
        SourceStates[source] = false;
        base.AddSource(source);
    }

    public override void ReceivePulse(bool isHighPulse, string source, long buttonCount, Queue<(string, string, bool)> pulseQueue)
    {
        this.SourceStates[source] = isHighPulse;
        var sendHighPulse = this.SourceStates.Any(s => !s.Value);
        if (sendHighPulse && FirstHighPulse == null)
        {
            FirstHighPulse = buttonCount;
        }

        foreach (var recipient in this.Destinations)
        {
            pulseQueue.Enqueue((this.Label, recipient, sendHighPulse));
        }
    }

    public override char StateChar
    {
        get
        {
            ushort state = 0;
            foreach (var (kvp, index) in this.SourceStates.WithIndex())
            {
                if (kvp.Value)
                {
                    state += (ushort)Math.Pow(2, index);
                }
            }

            return (char)state;
        }
    }
}

public class OutputModule : PulseModuleBase
{
    public bool HasReceivedLowPulse { get; private set; }

    public override void ReceivePulse(bool isHighPulse, string source, long buttonCount, Queue<(string, string, bool)> pulseQueue)
    {
        if (isHighPulse)
        {
            this.HighCount++;
        }
        else
        {
            this.LowCount++;
            this.HasReceivedLowPulse = true;
        }
    }

    public override char StateChar => HasReceivedLowPulse ? 'T' : 'F';

    public long LowCount { get; set; } = 0;
    public long HighCount { get; set; } = 0;

    public OutputModule(string label, int id) : base(label, id, new List<string>())
    {
    }
}