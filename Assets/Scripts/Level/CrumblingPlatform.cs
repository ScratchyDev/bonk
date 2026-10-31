using System.Collections;
using UnityEngine;

/// A platform that shakes and drops away shortly after someone steps on it, then comes back.
/// Works on any platform that has a collider - no Rigidbody2D needed.
public class CrumblingPlatform : MonoBehaviour
{
    public float shakeTime = 0.6f;
    public float shakeAmount = 0.06f;
    public float fallTime = 1.2f;
    public float respawnTime = 4f;
    [Tooltip("The Player tag is the trigger. Tick this to let enemies crumble it too.")]
    public bool triggeredByEnemies = false;

    Collider2D body;
    Vector3 home;
    bool triggered;

    void Awake(){
        body = GetComponent<Collider2D>();
        home = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(triggered) return;

        GameObject other = collision.rigidbody != null ? collision.rigidbody.gameObject : collision.gameObject;
        bool wanted = other.CompareTag("Player") || (triggeredByEnemies && other.GetComponent<Enemy>() != null);
        if(!wanted) return;

        // only when stood on from above, not when bumped from the side or below
        if(other.transform.position.y <= body.bounds.max.y) return;

        StartCoroutine(Crumble());
    }

    IEnumerator Crumble(){
        triggered = true;

        float t = 0f;
        while(t < shakeTime){
            t += Time.deltaTime;
            transform.position = home + new Vector3(Random.Range(-1f, 1f) * shakeAmount, 0f, 0f);
            yield return null;
        }

        transform.position = home;
        body.enabled = false;

        float velocity = 0f;
        t = 0f;
        while(t < fallTime){
            t += Time.deltaTime;
            velocity += 25f * Time.deltaTime;
            transform.position += Vector3.down * (velocity * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(Mathf.Max(0f, respawnTime - fallTime));

        transform.position = home;
        body.enabled = true;
        triggered = false;
    }
}
