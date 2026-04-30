using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    public static OverlayController Instance { get; private set; }
    [SerializeField] private Image image = null;
    [SerializeField] private float blinkDuration = 0.5f;
    private IEnumerator blinkTask = null;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetImageAlpha(0f);
        image.raycastTarget = false;
    }
    public void StartBlink(System.Action middleAction, System.Action postAction = null)
    {
        if (blinkTask != null)
        {
            Debug.LogWarning("Overriding blink task!");
            StopCoroutine(blinkTask);
        }
        blinkTask = BlinkTask(middleAction, postAction);
        StartCoroutine(blinkTask);
    }
    IEnumerator BlinkTask(System.Action middleAction, System.Action postAction)
    {
        image.raycastTarget = true;

        yield return FadeUtility.FadeTask(1f, blinkDuration, GetImageAlpha, SetImageAlpha);

        middleAction?.Invoke();

        yield return FadeUtility.FadeTask(0f, blinkDuration, GetImageAlpha, SetImageAlpha);

        postAction?.Invoke();

        image.raycastTarget = false;

        blinkTask = null;
    }
    private float GetImageAlpha()
    {
        return image.color.a;
    }
    private void SetImageAlpha(float value)
    {
        Color color = image.color;
        color.a = value;
        image.color = color;
    }
    public void LoadSceneWithFade(int sceneIndex)
    {
        StartBlink(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        });
    }
}
