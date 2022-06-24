using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnemyInfo : MonoBehaviour
{

    public Image background;
    public TMP_Text unitName;

    public Image imageWarrior;
    public Image imageArcher;
    public Image imageLancer;
    public Image imageThieve;

    public ClassSO cwarrior;
    public ClassSO carcher;
    public ClassSO clancer;
    public ClassSO cthieve;


    private void DrawEnemyInfo(Pawn pawn)
    {
        if (pawn.@class == cwarrior)
        {
            imageWarrior.gameObject.SetActive(true);
        }
        else if (pawn.@class == carcher)
        {
            imageArcher.gameObject.SetActive(true);
        }
        else if (pawn.@class == clancer)
        {
            imageLancer.gameObject.SetActive(true);
        }
        else if (pawn.@class == cthieve)
        {
            imageThieve.gameObject.SetActive(true);
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

        unitName.text = pawn.name;

    }



}
