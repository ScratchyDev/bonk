using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Variables")]
    public float speed;
    public float jumpForce;
    public float dashForce;
    public int maxJumps;

    [Header("References")]
    public EnergyManager energyManager;
    public Transform groundCheck;
    public LayerMask ground;

    [HideInInspector]
    public int direction;
    Rigidbody2D rb;
    int currentJumps;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, ground);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				currentJumps = 0;
		}

        if(rb.velocity.x > 5){
            rb.velocity = new Vector2(5, rb.velocity.y);
        }

        if(rb.velocity.x < -5){
            rb.velocity = new Vector2(-5, rb.velocity.y);
        }
    }

    public void Move(float amount){
        rb.AddForce(transform.right * amount * speed);
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
        if(currentJumps < maxJumps && energyManager.energy > 25){
            currentJumps++;
            rb.AddForce(transform.up * jumpForce);
            energyManager.TakeEnergy(25);    
        }
    }
    
    public void Dash(){
        if(energyManager.energy > 25){
            rb.AddForce(transform.right * dashForce * transform.localScale.x);
            energyManager.TakeEnergy(25);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.GetComponent<SliceBehaviour>() != null){
            energyManager.TakeEnergy(100);
            Destroy(collision.gameObject);
            rb.AddForce(transform.right * -3000f * transform.localScale.x);
        }
    }
}
