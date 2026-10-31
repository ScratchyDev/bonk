using System.Collections.Generic;
using UnityEngine;

/// A platform that glides back and forth. Add it to a platform that has a collider; the
/// Rigidbody2D is added for you and made kinematic. Anything standing on top is carried along.
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("How far it travels from where you placed it (world units).")]
    public Vector2 travel = new Vector2(6f, 0f);
    public float speed = 2f;
    public float pauseAtEnds = 0.5f;

    Rigidbody2D rb;
    Collider2D body;
    Vector2 start;
    Vector2 end;
    bool towardsEnd = true;
    float waitUntil;
    readonly HashSet<Rigidbody2D> riders = new HashSet<Rigidbody2D>();

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        body = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        start = rb.position;
        end = start + travel;
    }

    void FixedUpdate(){
        if(Time.time < waitUntil) return;

        Vector2 target = towardsEnd ? end : start;
        Vector2 next = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        Vector2 delta = next - rb.position;
        rb.MovePosition(next);

        foreach(Rigidbody2D rider in riders){
            if(rider != null) rider.position += delta;
        }

        if((next - target).sqrMagnitude < 0.0001f){
            towardsEnd = !towardsEnd;
            waitUntil = Time.time + pauseAtEnds;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){ Track(collision); }
    void OnCollisionStay2D(Collision2D collision){ Track(collision); }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.rigidbody != null) riders.Remove(collision.rigidbody);
    }

    // something counts as "standing on it" when its centre is above the platform's top
    void Track(Collision2D collision){
        Rigidbody2D other = collision.rigidbody;
        if(other == null || other.bodyType != RigidbodyType2D.Dynamic) return;

        if(other.position.y > body.bounds.max.y) riders.Add(other);
        else riders.Remove(other);
    }

    void OnDrawGizmosSelected(){
        Vector3 from = Application.isPlaying ? (Vector3)start : transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(from, from + (Vector3)travel);
    }
}
