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



    /// <summary>
    /// Initial Story Setup
    /// </summary>
    void SetupStory() {
        Storylet1 introduction;

        if (!DramaManager1.instance.storylets.TryGetValue("Introduction", out introduction))
        {
            Debug.LogError("No start point for the story found!");
            return;
        }

        DramaManager1.instance.initialStartUp = true;
        UpdateStoryScreen(introduction);
    }


    void DebugDictionary() {
        foreach (KeyValuePair<string, Storylet1> key in DramaManager1.instance.storylets)
        {
            Debug.Log(key.Key);
        }

    }


    /// <summary>
    /// Main Fn:
    /// Is Called on clicking a choice.
    /// </summary>
    /// <param name="storylet"></param>
    public void UpdateStoryScreen(Storylet1 storylet)
    {

        GameObject choiceObjectParent = GameObject.Find("Choices");

        //Destroys existing Choices
        for (int i = 0; i < choiceObjectParent.transform.childCount; i++)
        {
            Transform child = choiceObjectParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        DramaManager1.instance.UpdateScene(storylet);

        foreach (KeyValuePair<string, Storylet1> pair in DramaManager1.instance.storylets)
        {
            if (DramaManager1.instance.ProcessStorylet(pair.Value))
            {
                GameObject tile = Instantiate(GameManager.instance.choicePrefab);
                tile.transform.SetParent(choiceObjectParent.transform);
                tile.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.TileDisplayText;
                tile.GetComponent<Button>().onClick.AddListener(() => UpdateStoryScreen(pair.Value));
            }
        }
    }
}