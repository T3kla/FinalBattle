using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Logger;

public class Game : MonoBehaviour
{

    public GameSO gameSO = null;

    private float angleTrg = 0f;
    private float angleCur = 0f;

    private Vector3 posTrg = Vector3.one;
    private Vector3 posCur = Vector3.one;

    private void Awake()
    {
        if (!gameSO)
        {
            LogWarn("GameSO is null");
            return;
        }

        gameSO.pawnsPlayer = GameObject.FindObjectsOfType<PawnPlayer>().ToList();
        gameSO.pawnsEnemy = GameObject.FindObjectsOfType<PawnEnemy>().ToList();
        gameSO.camera = GameObject.FindObjectOfType<Camera>();

        if (gameSO.pawnsPlayer.Count > 0)
        {
            gameSO.currentPawn = gameSO.pawnsPlayer[0];
            Log($"Initializing game with {gameSO.pawnsPlayer.Count} player pawns");
            Log($"Initializing game with {gameSO.pawnsEnemy.Count} enemy pawns");
        }
        else
        {
            LogWarn("No pawns found");
        }
    }

    private void Update()
    {
        if (gameSO.camera)
            UpdateCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var list = new List<Pawn>(gameSO.pawnsPlayer.Count + gameSO.pawnsEnemy.Count);
            list.AddRange(gameSO.pawnsPlayer);
            list.AddRange(gameSO.pawnsEnemy);

            gameSO.currentPawn = list[Random.Range(0, list.Count)];
            posTrg = gameSO.currentPawn.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            angleTrg += 90f;

            if (angleTrg > 360f)
                angleTrg -= 360f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            angleTrg -= 90f;

            if (angleTrg < 0f)
                angleTrg += 360f;
        }
    }

    private void UpdateCamera()
    {
        var camera = gameSO.camera;

        if (!camera)
            return;

        var pawnPos = gameSO.currentPawn.transform.position;

        posCur = Vector3.Lerp(posCur, posTrg, Time.deltaTime * gameSO.camMoveSpeed);
        angleCur = Mathf.LerpAngle(angleCur, angleTrg, Time.deltaTime * gameSO.camLookSpeed);

        var camPosNew = Quaternion.AngleAxis(angleCur, Vector3.up) * Vector3.one * gameSO.camDistance + posCur;

        camera.transform.position = camPosNew;
        camera.transform.LookAt(posCur, Vector3.up);
    }

}
