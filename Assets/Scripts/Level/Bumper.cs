using System.Collections;
using UnityEngine;

/// Pinball bumper: launches whatever touches it away from its centre.
/// Put it on an object with a normal (non-trigger) collider.
public class Bumper : MonoBehaviour
{
    [Tooltip("Roughly the speed (units per second) the target is launched at.")]
    public float bounceSpeed = 14f;
    [Tooltip("Energy drained from the player on contact.")]
    public float energyDamage = 0f;
    public bool affectEnemies = true;

    Vector3 baseScale;

    void Awake(){
        baseScale = transform.localScale;
    }

    void OnCollisionEnter2D(Collision2D collision){
        Rigidbody2D other = collision.rigidbody;
        if(other == null) return;
        if(!affectEnemies && other.GetComponent<Enemy>() != null) return;

        Vector2 direction = other.position - (Vector2)transform.position;
        direction.y += 0.4f; // a little lift so it feels like a bounce
        BonkUtil.Knock(other.transform, direction, bounceSpeed, energyDamage);

        StopAllCoroutines();
        StartCoroutine(Pop());
    }

    IEnumerator Pop(){
        float t = 0f;
        const float duration = 0.18f;
        while(t < duration){
            t += Time.deltaTime;
            float k = Mathf.Sin(Mathf.Clamp01(t / duration) * Mathf.PI);
            transform.localScale = baseScale * (1f + 0.25f * k);
            yield return null;
        }
        transform.localScale = baseScale;
    }
}
