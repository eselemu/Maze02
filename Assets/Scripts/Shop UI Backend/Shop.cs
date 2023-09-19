using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct Item {
    public string name;
    public int cost;
    public string[] ingredients;
    public int type;
    public bool owned;
    public string recipe;
    public string sprite;
}

[Serializable]
public class Shop{
    public int money;
    public Item[] inventory;

    public static Shop CreateFromJSON(string path)
    {
        return JsonUtility.FromJson<Shop>(path);
    }

    public int TotalInventory(){
        return inventory.Length;
    }
}