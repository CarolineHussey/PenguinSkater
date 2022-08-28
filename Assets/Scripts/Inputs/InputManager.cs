using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // ensure there is only one instance in the scene
    private static InputManager instance;   
    public static InputManager Instance {  get { return instance; } } //use get to ensure it can't be over-written by another script. instance can only be set by Self.

    //Action Scheme
    private RunnerInputAction actionScheme;

    //Configs
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;


    //these properties will be get only (no setting!)
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
    private Vector2 startDrag;
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

    private void OnEndDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude;

        //Confirm swipe 
        if (sqrDistance > sqrSwipeDeadzone) // swipe length meets definition of swipe length
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x>y) //left or right
            {
                if (delta.x > 0)
                    swipeRight = true;
                else 
                    swipeLeft = true;
            }
            else
            {
                if (delta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
        }
        startDrag = Vector2.zero;
    }

    private void OnStartDrag(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }

    private void OnPosition(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
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
