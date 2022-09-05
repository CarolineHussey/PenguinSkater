using UnityEngine;
//BaseState is a parent class that will control how the different states behave
public abstract class BaseState : MonoBehaviour //by making the script abstract, this class cannot exist on its own - it must be inherite before it can be used.  eg. see RunningState
{
    protected PlayerMotor motor;

    public virtual void Construct() { }
    public virtual void Destruct() { }
    public virtual void Transition() { }

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }
    public virtual Vector3 ProcessMotion()
    {
        Debug.Log("Process motion is not implemented in " + this.ToString());
        return Vector3.zero;
    }
}
