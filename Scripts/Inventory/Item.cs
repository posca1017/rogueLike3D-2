using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
[CreateAssetMenu(fileName="Item",menuName="CreateItem")]
public class Item : ScriptableObject
{
    public enum KindOfItem
    {
        Weapon,
        UseItem
    }

    [SerializeField]
    private KindOfItem kindOfItem;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private string information;


    public KindOfItem GetKindOfItem()
    {
        return kindOfItem;
    }

    public Sprite GetItemIcon()
    {
        return icon;
    }

    public string GetItemName()
    {
        return itemName;
    }
    
    public string GetInformation()
    {
        return information;
    }
}
