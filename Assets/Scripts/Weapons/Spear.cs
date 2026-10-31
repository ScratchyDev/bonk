using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : BaseWeapon
{
    private Rigidbody2D rb;
    private Transform normalParent;
    private BoxCollider2D boxCollider;
    private bool thrown;
    private float thrownAt;

    [Header("Spear References")]
    public Movement playerMovement;

    [Header("Spear Tuning")]
    [Tooltip("Throw force (x is mirrored depending on the way the player faces). Used to be hard-coded.")]
    public Vector2 throwForce = new Vector2(10000f, 2000f);
    [Tooltip("A thrown spear that falls below this height comes back to the player instead of being lost forever.")]
    public float recallBelowY = -30f;
    [Tooltip("Automatically recall the spear this many seconds after the throw. 0 = never.")]
    public float autoRecallDelay = 0f;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        normalParent = base.transform.parent;
        thrown = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.enabled = false;
    }

    public override void WeaponFunction(){
        if (!thrown)
		{
			thrown = true;
            thrownAt = Time.time;
            boxCollider.enabled = true;
			base.transform.SetParent(null);
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.mass = 20f;

            // Movement.direction is 0 until the player has moved once, which used to throw the spear straight up.
            float dir = playerMovement.direction;
            if(dir == 0f){
                dir = playerMovement.transform.localScale.x < 0f ? -1f : 1f;
            }
			rb.AddForce(new Vector2(throwForce.x * dir, throwForce.y));
		}
    }

    void FixedUpdate(){
        if(!thrown){
            return;
        }

        if(transform.position.y < recallBelowY || (autoRecallDelay > 0f && Time.time - thrownAt > autoRecallDelay)){
            Recall();
        }
    }

    public void Recall(){
		base.transform.SetParent(normalParent);
		base.transform.position = base.transform.parent.position;
        base.transform.localScale = new Vector3(1, 1, 1);
		rb.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.enabled = true;
		thrown = false;
	}
}
