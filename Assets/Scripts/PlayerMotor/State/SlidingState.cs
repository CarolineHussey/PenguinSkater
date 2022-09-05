using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuraton = 1.0f;

    //collider logic
    private Vector3 initialCenter; 
    private float initialSize; //size of the collider at the start of the slide
    private float slideStart; //when the slide started

    public override void Construct()
    {
        motor.anim?.SetTrigger("Slide"); //the question mark is used here as a shorthand statement - if it's null, stop right here; if it's not null, continue
        slideStart = Time.time; //gives us the exact time that the slide ate=arted (to help determine when to trasition out of it)

        initialSize = motor.controller.height;

        initialCenter = motor.controller.center;

        motor.controller.height = initialSize * 0.5f;
        motor.controller.center = initialCenter * 0.5f;
    }

    public override void Destruct()
    {
        motor.anim?.SetTrigger("Running"); 
        motor.controller.height = initialSize;
        motor.controller.center = initialCenter;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(-1);
        
        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(1);
        
        if (!motor.isGrounded)
            motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeUp)
            motor.ChangeState(GetComponent<JumpingState>());

        if (Time.time - slideStart > slideDuraton) 
            motor.ChangeState(GetComponent<RunningState>());

    }

    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;
        m.x = 0.01f; //no movement
        m.y = -1.0f; // a small force to ensure the runner stays on the ground.
        m.z = motor.baseRunSpeed; //move at a certain speed - specified in baseRunSpeed.
        return m;
    }

}
