using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Variables")]
    public float speed;
    public float jumpForce;
    public float dashForce;
    public int maxJumps;

    [Header("Tuning")]
    [Tooltip("Horizontal speed cap (this used to be hard-coded to 5).")]
    public float maxSpeed = 5f;
    [Tooltip("Multiplies run force and speed cap. Power-ups change this.")]
    public float speedMultiplier = 1f;
    [Tooltip("Pressing jump this long before landing still jumps on touchdown.")]
    public float jumpBufferTime = 0.12f;
    [Tooltip("Minimum time between two dashes.")]
    public float dashCooldown = 0.15f;

    [Header("References")]
    public EnergyManager energyManager;
    public Transform groundCheck;
    public LayerMask ground;
    //[HideInInspector] 
    public bool isGrounded;

    [HideInInspector]
    public int direction;
    Rigidbody2D rb;
    public int currentJumps;

    float lastJumpTime = -10f;
    float jumpPressedTime = -10f;
    float nextDashTime;
    float speedCapSuspendedUntil;

    void Start(){
        rb = GetComponent<Rigidbody2D>();

        // direction is 0 until the player first moves, which broke things that read it (the spear throw).
        if(direction == 0){
            direction = transform.localScale.x < 0f ? -1 : 1;
        }
    }

    void FixedUpdate(){
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 1f), new Vector2(transform.position.x + 0.5f, transform.position.y - 1.1f), ground);
        
        // the short delay stops the ground check from refilling the jumps in the frame right after take-off
        if(isGrounded && Time.time - lastJumpTime > 0.1f){
            currentJumps = 1;

            if(Time.time - jumpPressedTime <= jumpBufferTime){
                jumpPressedTime = -10f;
                TryJump();
            }
        }

        if(Time.time >= speedCapSuspendedUntil){
            float cap = maxSpeed * speedMultiplier;

            if(rb.velocity.x > cap){
                rb.velocity = new Vector2(cap, rb.velocity.y);
            }

            if(rb.velocity.x < -cap){
                rb.velocity = new Vector2(-cap, rb.velocity.y);
            }
        }
    }

    public void Move(float amount){
        rb.AddForce(transform.right * amount * speed * speedMultiplier);
        if(amount > 0){
            transform.localScale = new Vector3(1, 1, 1);
            direction = 1;
        }
        else if(amount < 0){
            transform.localScale = new Vector3(-1, 1, 1);
            direction = -1;
        }
    }

    public void Jump(){
        if(!TryJump()){
            // remember the press so landing a moment later still jumps
            jumpPressedTime = Time.time;
        }
    }

    bool TryJump(){
        if(currentJumps < maxJumps && energyManager.energy > 25){
            currentJumps++;
            lastJumpTime = Time.time;
            rb.AddForce(transform.up * jumpForce);
            energyManager.TakeEnergy(25);
            return true;
        }
        return false;
    }
    
    public void Dash(){
        if(energyManager.energy > 25 && Time.time >= nextDashTime){
            nextDashTime = Time.time + dashCooldown;
            rb.AddForce(transform.right * dashForce * transform.localScale.x);
            energyManager.TakeEnergy(25);
        }
    }

    /// Lifts the horizontal speed cap for a moment so knock-backs (bumpers, shockwaves...) are not squashed.
    public void SuspendSpeedCap(float seconds){
        speedCapSuspendedUntil = Mathf.Max(speedCapSuspendedUntil, Time.time + seconds);
    }

    void OnCollisionEnter2D(Collision2D collision){
        SliceBehaviour slice = collision.gameObject.GetComponent<SliceBehaviour>();
        if(slice != null){
            energyManager.TakeEnergy(100);

            // knock the player along the way the slice was travelling (a slice moves towards -direction)
            float push = slice.direction != 0 ? -slice.direction : -transform.localScale.x;
            Destroy(collision.gameObject);
            rb.AddForce(transform.right * 3000f * push);

            GameFeel.PopText(transform.position + Vector3.up * 1.5f, "OUCH!", new Color(1f, 0.35f, 0.35f), 5f);
            GameFeel.HitStop(0.06f);
        }
    }
}
