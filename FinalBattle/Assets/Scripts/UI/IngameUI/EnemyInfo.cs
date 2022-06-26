using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnemyInfo : MonoBehaviour
{

    public Image background;
    public Image unitSprite;
    public TMP_Text unitName;

    public void SetEnemyInfo(Pawn pawn)
    {

        if (pawn.@class) {
            unitSprite.sprite = pawn.@class.uiSprite;
        }

        var allyPlayer = pawn as PawnPlayer;
        if (allyPlayer != null)
        {
            background.color = Color.blue;
        }
        else
        {
            background.color = Color.red;
        }

        unitName.text = pawn.title;
    }

}
