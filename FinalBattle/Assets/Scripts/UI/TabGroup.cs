using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{

    public List<CustomTabButton> tabButtons;
    public List<CustomTabButton> tabButtons2;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    public CustomTabButton selectedTab;

    public List<GameObject> pagesToSwap;

    public void Subscribe(CustomTabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<CustomTabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(CustomTabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.tabImage.sprite = tabHover;
        }
    }

    public void OnTabExit(CustomTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(CustomTabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }


        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.tabImage.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < pagesToSwap.Count; i++)
        {
            if (i == index)
            {
                pagesToSwap[i].SetActive(true);
            }
            else
            {
                pagesToSwap[i].SetActive(false);
            }
        }

    }

    public void ResetTabs()
    {
        foreach (CustomTabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            else
            {
                button.tabImage.sprite = tabIdle;
            }
        }
    }

}
