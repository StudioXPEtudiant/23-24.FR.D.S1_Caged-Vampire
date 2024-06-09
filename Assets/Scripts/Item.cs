using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemValue;
    public int damage;
    public int heal;
    public int number;
    public string effect;
    public string description;

    public Item(string name, int value, int dmg, int hp, int num, string eff, string descrip)
    {
        itemName = name;
        itemValue = value;
        damage = dmg;
        heal = hp;
        number = num;
        effect = eff;
        description = descrip;
    }
}
