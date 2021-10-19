using System.Collections.Generic;

public class CharachterManager
{

    public string charachterName;
    public CharachterMood mood;
    private int energy;
    private int happiness;
    public readonly Dictionary<string, int> charachterQualities = new Dictionary<string, int>();
    public string position;
    public readonly Dictionary<string, int> inventory = new Dictionary<string, int>();

    public CharachterManager() {
        this.charachterName = "Default";
    }

    public CharachterManager(string name, Dictionary<string, int> charQualities) {
        this.charachterName = name;
        this.charachterQualities = charQualities;
    }

    public int Energy { get => energy; set => energy = value; }
    public int Happiness { get => happiness; set => happiness = value; }

    public void UpdateCharachterQualities(string quality, int value) {
        if (charachterQualities.ContainsKey(quality))
        {
            charachterQualities[quality] += value;
        }
        else {
            charachterQualities.Add(quality, value);
        }
    }

    public void UpdateInventory(string item, int quantity) {
        if (inventory.ContainsKey("item"))
        {
            inventory[item] += quantity;
        }
        else {
            if (quantity > 0) { inventory.Add(item, quantity); }
        }
    }
}
