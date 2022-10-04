using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameStateDeath : GameState, IUnityAdsListener
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;

    //Circle Timer Fields

    [SerializeField] private Image circleTimer;
    public float timeToDecision = 2.5f;
    private float deathTime;

    private void Start()
    {
        Advertisement.AddListener(this);
    }
    public override void Construct()
    {
        Debug.Log("GameStateDeath");
        GameManager.Instance.motor.PausePlayer();
        deathUI.SetActive(true);

        //Set high Score
        if (SaveManager.Instance.save.HighScore < (int)GameStat.Instance.score) //HighScore is a float (calculated by distance) but SaveState saves ighscore as an int so Highscore is cast as an int here 
        {
            SaveManager.Instance.save.HighScore = (int)GameStat.Instance.score;
            currentScore.color = Color.cyan;
        }
        else
        {
            currentScore.color = Color.white;
        }
        SaveManager.Instance.save.Fish += GameStat.Instance.currentFish;
        SaveManager.Instance.Save();

        deathTime = Time.time;
        highScore.text = "High Score: " + SaveManager.Instance.save.HighScore;
        currentScore.text = GameStat.Instance.ScoreToText();
        fishTotal.text = "Fish Total: " + SaveManager.Instance.save.Fish;
        currentFish.text = GameStat.Instance.FishToText();
        
    }

    public override void Destruct()
    {
        deathUI.SetActive(false);
    }
    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        circleTimer.color = Color.Lerp(Color.green, Color.red, ratio);
        circleTimer.fillAmount = 1 - ratio;

        if (ratio > 1)
        {
            circleTimer.gameObject.SetActive(false);
        }
        //old logic 
        //if (InputManager.Instance.SwipeDown)
        //    ToMenu();
        //if (InputManager.Instance.SwipeUp)
        //    ResumeGame();
    }

    public void TryResumeGame() 
    {
        AdManager.Instance.ShowRewardedAd();
    }
    public void ResumeGame()
    {
        brain.ChangeState(GetComponent<GameStateGame>()); 
        GameManager.Instance.motor.RespawnPlayer();
    }

    public void ToMenu()
    {
        brain.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();


    }

    public void EnableRevive()
    {
        circleTimer.gameObject.SetActive(true);
    }
    public void OnUnityAdsReady(string placementID)
    {
        
    }
    public void OnUnityAdsDidError(string message)
    {
        
    }
    public void OnUnityAdsDidStart(string placementID)
    {
        
    }
    public void OnUnityAdsDidFinish(string placementID, ShowResult showResult)
    {
        circleTimer.gameObject.SetActive(false);
        switch (showResult)
        {
            case ShowResult.Failed:
                ToMenu();
                break;
            case ShowResult.Finished:
                ResumeGame();
                break;
            default:
                break;
        }
    }
}