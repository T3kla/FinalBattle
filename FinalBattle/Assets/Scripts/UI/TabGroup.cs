using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabButtons;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    public TabButton selectedTab;

    public List<GameObject> pagesToSwap;

    public void Subscribe(TabButton button) 
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.tabImage.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
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
        foreach (TabButton button in tabButtons) 
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
