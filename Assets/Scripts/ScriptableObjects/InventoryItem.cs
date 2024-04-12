using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName; // we can make it ana enum also to reduce string usage
    public GameObject ItemPrefab;
    public ItemType itemType;
    public ItemRarity itemRarity;
    public ItemStats itemStats;
    public int stackSize;
    //public int PlayerItemsCount; // this can be moved to another class that contains only the inventory item and it's count #done

}

public enum ItemType { Weapon, Armor, Potion };
public enum ItemRarity { Common, Rare, Epic };
public enum ItemStats { Damage, Defense, Health };

