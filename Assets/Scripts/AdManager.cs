using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get { return instance; } }
    private static AdManager instance;

    //config fields
    [SerializeField] private string gameID;
    [SerializeField] private string rewardVideoPlacementID; //flags that the user will be rewarded for watching a video
    [SerializeField] private bool testMode;

    private void Awake()
    {
        instance = this;
        Advertisement.Initialize(gameID, !testMode);
    }
    public void ShowRewardedAd() 
    {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(rewardVideoPlacementID, so);
    }
}
