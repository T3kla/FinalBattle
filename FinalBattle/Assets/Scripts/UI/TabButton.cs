using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public TabGroup tabGroup;

    public Image tabImage;
    public Image tabLogo;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    // Start is called before the first frame update
    void Start()
    {
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();

            Vector3 aux = tabLogo.rectTransform.position;
            aux.y -= 4;
            tabLogo.transform.position = aux;
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();

            Vector3 aux = tabLogo.rectTransform.position;
            aux.y += 4;
            tabLogo.transform.position = aux;

        }
    }

}


