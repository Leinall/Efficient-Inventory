using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Player Inventory Data")]
public class PlayerInventoryData : ScriptableObject
{
    [SerializeField]
    public InventoryItemSavedData[] inventoryItems;
}
