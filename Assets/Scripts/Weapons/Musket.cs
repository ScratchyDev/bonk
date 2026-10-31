using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : BaseWeapon
{
    [Header("Musket Variables")]
    public float bulletForce;
    public float scopeMultiplier;
    public bool dealsDamage;

    [Header("Musket References")]
    public GameObject musketBall;
    public Transform shootingPoint;
    public Movement playerMovement;
    public ScopeIn scope;

    private float currentScopeMultiplier;

    public override void WeaponFunction(){
        // Reset every shot. Before, a grounded scoped shot re-used the multiplier of an earlier airborne shot.
        currentScopeMultiplier = 1f;

        if(scope != null && scope.scoped){
            scope.ScopeExit();

            if(!playerMovement.isGrounded){
                currentScopeMultiplier = scopeMultiplier;
            }
        }

        Rigidbody2D rb = Instantiate(musketBall, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletForce * currentScopeMultiplier);

        MusketBall ball = rb.gameObject.GetComponent<MusketBall>();
        if(ball != null && dealsDamage){
            rb.gameObject.GetComponent<Collider2D>().isTrigger = true;
            ball.damage = EffectiveDamage;
        }
    }
}
