using UnityEngine;

public class RespawnState : BaseState
{
    [SerializeField] private float verticalDistance = 25.0f;
    [SerializeField] private float immunityTime = 1f;

    private float startTime;

    public override void Construct()
    {
        startTime = Time.time;
        motor.controller.enabled = false;
        motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
        motor.controller.enabled = true;

        motor.verticalVelocity = 0.0f;
        motor.currentLane = 0;
        motor.anim?.SetTrigger("Respawn");
    }

    public override Vector3 ProcessMotion()
    {
        //Apply gravity
        motor.ApplyGravity();

        //copied from Running State to create a return vector
        Vector3 m = Vector3.zero;
        m.x = motor.SnapToLane();
        m.y = motor.verticalVelocity;
        m.z = motor.baseRunSpeed;
        return m;
    }
    public override void Transition()
    {
        if (motor.isGrounded && (Time.time - startTime) > immunityTime)
            motor.ChangeState(GetComponent<RunningState>());

        if (InputManager.Instance.SwipeLeft)
        {
            motor.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            motor.ChangeLane(1);
        }
    }
}
