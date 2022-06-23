using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static Logger;

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
    public GameSO gameSO = null;
    public MapSO mapSO = null;
    public ClassSO classSO = null;
    public Transform modelSocket = null;

    [Header(" · ReadOnly")]
    [ReadOnly] public Tile tile = null;

    protected virtual void Awake()
    {
        title = Namer.GetName();
        Each.Add(this);
        OnPawnClicked += OnSomePawnClicked;
    }

    protected virtual void Start()
    {
        if (mapSO)
            TeleportToClosestTile(mapSO);

        if (classSO)
            AssignClass(classSO);
    }

    protected virtual void OnDestroy()
    {
        Each.Remove(this);
        OnPawnClicked -= OnSomePawnClicked;
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

    protected virtual void OnSomePawnClicked(Pawn pawn) { }

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
        var speed = Mathf.Clamp((1f / classSO.speed) / 2f + 0.2f, 0.5f, 10f);

        foreach (Tile tile in path)
        {
            var cur = 0f;
            var dur = speed;

            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
                if (ct.IsCancellationRequested) return;

                cur += Time.deltaTime;
                var nor = cur / dur;

                transform.position = Vector3.Lerp(transform.position, tile.transform.position, nor);

                if (nor > dur)
                    break;
            }
        }
    }

}
