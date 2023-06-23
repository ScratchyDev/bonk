using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Movement movement;
    public BaseWeapon baseWeapon;

    void FixedUpdate(){
        movement.Move(Input.GetAxis("Horizontal"));

        if(Input.GetButtonDown("Attack")){
            baseWeapon.Attack();
        }

        if(Input.GetButtonDown("Dash")){
            movement.Dash();
        }

        if(Input.GetButtonDown("Jump")){
            movement.Jump();
        }
    }
}
