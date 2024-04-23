using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int itemID;
    public InventoryItem item;

    public string ItemName()
    {
        return item.itemName;
    }
}
