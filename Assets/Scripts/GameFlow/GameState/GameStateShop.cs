using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI currentHat;
    public HatLogic hatLogic;

    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;

    protected override void Awake()
    {
        base.Awake();
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
            int index = i;
            GameObject go = Instantiate(hatPrefab, hatContainer) as GameObject;
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
            go.transform.GetChild(1).GetComponent<Image>().sprite = hats[index].Thumbnail;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();

        }
    }

    private void OnHatClick(int i)
    {
        currentHat.text = hats[i].ItemName;
        hatLogic.SelectHat(i);
    }

    public void OnHomeClick()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
    }
}
