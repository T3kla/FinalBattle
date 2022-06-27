using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public TMP_Text text = null;

    private GameSO gameSO => GameSO.Instance;

    private CancellationTokenSource cts = new CancellationTokenSource();

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    public void Activate(string toDisplay)
    {
        if (text == null || toDisplay == null)
            return;

        text.text = toDisplay;
        Float(cts.Token).Forget();
    }

    private async UniTask Float(CancellationToken ct)
    {
        var cur = 0f;
        var dur = gameSO.ftDuration;

        var oldPos = transform.position;
        var oldRot = transform.rotation;

        var cam = Camera.main;

        while (true)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            if (ct.IsCancellationRequested) break;

            cur += Time.deltaTime;
            var nor = cur / dur;

            transform.position =
                Vector3.Lerp(oldPos, oldPos + Vector3.up * gameSO.ftMoveStrength * gameSO.ftMovePattern.Evaluate(nor), nor);

            transform.LookAt(cam.transform, cam.transform.up);

            if (cur > dur) break;
        }

        Destroy(gameObject);
    }

}
