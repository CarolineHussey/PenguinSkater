using UnityEngine;

public class RunningState : BaseState
{
     public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;
        m.x = 0; //no movement
        m.y = -1.0f; // a small force to ensure the runner stays on the ground.
        m.z = motor.baseRunSpeed;
        return m;
    }

    public override void Transition() //what is the direction of the swipe?
    {
        if(InputManager.Instance.SwipeLeft)
        {
            motor.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            motor.ChangeLane(1);
        }
        if (InputManager.Instance.SwipeUp && motor.isGrounded)
        {
         //   motor.ChangeState(GetComponent<JumpingState>);
        }
    }
}
