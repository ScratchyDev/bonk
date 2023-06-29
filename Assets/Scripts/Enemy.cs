using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float stunTime;
    public float mass;
    public int maxEnergy;
    public float attackTime;
    public float attackRange;

    [Header("References")]
    public Transform player;
    public Slider energySlider;
    public SpriteRenderer[] spriteRenderers;
    public EnemyAttack enemyAttack;

    [Header("Other")]
    public Color defaultColour;
    public Color stunColour;

    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool stun;
    [HideInInspector]
    public int energy;
    [HideInInspector]
    public Rigidbody2D rb;

    float currentAttackTime;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        energySlider.maxValue = maxEnergy;
        energy = maxEnergy;
    }

    void Update(){
        float step = speed * Time.deltaTime;
        energySlider.value = energy;
        currentAttackTime -= Time.deltaTime;

        if(!stun){
            transform.position = Vector2.MoveTowards(transform.position, player.position, step);

            if(player.position.x > transform.position.x){
                transform.localScale = new Vector3(1, 1, 1);
            } 
            else if(player.position.x < transform.position.x){
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if(transform.position.y <= -50){
                dead = true;
            }

            if(Vector2.Distance(transform.position, player.position) < attackRange && currentAttackTime <= 0){
                currentAttackTime = attackTime;
                enemyAttack.Attack();
            }
        }
    }

    public void Stun(int amount){
        energy -= amount;
        if(energy <= 0 && !stun){
            StartCoroutine(Stun2());
        }
    }

    IEnumerator Stun2(){
        stun = true;
        rb.mass = mass / 10f;

        foreach (SpriteRenderer sprite in spriteRenderers){
            sprite.color = stunColour;
        }
        
        yield return new WaitForSeconds(stunTime);

        rb.mass = mass;
        energy = maxEnergy;
        stun = false;

        foreach (SpriteRenderer sprite in spriteRenderers){
            sprite.color = defaultColour;
        }
    }
}
