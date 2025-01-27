using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class FadeUtility
{
    public static IEnumerator FadeTask(float target, float duration, System.Func<float> get, System.Action<float> set, System.Action clear = null)
    {
        float progress = target < get() ? (1f - get()) : get();

        while (progress < duration)
        {
            set(Mathf.Lerp(1f - target, target, progress / duration));
            progress += Time.unscaledDeltaTime;
            yield return 0;
        }
        set(target);
        clear?.Invoke();
    }
    
}
