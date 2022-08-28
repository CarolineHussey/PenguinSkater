using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;

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
