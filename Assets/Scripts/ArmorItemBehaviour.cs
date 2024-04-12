using UnityEngine;

public class ArmorItemBehaviour : MonoBehaviour, IUseItemBehaviour
{
    private int defense;

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public void Use()
    {
        defense += 5;
        Debug.Log("Defense increased");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

}
