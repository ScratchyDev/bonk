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
    public Color attackWarningColour;

    [Header("Telegraph")]
    [Tooltip("The enemy flashes the warning colour this long before it attacks, so the player can react. 0 = attack instantly like before.")]
    public float attackWindup = 0.3f;

    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool stun;
    [HideInInspector]
    public int energy;
    [HideInInspector]
    public Rigidbody2D rb;

    float currentAttackTime;
    bool winding;

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

        // This check used to live inside the !stun block, so an enemy that was stunned
        // (and therefore easy to knock off) was not counted as defeated until the stun wore off.
        if(transform.position.y <= -50){
            dead = true;
        }

        if(!stun){
            transform.position = Vector2.MoveTowards(transform.position, player.position, step);

            if(player.position.x > transform.position.x){
                transform.localScale = new Vector3(1, 1, 1);
            } 
            else if(player.position.x < transform.position.x){
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if(!winding && Vector2.Distance(transform.position, player.position) < attackRange && currentAttackTime <= 0){
                currentAttackTime = attackTime;
                StartCoroutine(WindupAndAttack());
            }
        }
    }

    IEnumerator WindupAndAttack(){
        if(attackWindup <= 0f){
            enemyAttack.Attack();
            yield break;
        }

        winding = true;
        SetColour(attackWarningColour.a > 0.01f ? attackWarningColour : Color.red);

        float t = 0f;
        while(t < attackWindup){
            if(stun){
                // a stun colour is applied by Stun2, so just abort
                winding = false;
                yield break;
            }
            t += Time.deltaTime;
            yield return null;
        }

        SetColour(defaultColour);
        enemyAttack.Attack();
        winding = false;
    }

    public void Stun(int amount){
        energy -= amount;
        GameFeel.PopText(transform.position + Vector3.up * 1.5f, "-" + amount, Color.white, 4f);

        if(energy <= 0 && !stun){
            GameFeel.PopText(transform.position + Vector3.up * 2.2f, "STUNNED!", new Color(1f, 0.85f, 0.2f), 6f);
            GameFeel.HitStop(0.08f);
            StartCoroutine(Stun2());
        }
    }

    IEnumerator Stun2(){
        stun = true;
        rb.mass = mass / 10f;
        SetColour(stunColour);
        
        yield return new WaitForSeconds(stunTime);

        rb.mass = mass;
        energy = maxEnergy;
        stun = false;
        SetColour(defaultColour);
    }

    void SetColour(Color colour){
        foreach (SpriteRenderer sprite in spriteRenderers){
            sprite.color = colour;
        }
    }
}
