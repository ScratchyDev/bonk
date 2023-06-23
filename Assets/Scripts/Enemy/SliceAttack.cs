using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceAttack : EnemyAttack
{
    [Header("References")]
    public GameObject slicePrefab;
    public GameObject player;

    public override void Attack(){
        Debug.Log("Slice Attack");
        SliceBehaviour currentSlice = Instantiate(slicePrefab, transform.position, transform.rotation).GetComponent<SliceBehaviour>();
        currentSlice.player = player;

        if(player.transform.position.x > transform.position.x){
            currentSlice.direction = -1;
        }
        else{
            currentSlice.direction = 1;
        }
    }
}
