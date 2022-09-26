public class GameStateGame : GameState
{
    public override void Construct()
    {
        base.Construct(); 
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Init);
    }
}
