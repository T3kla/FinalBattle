using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static TBox.Logger;

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
    public ClassSO classSO = null;
    public Transform modelSocket = null;

    [Header(" · ReadOnly")]
    [ReadOnly] public Tile tileUnder = null;

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
            await Death();
    }
    protected virtual async UniTask Death() => await Die();

    // Useful methods

    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {
        OnPawnClicked?.Invoke(this);
    }

    public virtual void ShowTilesInMovingRange() { }

    public void AssignClass(ClassSO classSO)
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

    public void TeleportToClosestTile(MapSO map)
    {
        var coord = new Coord(transform.position / map.tileSize);
        var tiles = new List<Tile>(Tile.Each);

        tiles.Sort((a, b) => (a.coord - coord).CompareTo((b.coord - coord)));

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].pawn)
                continue;

            tileUnder = tiles[i];
            tiles[i].pawn = this;
            transform.position = tiles[i].transform.position;
            break;
        }
    }

    protected async UniTask WalkPath(CancellationToken ct, List<Tile> path)
    {
        var cur = 0f;
        var dur = 0f;
        var lookDur = 0.15f;
        var moveDur = Mathf.Clamp((1f / classSO.speed) / 2f + 0.2f, 0.5f, 10f);

        foreach (Tile tile in path)
        {
            // Look

            cur = 0f;
            dur = lookDur;

            var dir = tile.transform.position - transform.position; dir.y = 0;
            var rotOld = transform.rotation;
            var rotNew = Quaternion.LookRotation(dir, Vector3.up);

            if (rotOld != rotNew)
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                    if (ct.IsCancellationRequested) return;
                    cur += Time.deltaTime;

                    transform.rotation = Quaternion.Lerp(rotOld, rotNew, cur / dur);

                    if (cur > dur) break;
                }

            transform.rotation = rotNew;

            // Move

            cur = 0f;
            dur = moveDur;

            var posOld = transform.position;
            var posNew = tile.transform.position;

            if (posOld != posNew)
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                    if (ct.IsCancellationRequested) return;
                    cur += Time.deltaTime;

                    transform.position = Vector3.Lerp(posOld, posNew, cur / dur);

                    if (cur > dur) break;
                }

            transform.position = posNew;
        }
    }

    protected async UniTask Attack(CancellationToken ct, Tile tile, int damage)
    {
        var cur = 0f;
        var dur = 0f;
        var dir = Vector3.zero;
        var lookDur = 0.15f;
        var attackDur = 0.45f;

        var spawnedFloatingText = false;

        while (true)
        {
            // Look

            cur = 0f;
            dur = lookDur;

            dir = tile.transform.position - transform.position; dir.y = 0;
            var rotOld = transform.rotation;
            var rotNew = Quaternion.LookRotation(dir, Vector3.up);

            if (rotOld != rotNew)
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                    if (ct.IsCancellationRequested) return;
                    cur += Time.deltaTime;

                    transform.rotation = Quaternion.Lerp(rotOld, rotNew, cur / dur);

                    if (cur > dur) break;
                }

            transform.rotation = rotNew;

            // Attack

            cur = 0f;
            dur = attackDur;

            dir = (tile.transform.position - transform.position) / 2f; dir.y = 0;
            var posOld = transform.position;
            var posNew = transform.position + dir.normalized;

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
            if (ct.IsCancellationRequested) return;

            if (posOld != posNew)
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                    if (ct.IsCancellationRequested) return;
                    cur += Time.deltaTime;

                    // Spawn floating text
                    if (cur > 0.05f && !spawnedFloatingText)
                        spawnedFloatingText = SpawnFloatingText(tile.transform.position + Vector3.up * 2f, damage);

                    // Attack animation
                    if (cur < 0.05f)
                        transform.position = Vector3.Lerp(posOld, posNew, cur / dur);
                    else
                        transform.position = Vector3.Lerp(posNew, posOld, cur / dur);

                    if (cur > dur) break;
                }

            transform.position = posOld;

            if (cur > dur) break;
        }
    }

    protected async UniTask Die()
    {
        await UniTask.Delay(500);

        Log($"{title} died");

        // Initiative correction
        var initiative = 0;

        for (int i = 0; i < Game.Initiative.Count; i++)
            if (Game.Initiative[i] == this)
            {
                initiative = i;
                break;
            }

        if (initiative < Game.InitiativeTracker)
            Game.InitiativeTracker--;

        // List cleanup
        Game.Players.Remove(this as PawnPlayer);
        Game.Enemies.Remove(this as PawnEnemy);
        Game.Initiative.Remove(this);

        // Tile clieanup
        tileUnder.pawn = null;
        tileUnder = null;

        // Destruction
        Destroy(gameObject);
    }

    private bool SpawnFloatingText(Vector3 pos, int damage)
    {
        var obj = Instantiate(gameSO.floatingText, pos, Quaternion.identity);
        var scr = obj.GetComponent<FloatingText>();
        scr.Activate(damage.ToString());
        return true;
    }

}
