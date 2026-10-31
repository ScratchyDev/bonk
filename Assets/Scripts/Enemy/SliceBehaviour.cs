using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceBehaviour : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    [Tooltip("Slices that flew this far from where they started, or lived this long, remove themselves.")]
    public float maxDistance = 100f;
    public float lifetime = 8f;

    [HideInInspector]
    public int direction;
    [HideInInspector]
    public GameObject player;

    float startX;
    float age;

    void Start(){
        startX = transform.position.x;
    }

    void FixedUpdate(){
        transform.position += Vector3.left * (speed * direction);
        age += Time.fixedDeltaTime;

        // Only slices flying to the right were ever removed, so the ones flying left piled up forever.
        if(Mathf.Abs(transform.position.x - startX) > maxDistance || age > lifetime){
            Destroy(transform.gameObject);
        }
    }
}
