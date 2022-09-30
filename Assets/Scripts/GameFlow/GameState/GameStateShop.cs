using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI fishText;
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;

    private void Awake()
    {
        hats = Resources.LoadAll<Hat>("Hat");
        PopulateShop();
    }

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        fishText.text = SaveManager.Instance.save.Fish.ToString("00000");

        shopUI.SetActive(true);
    }

    public override void Destruct() 
    {
        shopUI.SetActive(false);
    }

    private void PopulateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            GameObject go = Instantiate(hatPrefab, hatContainer) as GameObject;
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(i));
            go.transform.GetChild(1).GetComponent<Image>().sprite = hats[i].Thumbnail;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hats[i].ItemName;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[i].ItemPrice.ToString();

        }
    }

    private void OnHatClick(int i)
    {
        Debug.Log("Hat Click! " + i);
    }
    public void OnHomeClick()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
    }
}
