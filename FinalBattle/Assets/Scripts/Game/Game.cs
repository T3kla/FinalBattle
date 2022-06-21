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

        var pawns = GameObject.FindObjectsOfType<Pawn>().ToList();

        if (pawns.Count > 0)
        {
            gameSO.initialized.runtime = true;
            gameSO.pawns = pawns;
            Log($"Initializing game with {pawns.Count} pawns");
        }
        else
        {
            gameSO.initialized.runtime = false;
            LogWarn("No pawns found");
        }

    }

}
