using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceBehaviour : MonoBehaviour
{
    [Header("Variables")]
    public float speed;

    [HideInInspector]
    public int direction;
    [HideInInspector]
    public GameObject player;

    void FixedUpdate(){
        transform.position += Vector3.left * (speed * direction);

        if(transform.position.x > 100){
            Destroy(transform.gameObject);
        }
    }
}
