using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : BaseWeapon
{
    private Rigidbody2D rb;
    private Transform normalParent;
    private BoxCollider2D boxCollider;
    private bool thrown;

    [Header("Spear References")]
    public Movement playerMovement;

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
            boxCollider.enabled = true;
			base.transform.SetParent(null);
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.mass = 20f;
			rb.AddForce(new Vector2(10000f * playerMovement.direction, 2000f));
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
