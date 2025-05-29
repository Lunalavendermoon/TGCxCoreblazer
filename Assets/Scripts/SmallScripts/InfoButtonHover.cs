using UnityEngine;
using UnityEngine.EventSystems;

public class InfoButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject infoPopup;
    public void OnPointerEnter(PointerEventData mouseData)
    {
        infoPopup.SetActive(true);
    }

    public void OnPointerExit(PointerEventData mouseData)
    {
        infoPopup.SetActive(false);
    }
}
