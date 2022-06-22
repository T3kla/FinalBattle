using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<Menu> menuList;
    public Menu currentMenu;


    public void ActivateMenu(int menuIndex) {

        if (menuList == null)
        {
            Debug.LogError("MenuManager: menuList is not declared.");
        }
        else if (menuList.Count < menuIndex || menuIndex < 0)
        {
            Debug.LogError("MenuManager: menuList does not have a component at [" + menuIndex + "].");
        }
        else {
            for (int i = 0; i < menuList.Count; i++)
            {
                if (i == menuIndex)
                {
                    menuList[i].gameObject.SetActive(true);
                    Debug.Log("Enabled " + menuList[i].gameObject.name);
                }
                else
                {
                    menuList[i].gameObject.SetActive(false);
                    Debug.Log("Disabled " + menuList[i].gameObject.name);
                }
            }
        }

    }




}
