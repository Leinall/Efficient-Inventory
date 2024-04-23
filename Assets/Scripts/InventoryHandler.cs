using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour // each kind of inventory shall has this class attached
{
    public Transform InvenoryParent;
    public PlayerInventoryData InventoryData;

    [SerializeField]
    private InventoryHandler otherInventory;

    // I can make it an array and manage it by input
    private Button currentClickedBtn;
    private int currentClickedBtnID = -1;

    private List<Button> InventoryBtns = new List<Button>();
    private Dictionary<InventoryItem, int> InventoryItems = new Dictionary<InventoryItem, int>();
    private int numberOfClicks = 0;
    private int NoOfItemsCreated = 0;

    private Stack<InventoryItem> itemsToBeRemoved = new Stack<InventoryItem>();

    // for multiclicking
    private int[] multiClickArray = new int[4] { -1, -1, -1, -1 };
    private int multiClickCounter = 0;
    private bool multiClicksIsValid;

    private void OnEnable()
    {
        RefreshPanel();
        //Invoke("RefreshPanel", 0.5f);

        // shall be uncommented for testing
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Axe"), 2);
        //AddItemToInventory(SearchForAnItem("Apple"), 3);
        //RemoveItemFromInventory(SearchForAnItem("Bag"));

        //For testing
        itemsToBeRemoved.Push(SearchForAnItem("Axe"));
        itemsToBeRemoved.Push(SearchForAnItem("Apple"));
        itemsToBeRemoved.Push(SearchForAnItem("Book"));
        itemsToBeRemoved.Push(SearchForAnItem("Coins"));

        RemoveStackedItems(itemsToBeRemoved);

        Invoke("RefreshPanel", 0.5f);
        //Invoke("test", 5f);
        //Invoke("RefreshPanel", 6f);
    }

    public void test() // adding item after deleting it
    {
        //print("test");
        AddItemToInventory(SearchForAnItem("Bag"), 3);
    }

    public void RemoveStackedItems(Stack<InventoryItem> itemsToBeRemoved)
    {
        // All are removed
        while (itemsToBeRemoved.Count > 0)
        {
            var item = itemsToBeRemoved.Pop();
            RemoveItemFromInventory(item);
        }
    }

    public void RemovingAllChildrenInPanel()
    {
        for (int i = 0; i < InvenoryParent.childCount; i++)
        {
            Destroy(InvenoryParent.GetChild(i).gameObject);
        }
    }

    // it reads from the dictionary to track any changes happened during play time
    public void RefreshPanel()
    {
        if (InventoryBtns.Count > 0)
        {
            foreach (var btn in InventoryBtns)
            {
                btn.onClick.RemoveAllListeners();
            }

            NoOfItemsCreated = 0;
            InventoryBtns.Clear();
            RemovingAllChildrenInPanel();
        }

        for (int i = 0; i < InventoryData.inventoryItems.Length; i++)
        {
            if (InventoryData.inventoryItems[i].itemCount >= 0 &&
                InventoryData.inventoryItems[i].itemCount < InventoryData.inventoryItems[i].inventoryItem.stackSize)
            {
                var key = InventoryData.inventoryItems[i].inventoryItem;

                //////print(this.gameObject.name + ": " + key + ", " + value);
                if (InventoryItems.ContainsKey(key))
                {
                    var value = InventoryItems[key];
                    if (value > 0)
                    {
                        // the item here shall take the reference of the button since we add all in the same order
                        var item = GameObject.Instantiate(InventoryData.inventoryItems[i].inventoryItem.ItemPrefab, InvenoryParent.transform);//InventoryBtns[i];
                        item.GetComponentInChildren<TextMeshProUGUI>().text
                           = value + "";
                        item.GetComponent<ItemInfo>().itemID = NoOfItemsCreated++;

                        //Adding the clicking functionality to each button
                        AddingFunctionalityToBtns(item);
                    }

                }
                else
                {
                    // In case the item is not found in the inventory which happens
                    // most of the time during only the intialisation
                    var itemCount = InventoryData.inventoryItems[i].itemCount;
                    InventoryItems.Add(key, itemCount);
                    if (itemCount > 0)
                    {
                        var item = GameObject.Instantiate(InventoryData.inventoryItems[i].inventoryItem.ItemPrefab, InvenoryParent.transform);//InventoryBtns[i];
                        item.GetComponentInChildren<TextMeshProUGUI>().text
                           = InventoryData.inventoryItems[i].itemCount + "";
                        item.GetComponent<ItemInfo>().itemID = NoOfItemsCreated++;

                        //Adding the clicking functionality to each button
                        AddingFunctionalityToBtns(item);
                    }
                }
            }

            //Unity takes some time to destroy the gameobjects and creating the new ones if the invoke is removed
            // it shall dtetcted doubled list 
            Invoke("ReassigningTheBtns", 0.2f);
        }
    }

    // Adding the highlight method + the transfer in case of the item is generated in the chest inventory
    public void AddingFunctionalityToBtns(GameObject item)
    {
        if (this.gameObject.CompareTag("chest"))
        {
            item.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                HighlightBtn(item.GetComponentInChildren<Button>());

                numberOfClicks++;
                if (numberOfClicks == 2)
                {
                    numberOfClicks = 0;
                    Transfer();
                }
            });
        }
        else
        {
            item.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                HighlightBtn(item.GetComponentInChildren<Button>());
            });
        }
    }

    public void ReassigningTheBtns()
    {
        InventoryBtns = InvenoryParent.GetComponentsInChildren<Button>().ToList();
    }

    public void AddItemToInventory(InventoryItem item)
    {
        if (!InventoryItems.ContainsKey(item) || item == null)
        {
            InventoryItems.Add(item, 1);
        }

        InventoryItems[item]++;
        if (InventoryItems[item] > item.stackSize)
        {
            InventoryItems[item] = item.stackSize;
        }
    }

    public void AddItemToInventory(InventoryItem item, int itemCount)
    {
        if (!InventoryItems.ContainsKey(item) || item == null)
        {
            InventoryItems.Add(item, 1);
        }

        InventoryItems[item] = InventoryItems[item] + itemCount;
        if (InventoryItems[item] > item.stackSize)
        {
            InventoryItems[item] = item.stackSize;
        }
    }

    // removes items totally from the inventory
    public void RemoveItemFromInventory(InventoryItem item)
    {
        if (item == null) return;
        InventoryItems[item] = 0;
    }

    public void DecreementItemCount(InventoryItem item)
    {
        if (item == null) return;

        print(item.itemName + "," + InventoryItems[item]);
        if (InventoryItems[item] <= 0)
        {
            InventoryItems[item] = 0;
        }
        else
        {
            InventoryItems[item]--;
        }
    }

    public void Transfer()
    {
        if (multiClickArray[0] != -1)
        {
            for (int i = 0; i < multiClickArray.Length; i++)
            {
                // to prevent producing an error if the user choose less than 4 items
                if (multiClickArray[i] != -1)
                {
                    if (InventoryBtns[multiClickArray[i]] == null) print("nulll");
                    MovetoOtherInventory(InventoryBtns[multiClickArray[i]].GetComponent<ItemInfo>().item);

                }
            }
            ResetMultiClickArray();
        }
        else
        {
            MovetoOtherInventory(currentClickedBtn.GetComponent<ItemInfo>().item);
        }

        Invoke("RefereshAllPanels", 0.25f);
    }

    // accessed by the transfer button
    public void MovetoOtherInventory(InventoryItem item)
    {
        otherInventory.AddItemToInventory(item);
        DecreementItemCount(item);
    }

    public void RefereshAllPanels()
    {
        RefreshPanel();
        otherInventory.gameObject.GetComponent<InventoryHandler>().RefreshPanel();
    }

    // once the use btn is clicked
    public void UseItems()
    {
        // checks if the user choose a single or multiple items
        if (multiClickArray[0] != -1)
        {
            for (int i = 0; i < multiClickArray.Length; i++)
            {
                // to prevent producing an error if the user choose less than 4 items
                if (multiClickArray[i] != -1)
                {
                    //print(multiClickArray[i]);
                    //print(InventoryBtns.Count);
                    if (InventoryBtns[multiClickArray[i]] == null) print("nulll");
                    var id = InventoryBtns[multiClickArray[i]].GetComponent<ItemInfo>().itemID;
                    InventoryBtns[multiClickArray[i]].GetComponent<IUseItemBehaviour>().Use();
                    DecreementItemCount(InventoryBtns[multiClickArray[i]].GetComponent<ItemInfo>().item);
                }
            }
            ResetMultiClickArray();
        }
        else
        {
            currentClickedBtn.GetComponent<IUseItemBehaviour>().Use();
            currentClickedBtnID = currentClickedBtn.GetComponent<ItemInfo>().itemID;
            print(InventoryBtns.Count);
            DecreementItemCount(InventoryBtns[InventoryBtns.IndexOf(currentClickedBtn)].GetComponent<ItemInfo>().item);
        }

        TurnOffBtnsHighlight();
        RefreshPanel();
    }

    public InventoryItem SearchForAnItem(string itemName)
    {
        foreach (var TempItem in InventoryItems)
        {
            if (itemName.Equals(TempItem.Key.itemName))
            {
                return TempItem.Key;
            }
        }

        return null;
    }

    // highlights the selected button
    public void HighlightBtn(Button clickedBtn)
    {
        // if the user choose more than one item
        if (multiClicksIsValid)
        {
            //print(multiClickCounter);
            // if the user choose more items than expected
            if (multiClickCounter >= 4)
            {
                TurnOffBtnsHighlight();
                ResetMultiClickArray();
                return;
            }
            currentClickedBtn = clickedBtn;
            clickedBtn.image.color = Color.white;
            multiClickArray[multiClickCounter] = currentClickedBtn.GetComponent<ItemInfo>().itemID;
            multiClickCounter++;
        }
        else
        {
            TurnOffBtnsHighlight();
            currentClickedBtn = clickedBtn;
            clickedBtn.image.color = Color.white;
        }
    }

    private void TurnOffBtnsHighlight()
    {
        foreach (var button in InventoryBtns)
        {
            button.image.color = Color.grey;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) // to choose up to 4
        {
            multiClicksIsValid = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)) // to choose up to 4
        {
            multiClicksIsValid = false;
        }
    }

    private void ResetMultiClickArray()
    {
        multiClickCounter = 0;

        for (int i = 0; i < multiClickArray.Length; i++)
        {
            multiClickArray[i] = -1;
        }
    }
}
