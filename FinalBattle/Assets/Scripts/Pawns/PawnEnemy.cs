using UnityEngine;

public class PawnEnemy : Pawn
{

    protected virtual Tile ChooseTarget()
    {
        Tile target = null;

        var maxScore = int.MaxValue;

        foreach (PawnPlayer pawnPlayer in Game.Players)
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
