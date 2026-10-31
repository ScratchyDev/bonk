using UnityEngine;

/// An enemy attack that needs no prefab: a ground slam that launches the player away if they are close.
/// Put it on the enemy and drag it into the Enemy's "Enemy Attack" slot (replacing the slice attack).
/// The Enemy's attack windup flashes its warning colour first, so the player can jump or dash away.
public class ShockwaveAttack : EnemyAttack
{
    public float radius = 3.5f;
    [Tooltip("Roughly the speed (units per second) the player is launched at.")]
    public float knockSpeed = 12f;
    public float energyDamage = 50f;
    public Transform player;

    public override void Attack(){
        if(player == null){
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if(found != null) player = found.transform;
        }

        GameFeel.Ring(transform.position, radius, new Color(1f, 0.6f, 0.2f));
        GameFeel.PopText(transform.position + Vector3.up * 2f, "SLAM!", new Color(1f, 0.6f, 0.2f), 6f);

        if(player == null) return;

        Vector2 offset = player.position - transform.position;
        if(offset.magnitude > radius) return;

        Vector2 direction = new Vector2(offset.x < 0f ? -1f : 1f, 0.4f);
        BonkUtil.Knock(player, direction, knockSpeed, energyDamage);
        GameFeel.HitStop(0.06f);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(1f, 0.6f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
