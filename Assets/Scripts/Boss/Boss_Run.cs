using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 4f;
    [Tooltip("Chase speed in units per second. Replaces the old 0.1 units per frame, so it no longer depends on frame rate (6 = the old speed at 60 fps).")]
    public float chaseSpeed = 6f;
    public float runDistance = 15f;

    Transform player;
    Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        Transform transform = animator.GetComponent<Transform>();
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if(Vector2.Distance(player.position, rb.position) >= runDistance){
            animator.SetBool("Run", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
       animator.ResetTrigger("Attack");
    }
}
