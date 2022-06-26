using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeActionBar : MonoBehaviour
{
    public GameObject enemyInfoPrefab;
    List<GameObject> EnemyInfoList;

    // Yah: I'm aware this is a crime against all of what its sacred but this is a jam so frick it.
    public void UpdateInitiativeUI() {

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (enemyInfoPrefab.GetComponent<EnemyInfo>())
        {
            for (int i = Game.InitiativeTracker; i < Game.Initiative.Count; i++)
            {
                GameObject unitUI = Instantiate(enemyInfoPrefab);
                unitUI.GetComponent<EnemyInfo>().SetEnemyInfo(Game.Initiative[i]);
                unitUI.transform.SetParent(this.transform);
            }

            if (Game.InitiativeTracker != 0) {

                for (int i = 0; i < Game.InitiativeTracker; i++)
                {
                    GameObject unitUI = Instantiate(enemyInfoPrefab);
                    unitUI.GetComponent<EnemyInfo>().SetEnemyInfo(Game.Initiative[i]);
                    unitUI.transform.SetParent(this.transform);
                }

            }

        }
    }


}
