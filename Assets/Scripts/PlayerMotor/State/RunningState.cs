using UnityEngine;

public class RunningState : BaseState
{
    public override void Construct()
    {
        motor.verticalVelocity = 0;
    }
     public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;
        m.x = 0.01f; //no movement
        m.y = -1.0f; // a small force to ensure the runner stays on the ground.
        m.z = motor.baseRunSpeed; //move at a certain speed - specified in baseRunSpeed.
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
        if (InputManager.Instance.SwipeUp && motor.isGrounded) //player must be grounded to perform a jump!
        {
            motor.ChangeState(GetComponent<JumpingState>());
        }
        if (!motor.isGrounded)
            motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeDown)
        {
            motor.ChangeState(GetComponent<SlidingState>());
        }
    }
}
