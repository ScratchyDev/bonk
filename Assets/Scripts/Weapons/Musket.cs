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
        
        if(scope != null){
            if(scope.scoped){
                scope.ScopeExit();

                if(!playerMovement.isGrounded){
                    currentScopeMultiplier = scopeMultiplier;
                    Debug.Log("epik gamr");
                }
            } else {
                currentScopeMultiplier = 1f;
            }
        } else {
            currentScopeMultiplier = 1f;
        }

        Rigidbody2D rb = Instantiate(musketBall, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletForce * currentScopeMultiplier);
        if(rb.gameObject.GetComponent<MusketBall>() != null && dealsDamage){
            rb.gameObject.GetComponent<Collider2D>().isTrigger = true;
            rb.gameObject.GetComponent<MusketBall>().damage = damage;
        }
    }
}
