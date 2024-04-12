using UnityEngine;

public class PotionItemBehaviour : MonoBehaviour, IUseItemBehaviour
{
    private int health;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public void Use()
    {
        health += 10;
        Debug.Log("Health is healed");
    }

    // Start is called before the first frame update
    void Start()
    {

    }


}
