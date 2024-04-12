using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int itemID;
    [SerializeField]
    private InventoryItem item;

    public string ItemName()
    {
        return item.itemName;
    }
}
