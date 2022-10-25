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
    private bool shopInit = false;

    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;


    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        hats = Resources.LoadAll<Hat>("Hat");
        shopUI.SetActive(true);
        fishText.text = SaveManager.Instance.save.Fish.ToString("00000");

        if (!shopInit)
        {
            
            currentHat.text = hats[SaveManager.Instance.save.CurrentHatIndex].ItemName;
            PopulateShop();
            shopInit = true;
        }
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
            if (SaveManager.Instance.save.UnlockedHatFlag[i] == 0)
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fish: " + hats[index].ItemPrice.ToString();
            else
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

        }
    }

    private void OnHatClick(int i)
    {
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHat.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            SaveManager.Instance.Save();
        }
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHat.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            fishText.text = SaveManager.Instance.save.Fish.ToString("00000");
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            SaveManager.Instance.Save();
        }
        else
        {
            Debug.Log("Not enough fish");
        }
    }

    public void OnHomeClick()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
    }
}
