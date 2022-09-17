public class GameStateGame : GameState
{
    public override void Construct()
    {
        GameManager.Instance.motor.ResumePlayer();
    }
}
