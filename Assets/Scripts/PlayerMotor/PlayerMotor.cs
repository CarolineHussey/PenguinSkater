using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    //These fields are public so they can be accessed in other scripts, but 'HideInInspector' so that they won't show there.
    [HideInInspector] public Vector3 moveVector; //how far away are we going to move in this frame
    [HideInInspector] public float verticalVelocity; //velocity when jumping or falling: positive = jumping; -1 or other very little value = on the floor; negative = free-falling
    [HideInInspector] public bool isGrounded; // are we grounded?
    [HideInInspector] public int currentLane; // -1 = left lane; 0 = center lane; 1 = right lane

    //public so they can be configured in the inspector
    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController controller;
    private BaseState state;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        state = GetComponent<RunningState>();
        state.Construct();

    }
    private void Update()
    {
        UpdateMotor();
    }
    private void UpdateMotor()
    {
        //Check if we are grounded
        isGrounded = controller.isGrounded;

        //How should we be moving based on the state
        moveVector = state.ProcessMotion();

        //are we trying to change state
        state.Transition();

        //Move the player
        controller.Move(moveVector * Time.deltaTime);

    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }

    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
}
