using TMPro;
using UnityEditor;
using UnityEngine;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private AudioClip gameLoopMusic;

    public override void Construct()
    {
        base.Construct(); 
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Init); 
        gameUI.SetActive(true);

        GameStat.Instance.OnCollectFish += OnCollectFish;
        GameStat.Instance.OnScoreChange += OnScoreChange;

        gameUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(gameLoopMusic, 0.5f);
    }

    public void OnCollectFish(int fishCollected)
    {
        fishCount.text = GameStat.Instance.FishToText();
    }

    public void OnScoreChange(float score)
    {
        scoreCount.text = GameStat.Instance.ScoreToText();
    }
    public override void Destruct()
    {
        gameUI.SetActive(false);
        GameStat.Instance.OnCollectFish -= OnCollectFish;
        GameStat.Instance.OnScoreChange -= OnScoreChange;
    }


    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}
