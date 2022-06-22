using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PawnEnemy : Pawn
{


    protected virtual Tile ChooseTarget()
    {
        Tile target = null;
        int shortestDist = int.MaxValue;
        List<PawnPlayer> pawnsPlayer = gameSO.pawnsPlayer;
        foreach (PawnPlayer pawnPlayer in pawnsPlayer)
        {
            int tempDist = ComputeHScore(tile, pawnPlayer.tile);
            if (tempDist < shortestDist)
            {
                target = pawnPlayer.tile;
                shortestDist = tempDist;
            }
        }
        return target;
    }


}
