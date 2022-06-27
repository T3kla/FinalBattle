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

    public bool isDead => health <= 0;

    private GameSO gameSO => GameSO.Instance;
    private MapSO mapSO => MapSO.Instance;

    protected ETurnStep turn = ETurnStep.None;

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

    protected virtual async UniTask TurnMove(CancellationToken ct)
    {
        await UniTask.Delay(0);
        turn = ETurnStep.Move;
    }
    protected virtual async UniTask TurnAttack(CancellationToken ct)
    {
        await UniTask.Delay(0);
        turn = ETurnStep.Attack;
    }
    protected virtual async UniTask TurnWait(CancellationToken ct)
    {
        await UniTask.Delay(0);
        turn = ETurnStep.Wait;
    }

    public virtual async UniTask ReceiveDamage(int damage)
    {
        health -= damage;
        if (isDead)
        {
            await Death();
        }
    }
    protected virtual async UniTask Death() => await UniTask.Delay(0);

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

    protected async UniTask Attack(CancellationToken ct, Tile _tile, int damage)
    {
        float nor = 0f, cur = 0f, dur = 0.15f;

        var oldPos = transform.position;
        var dir = (_tile.transform.position - transform.position) / 2f; dir.y = 0;

        var spawnedFloatingText = false;

        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, ct);
            if (ct.IsCancellationRequested) return;

            cur += Time.deltaTime;
            nor = cur / dur;

            // Spawn floating text
            if (cur > 0.05f && !spawnedFloatingText)
            {
                spawnedFloatingText = true;
                var ft = Instantiate(gameSO.floatingText, _tile.transform.position + Vector3.up * 2f, Quaternion.identity)
                        .GetComponent<FloatingText>();
                ft.Activate(damage.ToString());
            }

            // Attack animation
            if (cur < 0.05f)
                transform.position = Vector3.Lerp(oldPos, oldPos + dir, nor);
            else
                transform.position = Vector3.Lerp(oldPos + dir, oldPos, nor);

            if (cur > dur) break;
        }

    }

}
