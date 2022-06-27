using System.Collections.Generic;
using UnityEngine;

public class InitiativeActionBar : MonoBehaviour
{
    public GameObject enemyInfoPrefab;
    List<GameObject> EnemyInfoList;

    // Yah: I'm aware this is a crime against all of what its sacred but this is a jam so frick it.
    public void UpdateInitiativeUI()
    {
        var children = transform.childCount;

        for (int i = 0; i < children; i++)
            Destroy(transform.GetChild(i).gameObject);

        if (!enemyInfoPrefab.GetComponent<EnemyInfo>())
            return;

        // Spawn UI portraits
        for (int i = Game.InitiativeTracker; i < Game.Initiative.Count; i++) // First part of the list
            InstantiatePortrait(i);

        if (Game.InitiativeTracker != 0) // Second part of the list, if it should
            for (int i = 0; i < Game.InitiativeTracker; i++)
                InstantiatePortrait(i);
    }

    private void InstantiatePortrait(int i)
    {
        var unitUI = Instantiate(enemyInfoPrefab);
        unitUI.GetComponent<EnemyInfo>().SetEnemyInfo(Game.Initiative[i]);
        unitUI.transform.SetParent(this.transform);
    }

}
