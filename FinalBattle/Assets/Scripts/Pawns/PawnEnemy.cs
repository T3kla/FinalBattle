using UnityEngine;

public class PawnEnemy : Pawn
{

    protected virtual Tile ChooseTarget()
    {
        Tile target = null;

        var maxScore = int.MaxValue;
        var pawnsPlayer = gameSO.pawnsPlayer;

        foreach (PawnPlayer pawnPlayer in pawnsPlayer)
        {
            var score = Pathfinder.ComputeHScore(tile, pawnPlayer.tile);

            if (score < maxScore)
            {
                target = pawnPlayer.tile;
                maxScore = score;
            }
        }

        return target;
    }

}
