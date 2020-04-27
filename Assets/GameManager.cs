using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI chapterTitle;
    public TextMeshProUGUI text;

    private void Start()
    {
        Storylet introduction;


        if (!DramaManager.instance.storylets.TryGetValue("Introduction", out introduction)) {
            Debug.LogError("No start point for the story found!");
            return;
        }

        chapterTitle.text = introduction.GetName();
        text.text = introduction.GetText();

    }

    void DebugDictionary() {
        foreach (KeyValuePair<string, Storylet> key in DramaManager.instance.storylets)
        {
            Debug.Log(key.Key);
        }

    }
}
