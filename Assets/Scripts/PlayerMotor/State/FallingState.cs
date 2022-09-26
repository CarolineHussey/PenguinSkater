using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BaseState
{
    public override void Construct()
    {
        motor.anim?.SetTrigger("Fall");
        Debug.Log("Falling State True");
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
        if (motor.isGrounded)
            motor.ChangeState(GetComponent<RunningState>());
    }
}
