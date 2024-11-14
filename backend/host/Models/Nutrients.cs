namespace host.Models;

public class Nutrients
{
    public int? Kilocalories { get; set; }
    public int? Proteins { get; set; }
    public int? Fats { get; set; }
    public int? Carbohydrates { get; set; }

    public Nutrients(int? kilocalories, int? proteins, int? fats, int? carbohydrates)
    {
        Kilocalories = kilocalories;
        Proteins = proteins;
        Fats = fats;
        Carbohydrates = carbohydrates;
    }
}