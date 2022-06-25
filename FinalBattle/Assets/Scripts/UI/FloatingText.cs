using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public TMP_Text text = null;

    private GameSO gameSO => GameSO.Instance;

    public void Activate(string toDisplay)
    {
        if (text == null || toDisplay == null)
            return;

        text.text = toDisplay;
        Float().Forget();
    }

    private async UniTask Float()
    {
        var cur = 0f;
        var dur = gameSO.ftDuration;

        var oldPos = transform.position;
        var oldRot = transform.rotation;

        var cam = Camera.main;

        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            cur += Time.deltaTime;
            var nor = cur / dur;

            transform.position =
                Vector3.Lerp(oldPos, oldPos + Vector3.up * gameSO.ftMoveStrength * gameSO.ftMovePattern.Evaluate(nor), nor);

            transform.LookAt(cam.transform);

            transform.rotation =
                Quaternion.Lerp(oldRot, oldRot * Quaternion.Euler(0, 0, gameSO.ftRotationStrength * gameSO.ftRotationPattern.Evaluate(nor)), nor);

            if (cur > dur) break;
        }

        Destroy(gameObject);
    }

}
