using System.Collections.Generic;

public class BagItemComposite : Component
{
    protected List<Component> booksIheBag = new List<Component>();

    private InventoryHandler mainInv;
    private void Start()
    {
        mainInv = GameManager.Instance.inventoryHandlerMain;

        // random range for adding random number of books each time
        for (int i = 0; i < UnityEngine.Random.Range(1, 6); i++)
        {
            booksIheBag.Add(GameManager.Instance.book.GetComponent<Component>());
        }
    }

    // The bag implements its own reveal items which is adding the number of books it created in the start randomly
    public override void RevealItems()
    {
        foreach (var item in booksIheBag)
        {
            mainInv.AddItemToInventory(mainInv.SearchForAnItem("Book"));
        }
    }

    public void Add(Component component)
    {
        this.booksIheBag.Add(component);
    }

    public void Remove(Component component)
    {
        this.booksIheBag.Remove(component);
    }
}
