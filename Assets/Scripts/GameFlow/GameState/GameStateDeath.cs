using UnityEngine;
public class GameStateDeath : GameState
{
    public override void Construct()
    {
        Debug.Log("GameStateDeath");
        GameManager.Instance.motor.PausePlayer();
    }
    public override void UpdateState()
    {
        if (InputManager.Instance.SwipeDown)
            ToMenu();
        if (InputManager.Instance.SwipeUp)
            ResumeGame();
    }

    private void ResumeGame()
    {
        brain.ChangeState(GetComponent<GameStateGame>()); 
        GameManager.Instance.motor.RespawnPlayer();
    }

    public void ToMenu()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
    }
}
