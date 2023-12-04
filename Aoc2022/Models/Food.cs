namespace Aoc2022.Models;

public class Food : Item
{
    public Food(int calories): base('?')
    {
        this.Calories = calories;
    }
    public int Calories { get; }
}