using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Logger;

public class Game : MonoBehaviour
{

    public static Action OnGameStarted = null;
    public static Action<int> OnNextTurn = null;

    public static List<PawnPlayer> Players = new List<PawnPlayer>();
    public static List<PawnEnemy> Enemies = new List<PawnEnemy>();
    public static List<Pawn> Initiative = new List<Pawn>();
    public static Camera Cam = null;
    public static int InitiativeTracker = -1;

    public GameSO gameSO = null;

    private float angleTrg = 0f;
    private float angleCur = 0f;

    private Vector3 posTrg = Vector3.one;
    private Vector3 posCur = Vector3.one;

    private Vector3 v_up = new Vector3(-1, 0, -1);
    private Vector3 v_down = new Vector3(1, 0, 1);
    private Vector3 v_right = new Vector3(-1, 0, 1);
    private Vector3 v_left = new Vector3(1, 0, -1);

    private void Start()
    {
        if (!gameSO)
        {
            LogWarn("GameSO is null");
            return;
        }

        Players = new List<PawnPlayer>();
        Enemies = new List<PawnEnemy>();

        foreach (var pawn in Pawn.Each)
        {
            var pawnPlayer = pawn as PawnPlayer;

            if (pawnPlayer)
                Players.Add(pawnPlayer);
        }

        foreach (var pawn in Pawn.Each)
        {
            var pawnEnemy = pawn as PawnEnemy;

            if (pawnEnemy)
                Enemies.Add(pawnEnemy);
        }

        Initiative = Pawn.Each?.OrderByDescending(pawn => pawn.classSO.speed).ToList() ?? new List<Pawn>();

        NextPawnTurn();

        Cam = Camera.main;

        if (Players.Count > 0)
        {
            gameSO.currentPawn = Players[0];
            Log($"Initializing game with {Players.Count} players and {Enemies.Count} enemies");
        }
        else
        {
            LogWarn($"No pawns found");
        }

        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (Cam)
            UpdateCamera();

        if (Input.GetKeyDown(KeyCode.Space))
            posTrg = gameSO.currentPawn.transform.position;

        if (Input.GetKeyDown(KeyCode.Q))
            AddAngleTarget(90f);

        if (Input.GetKeyDown(KeyCode.E))
            AddAngleTarget(-90f);

        var quat = Quaternion.AngleAxis(angleCur, Vector3.up);

        if (Input.GetKey(KeyCode.W))
            posTrg += quat * v_up * gameSO.camMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            posTrg += quat * v_down * gameSO.camMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            posTrg += quat * v_left * gameSO.camMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            posTrg += quat * v_right * gameSO.camMoveSpeed * Time.deltaTime;
    }

    private void AddAngleTarget(float angles)
    {
        angleTrg += angles;

        if (angleTrg > 360f)
            angleTrg -= 360f;
        else if (angleTrg < 0f)
            angleTrg += 360f;
    }

    private void NextPawnTurn()
    {
        InitiativeTracker = InitiativeTracker < Initiative.Count - 1 ? InitiativeTracker + 1 : 0;

        gameSO.currentPawn = Initiative[InitiativeTracker];
        gameSO.currentPawnTitle = gameSO.currentPawn?.title ?? null;
        posTrg = gameSO.currentPawn.transform.position;

        OnNextTurn?.Invoke(InitiativeTracker);
        gameSO.currentPawn.Turn(PawnFinishedTurn).Forget();
    }

    public void PawnFinishedTurn()
    {
        NextPawnTurn();
    }

    private void UpdateCamera()
    {
        var camera = Cam;

        if (!camera)
            return;

        var pawnPos = gameSO.currentPawn?.transform.position ?? Vector3.zero;

        posCur = Vector3.Lerp(posCur, posTrg, Time.deltaTime * gameSO.camMoveSpeed);
        angleCur = Mathf.LerpAngle(angleCur, angleTrg, Time.deltaTime * gameSO.camLookSpeed);

        var camPosNew = Quaternion.AngleAxis(angleCur, Vector3.up) * Vector3.one * gameSO.camDistance + posCur;

        camera.transform.position = camPosNew;
        camera.transform.LookAt(posCur, Vector3.up);
    }

}
