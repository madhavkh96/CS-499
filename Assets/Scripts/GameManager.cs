using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public CharachterManager charachter;
    public TextMeshProUGUI chapterTitle;
    public TextMeshProUGUI text;
    public GameObject initialStartScreen;
    public bool initialStart = false;

    public Button characterConfirm;

    public GameObject choicePrefab;


    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }
        
    }


    private void Start()
    {

        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("initialStart"))
        {
            PlayerPrefs.SetInt("initialStart", 1);
            initialStart = true;
            characterConfirm.onClick.AddListener(CharacterConfirmation);
        }
        else
        {
            charachter.charachterName = PlayerPrefs.GetString("player_name");
        }

        if (initialStart)
        {
            initialStartScreen.SetActive(true);
        }
        else {
            SetupStory();
        }

    }


    void CharacterConfirmation() {
        PlayerPrefs.SetString("player_name", GameObject.Find("NameInput").GetComponent<TMP_InputField>().text);
        DramaManager1.instance.scene.player_name = GameObject.Find("NameInput").GetComponent<TMP_InputField>().text;
        DramaManager1.instance.AssetLoad();
        initialStartScreen.SetActive(false);
        SetupStory();
    }




    void SetupStory() {
        Storylet1 introduction;

        if (!DramaManager1.instance.storylets.TryGetValue("Introduction", out introduction))
        {
            Debug.LogError("No start point for the story found!");
            return;
        }

        DramaManager1.instance.initialStartUp = true;
        DramaManager1.instance.UpdateStoryScreen(introduction);

    }
    void DebugDictionary() {
        foreach (KeyValuePair<string, Storylet1> key in DramaManager1.instance.storylets)
        {
            Debug.Log(key.Key);
        }

    }

}
