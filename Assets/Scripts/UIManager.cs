using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform btn;
    private Vector3 defaultPlace;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimateUI()
    {

    }

    private void FadeIn(Transform transformUI)
    {
        defaultPlace = transformUI.position;
        transformUI.transform.DOScaleX(0, 1);
        transformUI.transform.DOMove(btn.position, 1);
    }

    private void FadeOut(Transform transformUI)
    {
        transformUI.transform.DOScaleX(1, 1);
        transformUI.transform.DOMove(defaultPlace, 1);
    }
}
