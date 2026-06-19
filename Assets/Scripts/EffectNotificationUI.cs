using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectNotificationUI : MonoBehaviour
{
    static EffectNotificationUI instance;

    [SerializeField] float displayDuration = 3f;
    [SerializeField] float duplicateCooldown = 2f;

    TextMeshProUGUI messageText;
    GameObject panel;
    Coroutine hideRoutine;
    readonly Dictionary<string, float> lastShownAt = new Dictionary<string, float>();

    public static void Show(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        EnsureInstance();
        instance.Display(message);
    }

    static void EnsureInstance()
    {
        if (instance != null)
        {
            return;
        }

        var go = new GameObject("EffectNotificationUI");
        instance = go.AddComponent<EffectNotificationUI>();
        instance.BuildUI();
    }

    void BuildUI()
    {
        // Grab a font that is guaranteed to exist in this project before we
        // create our own text (otherwise the runtime text can render blank).
        TMP_FontAsset font = TMP_Settings.defaultFontAsset;
        if (font == null)
        {
            TMP_Text existing = FindAnyObjectByType<TMP_Text>();
            if (existing != null)
            {
                font = existing.font;
            }
        }

        var canvasGo = new GameObject("NotificationCanvas");
        canvasGo.transform.SetParent(transform);

        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32000;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasGo.AddComponent<GraphicRaycaster>();

        var bgGo = new GameObject("Background");
        panel = bgGo;
        bgGo.transform.SetParent(canvasGo.transform, false);
        var bg = bgGo.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.6f);
        var bgRect = bg.rectTransform;
        bgRect.anchorMin = new Vector2(0.5f, 1f);
        bgRect.anchorMax = new Vector2(0.5f, 1f);
        bgRect.pivot = new Vector2(0.5f, 1f);
        bgRect.anchoredPosition = new Vector2(0f, -70f);
        bgRect.sizeDelta = new Vector2(1000f, 90f);

        var textGo = new GameObject("Message");
        textGo.transform.SetParent(bgGo.transform, false);

        messageText = textGo.AddComponent<TextMeshProUGUI>();
        if (font != null)
        {
            messageText.font = font;
        }

        messageText.alignment = TextAlignmentOptions.Center;
        messageText.enableAutoSizing = true;
        messageText.fontSizeMin = 18;
        messageText.fontSizeMax = 44;
        messageText.fontSize = 36;
        messageText.color = Color.white;

        var rect = messageText.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(20f, 10f);
        rect.offsetMax = new Vector2(-20f, -10f);

        bgGo.SetActive(false);
    }

    void Display(string message)
    {
        if (lastShownAt.TryGetValue(message, out float lastTime)
            && Time.unscaledTime - lastTime < duplicateCooldown)
        {
            return;
        }

        lastShownAt[message] = Time.unscaledTime;

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        messageText.text = message;
        panel.SetActive(true);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displayDuration);
        panel.SetActive(false);
        hideRoutine = null;
    }
}
