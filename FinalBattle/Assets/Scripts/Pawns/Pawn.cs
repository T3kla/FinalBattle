using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TBox;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Pawn : MonoBehaviour, IPointerClickHandler
{

    public static List<Pawn> Each = new List<Pawn>(20);
    public static Action<Pawn> OnPawnClicked = null;

    [Header(" · Variables")]
    [ReadOnly] public string title = null;
    public int health = 1;
    public int mana = 1;

    [Header(" · Assignables")]
    public ClassSO @class = null;
    public Transform modelSocket = null;

    [Header(" · ReadOnly")]
    [ReadOnly] public Tile tile = null;

    private GameSO gameSO => GameSO.Instance;
    private MapSO mapSO => MapSO.Instance;

    protected virtual void Awake()
    {
        title = Namer.GetName();
        Each.Add(this);
    }

    protected virtual void Start()
    {
        if (mapSO)
            TeleportToClosestTile(mapSO);

        if (@class)
            AssignClass(@class);
    }

    protected virtual void OnDestroy()
    {
        Each.Remove(this);
    }

    // Turn

    public virtual async UniTask Turn(CancellationToken ct, Action OnTurnEnded)
    {
        await TurnMove(ct);
        await TurnAttack(ct);
        await TurnWait(ct);

        if (!ct.IsCancellationRequested)
            OnTurnEnded?.Invoke();
    }

    protected virtual async UniTask TurnMove(CancellationToken ct) => await UniTask.Delay(0);
    protected virtual async UniTask TurnAttack(CancellationToken ct) => await UniTask.Delay(0);
    protected virtual async UniTask TurnWait(CancellationToken ct) => await UniTask.Delay(0);

    // Useful methods

    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {
        OnPawnClicked?.Invoke(this);
    }

    public virtual void ShowTilesInMovingRange() { }

    protected virtual void AssignClass(ClassSO classSO)
    {
        health = classSO.health;
        mana = classSO.mana;

        // Destroy model
        if (modelSocket && modelSocket.childCount > 0)
            foreach (Transform child in modelSocket)
                Destroy(child.gameObject);

        // Attach model
        if (classSO.model && modelSocket)
        {
            var model = Instantiate(classSO.model.transform, modelSocket).GetComponent<Model>();

            if (model.RightWeaponSocket && classSO.weaponR)
                Instantiate(classSO.weaponR, model.RightWeaponSocket);
            if (model.LeftWeaponSocket && classSO.weaponL)
                Instantiate(classSO.weaponL, model.LeftWeaponSocket);
            foreach (var dyeable in model.Dyeables)
            {
                var dye = this as PawnPlayer;
                dyeable.GetComponent<MeshRenderer>().material = dye ? gameSO.playerDye : gameSO.enemyDye;
            }
        }
    }

    protected virtual void TeleportToClosestTile(MapSO map)
    {
        var coord = new Coord(transform.position / map.tileSize);
        var tiles = new List<Tile>(Map.Tiles.Values);

        tiles.Sort((a, b) => (a.coord - coord).CompareTo((b.coord - coord)));

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].pawn)
                continue;

            this.tile = tiles[i];
            tiles[i].pawn = this;
            transform.position = tiles[i].transform.position;
            break;
        }
    }

    protected async UniTask WalkPath(CancellationToken ct, List<Tile> path)
    {
        var lookDur = 0.15f;
        var moveDur = Mathf.Clamp((1f / @class.speed) / 2f + 0.2f, 0.5f, 10f);

        foreach (Tile tile in path)
        {
            float cur = 0f, dur = 0f;

            while (true)
            {
                cur = 0f;
                dur = lookDur;

                var dir = tile.transform.position - transform.position; dir.y = 0;
                var rotOld = transform.rotation;
                var rotNew = Quaternion.LookRotation(dir, Vector3.up);

                if (rotOld != rotNew)
                    while (true) // Look
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, ct);
                        if (ct.IsCancellationRequested) return;
                        cur += Time.deltaTime;

                        transform.rotation = Quaternion.Lerp(rotOld, rotNew, cur / dur);
                        Debug.DrawRay(transform.position, dir, Color.red, 0.5f);

                        if (cur > dur) break;
                    }

                transform.rotation = rotNew;

                cur = 0f;
                dur = moveDur;

                var posOld = transform.position;
                var posNew = tile.transform.position;

                if (posOld != posNew)
                    while (true) // Move
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, ct);
                        if (ct.IsCancellationRequested) return;
                        cur += Time.deltaTime;

                        transform.position = Vector3.Lerp(posOld, posNew, cur / dur);

                        if (cur > dur) break;
                    }

                transform.position = posNew;

                break;
            }
        }
    }

}
