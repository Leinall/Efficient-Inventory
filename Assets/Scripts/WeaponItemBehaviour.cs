using UnityEngine;

public class WeaponItemBehaviour : MonoBehaviour, IUseItemBehaviour
{

    private int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public void Use()
    {
        damage++;
        Debug.Log("Damage increased");
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}

public interface IUseItemBehaviour
{
    public void Use();
}
