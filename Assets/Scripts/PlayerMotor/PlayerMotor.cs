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
    public Animator anim;
    public AudioClip deathSFX;

    private BaseState state;

    private bool isPaused;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        state.Construct();
        isPaused = true;

    }
    private void Update()
    {
        if (!isPaused)
            UpdateMotor();
    }
    private void UpdateMotor()
    {
        //Check if we are grounded
        isGrounded = controller.isGrounded;

        //How should we be moving based on the state
        moveVector = state.ProcessMotion();

        //are we trying to change state? (this needs to be checked every frame)
        state.Transition();

        //Feed our animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        //Move the player
        controller.Move(moveVector * Time.deltaTime);

    }
    public float SnapToLane()
    {
        float r = 0.0f; //the return value

        //if we are not directly on top of a lane, move, else, don't!
        if(transform.position.x != (currentLane * distanceInBetweenLanes)) // if we are not where we should be
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x; //how far do we need to go to get to where we should be
            r = (deltaToDesiredPosition > 0) ? 1 : -1; //if that distance is >0 then r = 1, otherwise if it is <0 then r = -1
            r *= baseSidewaySpeed; //this will be -10 or +10 depending on the r value above.  

            float actualDistance = r * Time.deltaTime;
            if(Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition)) //if actual distance is further than desired distance then clamp it down
                r = deltaToDesiredPosition * (1/Time.deltaTime); //then multiply deltaToDesiredPosition by 1/Time.deltaTime

        }
        else //do nothing
        {
            r = 0;
        }
        return r; 
    }
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1); //clamp on both left (-1) and right (1)
    }

    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }

    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if(verticalVelocity < -terminalVelocity)
            verticalVelocity = -terminalVelocity;
    }

    public void PausePlayer()
    {
        isPaused = true;
    }

    public void ResumePlayer()
    {
        isPaused = false;
    }

    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer); //LayerToName is a lookup function - it will lookup object at specified index, and returns the name associated with that index; hit.gameObject.layer is the int layer of the object that we are collding with
        if(hitLayerName == "Death")
        {
            ChangeState(GetComponent<DeathState>());
            AudioManager.Instance.PlaySFX(deathSFX, 0.5f);
        }
            
    }
    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        ChangeState(GameManager.Instance.motor.GetComponent<RunningState>());
        PausePlayer();
    }
}
