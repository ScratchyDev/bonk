using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// A small time / best time / wave read-out at the top of the screen.
/// GameManager creates it automatically (untick "Show Hud" there to switch it off),
/// it builds its own canvas so no prefab or scene editing is needed.
public class LevelHud : MonoBehaviour
{
    public GameManager manager;

    TextMeshProUGUI label;
    float nextRefresh;

    void Awake(){
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if(canvas == null){
            canvas = gameObject.AddComponent<Canvas>();
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 20;

        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject textObject = new GameObject("Label", typeof(RectTransform));
        textObject.transform.SetParent(transform, false);

        label = textObject.AddComponent<TextMeshProUGUI>();
        label.alignment = TextAlignmentOptions.Top;
        label.fontSize = 30f;
        label.fontStyle = FontStyles.Bold;
        label.color = new Color(1f, 1f, 1f, 0.85f);
        label.raycastTarget = false;

        RectTransform rect = label.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -12f);
        rect.sizeDelta = new Vector2(1000f, 50f);
    }

    void Update(){
        if(manager == null){
            Destroy(gameObject);
            return;
        }

        if(Time.unscaledTime < nextRefresh) return;
        nextRefresh = Time.unscaledTime + 0.1f;

        string text = FormatTime(manager.LevelTime);

        float best = manager.BestTime;
        if(best > 0f){
            text += "    Best " + FormatTime(best);
        }

        WaveSpawner waves = manager.waveSpawner;
        if(waves != null && waves.CurrentWave > 0){
            text += "    Wave " + waves.CurrentWave;
            if(waves.wavesToWin > 0){
                text += "/" + waves.wavesToWin;
            }
        }

        label.text = text;
    }

    static string FormatTime(float seconds){
        int minutes = (int)(seconds / 60f);
        float rest = seconds - minutes * 60f;
        return minutes.ToString("00", CultureInfo.InvariantCulture) + ":" + rest.ToString("00.0", CultureInfo.InvariantCulture);
    }
}
