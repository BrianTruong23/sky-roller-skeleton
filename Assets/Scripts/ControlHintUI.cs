using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlHintUI : MonoBehaviour
{
    static ControlHintUI instance;

    TextMeshProUGUI hintText;

    public static void ShowDefaultControls()
    {
        EnsureInstance();
        instance.SetText("Controls: A/D or Left/Right to move    Space to jump | Jump over spikes | Don't fall off the cliff");
    }

    static void EnsureInstance()
    {
        if (instance != null)
        {
            return;
        }

        var go = new GameObject("ControlHintUI");
        instance = go.AddComponent<ControlHintUI>();
        instance.BuildUI();
    }

    void BuildUI()
    {
        TMP_FontAsset font = TMP_Settings.defaultFontAsset;
        if (font == null)
        {
            TMP_Text existing = FindAnyObjectByType<TMP_Text>();
            if (existing != null)
            {
                font = existing.font;
            }
        }

        var canvasGo = new GameObject("ControlHintCanvas");
        canvasGo.transform.SetParent(transform);

        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasGo.AddComponent<GraphicRaycaster>();

        var panelGo = new GameObject("ControlHintPanel");
        panelGo.transform.SetParent(canvasGo.transform, false);
        var background = panelGo.AddComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.45f);

        var panelRect = background.rectTransform;
        panelRect.anchorMin = new Vector2(0f, 0f);
        panelRect.anchorMax = new Vector2(0f, 0f);
        panelRect.pivot = new Vector2(0f, 0f);
        panelRect.anchoredPosition = new Vector2(24f, 24f);
        panelRect.sizeDelta = new Vector2(1250f, 64f);

        var textGo = new GameObject("ControlHintText");
        textGo.transform.SetParent(panelGo.transform, false);

        hintText = textGo.AddComponent<TextMeshProUGUI>();
        if (font != null)
        {
            hintText.font = font;
        }

        hintText.alignment = TextAlignmentOptions.Center;
        hintText.enableAutoSizing = true;
        hintText.fontSizeMin = 16f;
        hintText.fontSizeMax = 30f;
        hintText.fontSize = 24f;
        hintText.color = Color.white;

        var textRect = hintText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(16f, 8f);
        textRect.offsetMax = new Vector2(-16f, -8f);
    }

    void SetText(string message)
    {
        hintText.text = message;
    }
}
