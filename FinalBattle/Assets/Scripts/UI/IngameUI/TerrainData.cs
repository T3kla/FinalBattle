using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainData : MonoBehaviour
{

    public Image TerrainSprite;
    public Image UnitSprite;

    public TMP_Text Unit_HP;
    public TMP_Text Unit_Mana;
    public TMP_Text Terrain_Height;

    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite groundSprite;
    [SerializeField]
    private Sprite grassSprite;
    [SerializeField]
    private Sprite waterSprite;
    [SerializeField]
    private Sprite nullSprite;

    public void UpdateTerrainInfo()
    {

        UnitSprite.sprite = Game.CurrentPawn.classSO.uiSprite;
        Unit_HP.text = Game.CurrentPawn.health.ToString();
        Unit_Mana.text = Game.CurrentPawn.mana.ToString();

        switch (Game.CurrentPawn.tileUnder.type)
        {
            case ETileType.Default:
                TerrainSprite.sprite = defaultSprite;
                break;
            case ETileType.Ground:
                TerrainSprite.sprite = groundSprite;
                break;
            case ETileType.Grass:
                TerrainSprite.sprite = grassSprite;
                break;
            case ETileType.Water:
                TerrainSprite.sprite = waterSprite;
                break;
            case ETileType.None:
                TerrainSprite.sprite = nullSprite;
                break;
            default:
                TerrainSprite.sprite = nullSprite;
                break;
        }

        Terrain_Height.text = Game.CurrentPawn.tileUnder.height.ToString();

    }






}
