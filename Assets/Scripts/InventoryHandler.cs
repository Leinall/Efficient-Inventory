using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour // handle the player getting an item more than he wants
{
    public Transform InvenoryParent;
    public PlayerInventoryData InventoryData;

    [SerializeField]
    private InventoryHandler otherInventory;

    private Button currentClickedBtn; // I can make it an array and manage it by input
    private int currentClickedBtnID = -1;

    private List<Button> InventoryBtns = new List<Button>();
    private Dictionary<InventoryItem, int> InventoryItems = new Dictionary<InventoryItem, int>();
    private int numberOfClicks = 0;
    private int NoOfItemsCreated = 0;

    // for multiclicking
    private int[] multiClickArray = new int[4] { -1, -1, -1, -1 };
    private int multiClickCounter = 0;
    private bool multiClicksIsValid;
    private void Awake()
    {

    }

    private void OnEnable()
    {
        PanelInitiation();
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

        //Invoke("RefreshPanel", 0.5f);
        //Invoke("test", 5f);
        //Invoke("RefreshPanel", 6f);
    }

    public void test() // adding item after deleting it
    {
        //print("test");
        AddItemToInventory(SearchForAnItem("Bag"), 3);
    }

    public void RemovingAllChildrenInPanel()
    {
        for (int i = 0; i < InvenoryParent.childCount; i++)
        {
            Destroy(InvenoryParent.GetChild(i).gameObject);
        }
    }

    // it reads the initial data from the scriptable object that shall be changed to any form of data storage if we are willing to store the player progress
    public void PanelInitiation()
    {
        for (int i = 0; i < InventoryData.inventoryItems.Length; i++)
        {
            if (InventoryData.inventoryItems[i].itemCount >= 0 &&
                InventoryData.inventoryItems[i].itemCount < InventoryData.inventoryItems[i].inventoryItem.stackSize)
            {
                var key = InventoryData.inventoryItems[i].inventoryItem;
                InventoryItems[key] = InventoryData.inventoryItems[i].itemCount;
                /////print("###" + this.gameObject.name + "," + key.itemName);

                if (InventoryItems[key] > 0)
                {
                    // the item here shall take the reference of the button since we add all in the same order
                    var item = GameObject.Instantiate(InventoryData.inventoryItems[i].inventoryItem.ItemPrefab, InvenoryParent.transform);//InventoryBtns[i];
                    item.GetComponentInChildren<TextMeshProUGUI>().text
                       = InventoryData.inventoryItems[i].itemCount + "";
                    item.GetComponent<ItemInfo>().itemID = NoOfItemsCreated++;

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

            }
        }

        InventoryBtns = InvenoryParent.GetComponentsInChildren<Button>().ToList();
    }

    // it reads from the dictionary to track any changes happened during play time
    public void RefreshPanel()
    {
        //print("RefreshPanel");
        NoOfItemsCreated = 0;

        foreach (var btn in InventoryBtns)
        {
            btn.onClick.RemoveAllListeners();
        }

        RemovingAllChildrenInPanel();

        #region first approach
        // first apporoach not to delete and instantiate
        // but it requires saving each item with it's associated button , complex data structures can be used like using a dictionary item
        // as nother dictionary key and assign the button at the beginig, but here I'll keep things simple - requires a lot of modification-
        //int btnsCounter = 0;
        //foreach (var item in InventoryItems)
        //{
        //    if (item.Value > 0 && item.Value < item.Key.stackSize)
        //    {

        //        var itemtemp = InventoryBtns[btnsCounter];
        //        itemtemp.GetComponentInChildren<TextMeshProUGUI>().text
        //           = InventoryItems[item.Key] + "";
        //        //itemtemp.GetComponentInChildren<Button>().onClick.AddListener(() =>
        //        //{
        //        //    HighlightBtn(itemtemp.GetComponentInChildren<Button>());
        //        //});

        //    }
        //    btnsCounter++;
        //}

        //InventoryBtns = InvenoryParent.GetComponentsInChildren<Button>().ToList();
        #endregion

        for (int i = 0; i < InventoryData.inventoryItems.Length; i++)
        {
            if (InventoryData.inventoryItems[i].itemCount >= 0 &&
                InventoryData.inventoryItems[i].itemCount < InventoryData.inventoryItems[i].inventoryItem.stackSize)
            {
                var key = InventoryData.inventoryItems[i].inventoryItem;
                var value = InventoryItems[InventoryData.inventoryItems[i].inventoryItem];
                //////print(this.gameObject.name + ": " + key + ", " + value);
                if (InventoryItems.ContainsKey(key) && value > 0)
                {
                    //InventoryItems[key] = InventoryData.inventoryItems[i].itemCount;
                    // the item here shall take the reference of the button since we add all in the same order
                    var item = GameObject.Instantiate(InventoryData.inventoryItems[i].inventoryItem.ItemPrefab, InvenoryParent.transform);//InventoryBtns[i];
                    item.GetComponentInChildren<TextMeshProUGUI>().text
                       = value + "";
                    item.GetComponent<ItemInfo>().itemID = NoOfItemsCreated++;

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

            }
        }
        //Unity takes some time to destroy the gameobjects and creating the new ones if the invoke is removed
        // it shall dtetcted doubled list 
        Invoke("ReassigningTheBtns", 0.5f);
    }

    public void ReassigningTheBtns()
    {
        InventoryBtns.Clear();
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

    public void RemoveItemFromInventory(InventoryItem item)
    {
        if (item == null) return;
        InventoryItems[item] = 0;
    }

    public void DecreementItemCount(InventoryItem item) // item count--;
    {
        if (item == null) return;
        InventoryItems[item]--;
        if (InventoryItems[item] < 0)
        {
            InventoryItems[item] = 0;
        }
    }

    public void Transfer()
    {
        currentClickedBtnID = currentClickedBtn.GetComponent<ItemInfo>().itemID;
        MovetoOtherInventory(InventoryData.inventoryItems[currentClickedBtnID].inventoryItem);
    }

    public void MovetoOtherInventory(InventoryItem item) // accessed by the transfer button
    {
        otherInventory.AddItemToInventory(item);
        DecreementItemCount(item);

        Invoke("RefereshAllPanels", 0.5f);
    }

    public void RefereshAllPanels()
    {
        RefreshPanel();
        otherInventory.gameObject.GetComponent<InventoryHandler>().RefreshPanel();
    }

    // once the use btn is clicked
    public void UseItems() // from a button
    {
        if (multiClickArray[0] != -1)
        {
            print("in the right position");
            for (int i = 0; i < multiClickArray.Length; i++)
            {
                print("forts " + multiClickArray[i]);
                if (multiClickArray[i] != -1)
                {
                    print(multiClickArray[i]);
                    print(InventoryBtns.Count);
                    if (InventoryBtns[multiClickArray[i]] == null) print("nalll");
                    var id = InventoryBtns[multiClickArray[i]].GetComponent<ItemInfo>().itemID;
                    InventoryBtns[multiClickArray[i]].GetComponent<IUseItemBehaviour>().Use();
                    DecreementItemCount(
                        SearchForAnItem(InventoryBtns[multiClickArray[i]].GetComponent<ItemInfo>().ItemName()));
                }
            }
            ResetMultiClickArray();
        }
        else
        {
            currentClickedBtn.GetComponent<IUseItemBehaviour>().Use();
            currentClickedBtnID = currentClickedBtn.GetComponent<ItemInfo>().itemID;
            DecreementItemCount(InventoryData.inventoryItems[currentClickedBtnID].inventoryItem);
        }

        TurnOffBtnsHighlight();
        RefreshPanel();
    }






    public InventoryItem SearchForAnItem(string itemName)
    {
        // as long as our array is unsorted the best option is the linear search but we can optimize it by reducing
        // the O(n) by multiple checking in the same loop but since our inventory items is not that much I'll check 
        // one item a time
        //print(InventoryItems.Count);
        foreach (var TempItem in InventoryItems)
        {
            //print(itemName + "," + TempItem.Key.itemName);
            if (itemName.Equals(TempItem.Key.itemName))
            {
                //print("weeee");
                return TempItem.Key;
            }
        }

        return null;
    }


    public void HighlightBtn(Button clickedBtn)
    {

        if (multiClicksIsValid)
        {
            print(multiClickCounter);
            if (multiClickCounter > 4) { TurnOffBtnsHighlight(); return; }
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

        //currentClickedBtnID[0] = currentClickedBtn
        TurnONSelectedBtnsHighlight();
    }

    private void TurnOffBtnsHighlight()
    {
        foreach (var button in InventoryBtns)
        {
            button.image.color = Color.grey;
        }
    }

    private void TurnONSelectedBtnsHighlight()
    {


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
