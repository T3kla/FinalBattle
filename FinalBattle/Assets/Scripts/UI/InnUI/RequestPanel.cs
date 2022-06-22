using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color IdleColor;
    public Color HoverColor;
    public Image background;

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = IdleColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
