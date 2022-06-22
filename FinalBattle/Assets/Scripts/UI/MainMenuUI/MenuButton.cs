using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public Image arrowSelection;
    public UnityEvent onButtonClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        onButtonClicked.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowSelection.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrowSelection.gameObject.SetActive(false);
    }

}
