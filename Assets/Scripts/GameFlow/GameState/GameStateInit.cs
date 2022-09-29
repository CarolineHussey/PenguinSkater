using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI hiScoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Game);
        hiScoreText.text = "High Score: " + "TBD";
        fishCountText.text = "Fish: " + "TBD";

        menuUI.SetActive(true);

    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameStat.Instance.ResetSession();
    }

    public void OnShopClick()
    {
        //brain.ChangeState(GetComponent<GameStateShop>());
        Debug.Log("Shop button click");
    }
}
