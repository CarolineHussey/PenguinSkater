using System;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    public static GameStat Instance { get { return instance; } }
    private static GameStat instance;
    public AudioClip fishCollectSFX;

    //Scores
    public float score;
    public float highScore;
    public float distanceModifier = 1.5f;

    //fish
    public int totalFish;
    public int currentFish;
    public float points = 0.0f;

    //Internal cooldown
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f;


    //Action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += currentFish * points;

        if (s > score)
        {
            score = s;
            if(Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
            
        }
    }   

    public void CollectFish()
    {
        currentFish++;
        OnCollectFish?.Invoke(currentFish);
        AudioManager.Instance.PlaySFX(fishCollectSFX, 0.2f);
    }

    public void ResetSession()
    {
        score = 0;
        currentFish = 0;
        OnCollectFish?.Invoke(currentFish);
        OnScoreChange?.Invoke(score);
    }

    public string ScoreToText()
    {
        return score.ToString("000000");
    }
    public string FishToText()
    {
        return currentFish.ToString("0000");
    }

}
