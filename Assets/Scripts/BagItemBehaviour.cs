using UnityEngine;

public class BagItemBehaviour : MonoBehaviour, IUseItemBehaviour
{
    public void Use()
    {
        this.GetComponent<BagItemComposite>().RevealItems();
        print("books are increased");
    }
}
