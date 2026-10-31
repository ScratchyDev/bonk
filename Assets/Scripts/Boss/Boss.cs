using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Variables")]
    public float attackPower;
    public int maxEnergy;
    public float mass;
    public float stunTime;
    public int maxHealth = 3;

    [Header("References")]
    public Animator healthAnim;
    public Transform spawnPoint;
    public Slider energySlider;

    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public int health;

    GameObject player;
    Rigidbody2D playerRb;
    Rigidbody2D rb;
    Animator bossAnim;
    
    int energy;
    bool stun;

    void Start(){
        health = maxHealth;
        
        healthAnim.SetInteger("health", health);
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();

        rb.mass = mass;
        energy = maxEnergy;
        energySlider.maxValue = maxEnergy;
    }

    void Update(){
        energySlider.value = energy;

        // `!dead` stops the health from counting down further every frame once the boss is already beaten
        if(!dead && transform.position.y <= -10){
            health--;
            if(health > 0){
                if(health == 1){
                    bossAnim.SetBool("Rage", true);
                    maxEnergy = maxEnergy * 2;
                    energySlider.maxValue = maxEnergy;
                }

                transform.position = spawnPoint.position;
                energy = maxEnergy;
            }
            else{dead = true;}
        }

        healthAnim.SetInteger("health", health);
    }

    public void Stun(){
        Hit(1);
    }

    /// Same as Stun() but lets a source take more than one point of energy.
    /// (Named differently on purpose: Stun() may be used as an animation event, which dislikes overloads.)
    public void Hit(int amount){
        if(dead) return;

        energy -= amount;
        GameFeel.PopText(transform.position + Vector3.up * 2.5f, "-" + amount, Color.white, 4f);

        if(energy <= 0 && !stun){
            stun = true;
            GameFeel.PopText(transform.position + Vector3.up * 3.2f, "STUNNED!", new Color(1f, 0.85f, 0.2f), 6f);
            GameFeel.HitStop(0.1f);
            StartCoroutine(Stun2());
        }
    }

    IEnumerator Stun2(){
        stun = true;
        rb.mass = mass / 10f;
        bossAnim.SetTrigger("Stun");
        
        yield return new WaitForSeconds(stunTime);
        
        rb.mass = mass;
        energy = maxEnergy;
        stun = false;
    }

    // The attacks used to push the player along world +X no matter where the boss stood.
    // Now the player is always pushed away from the boss.
    float DirectionAwayFromBoss(){
        return player.transform.position.x < transform.position.x ? -1f : 1f;
    }

    public void Attack(){
        playerRb.AddForce(Vector2.right * DirectionAwayFromBoss() * attackPower);
    }

    public void EnragedAttack(){
        playerRb.AddForce(Vector2.right * DirectionAwayFromBoss() * (attackPower * 2.5f));
        rb.AddForce(transform.right * (attackPower));
    }
}
