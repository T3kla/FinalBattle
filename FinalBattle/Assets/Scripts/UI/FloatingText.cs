using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public CanvasGroup group = null;
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

            if (ct.IsCancellationRequested) return;

            cur += Time.deltaTime;
            var nor = cur / dur;

            group.alpha = gameSO.ftAlphaPattern.Evaluate(nor);
            transform.position = oldPos + Vector3.up * gameSO.ftMoveStrength * gameSO.ftMovePattern.Evaluate(nor);

            transform.LookAt(-cam.transform.forward + transform.position);

            if (cur > dur) break;
        }

        Destroy(gameObject);
    }

}
