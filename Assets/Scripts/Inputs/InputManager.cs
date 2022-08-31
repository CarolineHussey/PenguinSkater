using UnityEngine;

//InputManager sets the flags to control PlayerMotor - InputManager confirms what move took place, PlayerMotor then performs the relevant action.  
//We need to ensure that InputManager is called before PlayerMotor so that PlayerMotor receives the right signals at the start of each frame. If playerMotor is called first then the actions will all be set to false and the player won't be able to do anything, even if InputManager is alled after it.  
//To prevent this from happening we can set the Script Execution Order in Project settings
//Go to Project Settings -> Script Execution Order and add 'InputManager' just below 'PlayerInput' to ensure that this script is called at the beginning of each frame.  
public class InputManager : MonoBehaviour
{
    // ensure there is only one instance in the scene
    private static InputManager instance;   
    public static InputManager Instance {  get { return instance; } } //use get to ensure it can't be over-written by another script. instance can only be set by Self.

    //Action Scheme
    private RunnerInputAction actionScheme;

    //Configs
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;


    //these properties will be get only (no setting!) - to 'get' the actionScheme in use during each frame. This set up protects the values of the getters - they can't be set - but allows us to get these actions from anywhere within our code (eg. use InputManager.instance.TouchPosition from another script, or call the pivate touchPosition from this script as we already have instance = this)
    #region public properties
    public bool Tap { get { return tap; } }
    public Vector2 TouchPosition { get { return touchPosition; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    #endregion

    #region privates
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag; //doesn't need a public value as it won;t be used outside of this script
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;

    #endregion

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    private void LateUpdate()
    {
        ResetInputs();
    }
    private void ResetInputs()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false; //a one-liner to set the boolean value of each of these privates in one go

    }

    private void SetupControl()
    {
       actionScheme = new RunnerInputAction();

        //register different action to our functions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }

    //these functions set the private values of the private actions declared above
    private void OnEndDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag; //returns how far our cursor has travelled since we pressed for the last time
        float sqrDistance = delta.sqrMagnitude; // returns the square distance

        //Confirm swipe 
        if (sqrDistance > sqrSwipeDeadzone) // swipe length meets definition of a swipe (we swiped for long enough!)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x>y) //if x > y, we are swiping left or right
            {
                if (delta.x > 0) //if x > 0, we are swiping right
                    swipeRight = true;
                else 
                    swipeLeft = true; //otherwise we are swiping left
            }
            else // if x !> y, we are swiping up or down
            {
                if (delta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
        }
        startDrag = Vector2.zero; // value is reset to zero to set up for the next swipe.  the values for the other actions are reset duing LateUpdate by calling ResetInputs()
    }

    private void OnStartDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }

    private void OnPosition(UnityEngine.InputSystem.InputAction.CallbackContext ctx) //gets the position of cursor / mouse / touchscreen
    {
        touchPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTap(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        tap = true;
    }

    public void OnEnable()
    {
        actionScheme.Enable();
    }
    public void OnDisable()
    {
        actionScheme.Disable();
    }
}
