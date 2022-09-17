using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get { return instance; } }
    private static GameManager instance;

    public PlayerMotor motor;

    private GameState state;
    private void Awake()
    {
        instance = this;
        state = GetComponent<GameStateInit>();
        state.Construct();
    }

    private void Update()
    {
        state.UpdateState();
    }

    public void ChangeState(GameState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
}
