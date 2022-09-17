using UnityEngine;

public class GameStateInit : GameState
{
    public override void UpdateState()
    {
        if (InputManager.Instance.Tap)
            brain.ChangeState(GetComponent<GameStateGame>());
    }
}
