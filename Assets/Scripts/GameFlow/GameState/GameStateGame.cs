using TMPro;
using UnityEditor;
using UnityEngine;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    public override void Construct()
    {
        base.Construct(); 
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Init); 
        gameUI.SetActive(true);

        fishCount.text = "TBD";
        scoreCount.text = "TBD";
    }

    public override void Destruct()
    {
        gameUI.SetActive(false);
    }


    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}
