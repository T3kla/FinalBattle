using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static Logger;

[SelectionBase]
public class Pawn : MonoBehaviour, IPointerClickHandler
{

    public static List<Pawn> Each = new List<Pawn>(20);

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
    }

    protected virtual void AssignClass(ClassSO cls)
    {
        health = cls.health;
        mana = cls.mana;

        // Destroy model
        if (modelSocket && modelSocket.childCount > 0)
            foreach (Transform child in modelSocket)
                Destroy(child.gameObject);

        // Attach model
        if (cls.model && modelSocket)
        {
            var model = Instantiate(cls.model.transform, modelSocket).GetComponent<Model>();

            if (model.RightWeaponSocket && cls.weaponR)
                Instantiate(cls.weaponR, model.RightWeaponSocket);
            if (model.LeftWeaponSocket && cls.weaponL)
                Instantiate(cls.weaponL, model.LeftWeaponSocket);
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

    protected virtual async UniTask MoveTo(Tile target)
    {
        for (int current = 0; current < 200; current += 16)
        {
            await UniTask.Delay(16);
            var normalizedTime = current / 200f;
            Vector3 newMove = Vector3.Lerp(transform.position, target.transform.position, normalizedTime);
            transform.position = newMove;
        }

        tile = target;
    }

    protected virtual async UniTask Attack(Pawn target)
    {
        await UniTask.Delay(0);

        List<Tile> tilesInRange = null;

        if (tilesInRange.FirstOrDefault(t => t.coord.x == target.tile.coord.x && t.coord.z == target.tile.coord.z) != null)
        {
            // TODO: Attack animation
            // TODO: Dodge stuff
            target.health -= (classSO.attack - classSO.defence);
        }
    }

    protected virtual async UniTask Wait()
    {
        await UniTask.Delay(0);

        // TODO: Create buttons to choose direction and pass turn
    }

    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

}
