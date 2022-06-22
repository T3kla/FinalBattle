using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


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
        if (pawn.classSO == cwarrior) {
            imageWarrior.gameObject.SetActive(true);
        }
        else if (pawn.classSO == carcher)
        {
            imageArcher.gameObject.SetActive(true);
        }
        else if (pawn.classSO == clancer)
        {
            imageLancer.gameObject.SetActive(true);
        }
        else if (pawn.classSO == cthieve)
        {
            imageThieve.gameObject.SetActive(true);
        }

        var allyPlayer = pawn as PawnPlayer;
        if(allyPlayer != null) 
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
