using UnityEngine;

public abstract class Component : MonoBehaviour
{
    public virtual void RevealItems()
    {
        print("The component");
    }

}
