using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOnboarding : Menu
{
    void Initialize()
    {
        gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            menuManager.ActivateMenu(1);
        }

    }
}
