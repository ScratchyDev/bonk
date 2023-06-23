using UnityEngine;

public class JoystickHandler : MonoBehaviour
{
    public Joystick joystick;
    public Movement movement;

    void FixedUpdate(){
        movement.Move(joystick.Horizontal);
    }
}
