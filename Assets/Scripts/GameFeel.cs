using System.Collections;
using TMPro;
using UnityEngine;

/// Tiny "juice" toolbox: hit-stop, floating text and an expanding ring.
/// Everything is created on demand - nothing has to be wired up in the editor.
/// Set GameFeel.juice = false (for example from a settings toggle) to switch it all off.
public static class GameFeel
{
    public static bool juice = true;

    const float StopScale = 0.02f;
    static GameFeelRunner runner;

    static GameFeelRunner GetRunner()
    {
        if (runner == null)
        {
            GameObject go = new GameObject("[GameFeel]");
            go.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(go);
            runner = go.AddComponent<GameFeelRunner>();
        }
        return runner;
    }

    /// Runs a coroutine that must survive the object that started it (or a scene change).
    public static void Run(IEnumerator routine)
    {
        GetRunner().StartCoroutine(routine);
    }

    /// Freezes the game for a split second on a big hit. Does nothing while paused or in slow-mo.
    public static void HitStop(float duration)
    {
        if (!juice || duration <= 0f) return;
        GameFeelRunner r = GetRunner();
        if (r.stopping || Time.timeScale < 0.99f) return;
        r.StartCoroutine(HitStopRoutine(r, duration));
    }

    static IEnumerator HitStopRoutine(GameFeelRunner r, float duration)
    {
        r.stopping = true;
        Time.timeScale = StopScale;
        yield return new WaitForSecondsRealtime(duration);
        // only restore if nobody else (pause menu, scope) has changed the time scale meanwhile
        if (Mathf.Approximately(Time.timeScale, StopScale)) Time.timeScale = 1f;
        r.stopping = false;
    }

    /// Text that floats upwards and fades out.
    public static void PopText(Vector3 position, string text, Color colour, float size = 5f)
    {
        if (!juice) return;
        GetRunner().StartCoroutine(PopRoutine(position, text, colour, size));
    }

    static IEnumerator PopRoutine(Vector3 position, string text, Color colour, float size)
    {
        GameObject go = new GameObject("PopText");
        position.x += Random.Range(-0.3f, 0.3f);
        go.transform.position = position;

        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = colour;
        tmp.sortingOrder = 100;

        float t = 0f;
        const float life = 0.9f;
        while (t < life)
        {
            if (go == null) yield break; // scene was unloaded
            t += Time.unscaledDeltaTime;
            float k = t / life;
            go.transform.position = position + Vector3.up * (k * 1.5f);
            Color c = colour;
            c.a = 1f - k * k;
            tmp.color = c;
            yield return null;
        }
        if (go != null) Object.Destroy(go);
    }

    /// A quick expanding ring, handy for shockwaves. Needs no assets.
    public static void Ring(Vector3 centre, float radius, Color colour, float duration = 0.3f)
    {
        if (!juice) return;
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null) return;
        GetRunner().StartCoroutine(RingRoutine(centre, radius, colour, duration, shader));
    }

    static IEnumerator RingRoutine(Vector3 centre, float radius, Color colour, float duration, Shader shader)
    {
        const int segments = 40;
        GameObject go = new GameObject("Ring");
        go.transform.position = centre;
        LineRenderer line = go.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = segments;
        line.material = new Material(shader);
        line.sortingOrder = 90;

        float t = 0f;
        while (t < duration)
        {
            if (go == null) yield break;
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            float r = Mathf.Lerp(0.2f, radius, 1f - (1f - k) * (1f - k));
            for (int i = 0; i < segments; i++)
            {
                float a = (i / (float)segments) * Mathf.PI * 2f;
                line.SetPosition(i, new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0f));
            }
            Color c = colour;
            c.a = 1f - k;
            line.startColor = c;
            line.endColor = c;
            float w = Mathf.Lerp(0.25f, 0.03f, k);
            line.startWidth = w;
            line.endWidth = w;
            yield return null;
        }
        if (go != null)
        {
            Object.Destroy(line.material);
            Object.Destroy(go);
        }
    }
}

/// Hidden helper object that GameFeel uses to run coroutines.
internal class GameFeelRunner : MonoBehaviour
{
    public bool stopping;
}
