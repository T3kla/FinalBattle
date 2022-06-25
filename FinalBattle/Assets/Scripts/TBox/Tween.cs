using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TBox
{

    public class Tween
    {

        public static async UniTask Scale(CanvasGroup group, float i, float f, float duration = 1.0f, CancellationToken ct = default)
        {
            if (duration <= 0.0f) goto end;

            group.gameObject.SetActive(true);

            float counter = 0f, completion = 0f, dif = f - i;

            while (counter < duration)
            {
                await UniTask.NextFrame(ct);
                if (ct.IsCancellationRequested) goto end;

                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                group.transform.localScale = Vector2.one * (completion * dif + i);
            }

        end:

            group.transform.localScale = new Vector2(f, f);
            group.transform.gameObject.SetActive(group.transform.localScale != Vector3.zero);
        }

        public static async UniTask Fade(CanvasGroup group, float i, float f, float duration = 1.0f, CancellationToken ct = default)
        {
            if (duration <= 0.0f) goto end;

            group.gameObject.SetActive(true);

            float counter = 0f, completion = 0f, dif = f - i;

            while (counter < duration)
            {
                await UniTask.NextFrame(ct);
                if (ct.IsCancellationRequested) goto end;

                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                group.alpha = completion * dif + i;
            }

        end:

            group.alpha = f;
            group.transform.gameObject.SetActive(group.alpha != 0.0f);
        }

    }

}
