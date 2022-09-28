using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDeath : GameState
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
    public override void Construct()
    {
        Debug.Log("GameStateDeath");
        GameManager.Instance.motor.PausePlayer();
        deathUI.SetActive(true);
        circleTimer.gameObject.SetActive(true);

        deathTime = Time.time;
        highScore.text = "High Score: TBD";
        currentScore.text = "TBD";
        fishTotal.text = "Fish Total: TBD";
        currentFish.text = "TBD";
        
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
    }
}
