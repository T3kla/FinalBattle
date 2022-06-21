using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Logger;

public class Game : MonoBehaviour
{

    public GameSO gameSO = null;

    private float cameraAngleTarget = 45;
    private float cameraAngleCurrent = 45;

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
            gameSO.initialized.runtime = true;
            gameSO.currentPawn = gameSO.pawnsPlayer[0];
            Log($"Initializing game with {gameSO.pawnsPlayer.Count} player pawns");
            Log($"Initializing game with {gameSO.pawnsEnemy.Count} enemy pawns");
        }
        else
        {
            gameSO.initialized.runtime = false;
            LogWarn("No pawns found");
        }
    }

    private void Update()
    {
        if (!gameSO.initialized.runtime)
            return;

        if (gameSO.camera)
            UpdateCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var list = new List<Pawn>(gameSO.pawnsPlayer.Count + gameSO.pawnsEnemy.Count);
            list.AddRange(gameSO.pawnsPlayer);
            list.AddRange(gameSO.pawnsEnemy);

            gameSO.currentPawn = list[Random.Range(0, list.Count)];
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            cameraAngleTarget += 90f;

            if (cameraAngleTarget > 360f)
                cameraAngleTarget -= 360f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            cameraAngleTarget -= 90f;

            if (cameraAngleTarget < 0f)
                cameraAngleTarget += 360f;
        }
    }

    private void UpdateCamera()
    {
        var camera = gameSO.camera;

        if (!camera)
            return;

        cameraAngleCurrent = Mathf.Lerp(
            cameraAngleCurrent,
            cameraAngleTarget,
            Time.deltaTime * gameSO.cameraLookSpeed);

        var pawnPos = gameSO.currentPawn?.transform.position ?? Vector3.zero;
        var camPos = camera.transform.position;

        var targetPos = pawnPos + Vector3.one * gameSO.cameraDistance;

        targetPos = (camPos - pawnPos).RotateAround(Vector3.up, cameraAngleCurrent) + pawnPos;

        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            targetPos,
            gameSO.cameraMoveSpeed * Time.deltaTime);

        camera.transform.rotation.SetLookRotation((pawnPos - camPos), Vector3.up);
    }

}
