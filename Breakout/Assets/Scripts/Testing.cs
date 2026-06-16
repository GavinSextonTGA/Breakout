using UnityEngine;

public class Character    
{
    public string Name;
    public Character(string name)
    {
        this.Name = name;
    }

}
public class Armor
{
    public string ArmorName;

    public Armor(string name)
    {
        ArmorName = name;
    }
}

public class PlayerCharacter : Character
{
    private Armor armor;

    public PlayerCharacter(string name, string ArmorName)
        : base(name) 
    {
        armor = new Armor(ArmorName);
    }
}