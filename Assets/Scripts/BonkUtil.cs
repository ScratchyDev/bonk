using UnityEngine;

/// Small shared helpers used by the newer scripts.
public static class BonkUtil
{
    public static T FindFirst<T>() where T : Object
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindFirstObjectByType<T>();
#else
        return Object.FindObjectOfType<T>();
#endif
    }

    public static T[] FindAll<T>() where T : Object
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
        return Object.FindObjectsOfType<T>();
#endif
    }

    /// Launches a body so its velocity changes by roughly `speed` (independent of its mass).
    /// If the target has a Movement component the horizontal speed cap is lifted for a moment
    /// so the launch is actually visible, and an optional amount of energy is drained.
    public static bool Knock(Transform target, Vector2 direction, float speed, float energyDamage)
    {
        if (target == null) return false;
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return false;

        Movement movement = target.GetComponent<Movement>();
        if (movement != null)
        {
            movement.SuspendSpeedCap(0.35f);
            if (energyDamage > 0f && movement.energyManager != null)
            {
                movement.energyManager.TakeEnergy(energyDamage);
            }
        }

        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.up;
        rb.AddForce(direction.normalized * speed * rb.mass, ForceMode2D.Impulse);
        return true;
    }
}
