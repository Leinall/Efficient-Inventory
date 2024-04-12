using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryHandler inventoryHandlerMain;

    public GameObject book;// creating a scenario to use the composte as I don't need to create objects, only interested in their count 

    public static GameManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
