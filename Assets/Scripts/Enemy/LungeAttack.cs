using System.Collections;
using UnityEngine;

/// An enemy attack that needs no prefab: the enemy throws itself at the player.
/// Put it on the enemy and drag it into the Enemy's "Enemy Attack" slot.
public class LungeAttack : EnemyAttack
{
    public float lungeSpeed = 14f;
    public float lungeTime = 0.35f;
    [Tooltip("The lunge connects when the enemy ends up this close to the player.")]
    public float hitRadius = 1.6f;
    public float knockSpeed = 10f;
    public float energyDamage = 25f;
    public Transform player;

    bool lunging;

    public override void Attack(){
        if(lunging) return;

        if(player == null){
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if(found != null) player = found.transform;
        }

        Enemy enemy = GetComponentInParent<Enemy>();
        if(player == null || enemy == null) return;

        StartCoroutine(Lunge(enemy));
    }

    IEnumerator Lunge(Enemy enemy){
        lunging = true;

        Transform body = enemy.transform;
        Vector2 direction = ((Vector2)player.position - (Vector2)body.position).normalized;

        float t = 0f;
        while(t < lungeTime){
            if(enemy.stun) break; // a stunned enemy stops in its tracks
            t += Time.deltaTime;
            body.position += (Vector3)(direction * lungeSpeed * Time.deltaTime);
            yield return null;
        }

        if(!enemy.stun && Vector2.Distance(body.position, player.position) <= hitRadius){
            BonkUtil.Knock(player, direction, knockSpeed, energyDamage);
            GameFeel.PopText(player.position + Vector3.up * 1.5f, "BONK!", new Color(1f, 0.4f, 0.3f), 6f);
            GameFeel.HitStop(0.06f);
        }

        lunging = false;
    }
}
