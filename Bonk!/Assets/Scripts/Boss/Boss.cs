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
    Rigidbody2D rb;
    Animator bossAnim;
    
    int energy;
    bool stun;

    void Start(){
        health = maxHealth;
        
        healthAnim.SetInteger("health", health);
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();

        rb.mass = mass;
        energy = maxEnergy;
        energySlider.maxValue = maxEnergy;
    }

    void Update(){
        energySlider.value = energy;

        if(transform.position.y <= -10){
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
        energy--;
        if(energy <= 0 && !stun){
            stun = true;
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

    public void Attack(){
        player.GetComponent<Rigidbody2D>().AddForce(player.transform.right * attackPower);
    }

    public void EnragedAttack(){
        player.GetComponent<Rigidbody2D>().AddForce(player.transform.right * (attackPower * 2.5f));
        rb.AddForce(transform.right * (attackPower));
    }
}
