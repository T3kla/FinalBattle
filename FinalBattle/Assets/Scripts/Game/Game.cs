using System.Linq;
using UnityEngine;
using static Logger;

public class Game : MonoBehaviour
{

    public GameSO gameSO = null;

    public void Awake()
    {
        if (!gameSO)
        {
            LogWarn("GameSO is null");
            return;
        }

        gameSO.pawnsPlayer = GameObject.FindObjectsOfType<PawnPlayer>().ToList();
        gameSO.pawnsEnemy = GameObject.FindObjectsOfType<PawnEnemy>().ToList();

        if (gameSO.pawnsPlayer.Count > 0 || gameSO.pawnsEnemy.Count > 0)
        {
            gameSO.initialized.runtime = true;
            Log($"Initializing game with {gameSO.pawnsPlayer.Count} player pawns");
            Log($"Initializing game with {gameSO.pawnsEnemy.Count} enemy pawns");
        }
        else
        {
            gameSO.initialized.runtime = false;
            LogWarn("No pawns found");
        }
    }

}
