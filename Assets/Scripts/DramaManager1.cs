using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DramaManager1 : MonoBehaviour
{
    public string inputFile;
    public static DramaManager1 instance;
    // Start is called before the first frame update
    public StoryScene scene = new StoryScene();
    public CharachterManager character = new CharachterManager();
    public Dictionary<string, Storylet1> storylets = new Dictionary<string, Storylet1>();
    public bool initialStartUp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    /// <summary>
    /// Main Fn:
    /// Is Called on clicking a choice.
    /// </summary>
    /// <param name="storylet"></param>
    public void UpdateStoryScreen(Storylet1 storylet) {

        GameObject choiceObjectParent = GameObject.Find("Choices");

        //Destroys existing Choices
        for (int i = 0; i < choiceObjectParent.transform.childCount; i++)
        {
            Transform child = choiceObjectParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        UpdateScene(storylet);

        foreach (KeyValuePair<string, Storylet1> pair in storylets) {
            if (ProcessStorylet(pair.Value)) {
                GameObject tile = Instantiate(GameManager.instance.choicePrefab);
                tile.transform.SetParent(choiceObjectParent.transform);
                tile.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.TileDisplayText;
                tile.GetComponent<Button>().onClick.AddListener(() => UpdateStoryScreen(pair.Value));
            }
        }
    }



    /// <summary>
    /// Updates the Current Scene with the provided storylet.
    /// </summary>
    /// <param name="storylet"></param>
    void UpdateScene(Storylet1 storylet) {
        storylets.Remove(storylet.TileDisplayText);
        scene.currentStorylet = storylet;
        foreach (string actor in storylet.Postcondition.actors) {
            Updatecharachter(actor);
        }
        scene.current_position = storylet.Postcondition.position;
        scene.current_Level = storylet.Postcondition.level_post;
        foreach (KeyValuePair<string, int> pair in storylet.charachterQualityUpdates) {
            character.UpdateCharachterQualities(pair.Key, pair.Value);
        }

        if (!string.IsNullOrEmpty(storylet.Postcondition.deadActor)) {
            scene.deadActors.Insert(0, storylet.Postcondition.deadActor); 
        }

        if (!string.IsNullOrEmpty(storylet.Postcondition.interactableObj)) {
            scene.interactionObject = storylet.Postcondition.interactableObj;
        }


        UpdateSceneText(storylet);

        GameManager.instance.chapterTitle.text = storylet.TileDisplayText;
        GameManager.instance.text.text = storylet.StoryletText;

    }

    /// <summary>
    /// Replaces the Scene Text with the Objects and the Actor Names
    /// </summary>
    /// <param name="storylet"></param>
    void UpdateSceneText(Storylet1 storylet) {
        storylet.StoryletText = storylet.StoryletText.Replace("_DEAD_", scene.deadActors[0]);
        storylet.StoryletText = storylet.StoryletText.Replace("#OBJ", scene.interactionObject);
        storylet.TileDisplayText = storylet.TileDisplayText.Replace("_DEAD_", scene.deadActors[0]);
        storylet.TileDisplayText = storylet.TileDisplayText.Replace("#OBJ", scene.interactionObject);
    }

    /// <summary>
    /// Checking what all storylets are valid for the current storylevel.
    /// </summary>
    /// <param name="storylet"></param>
    /// <returns></returns>
    bool ProcessStorylet(Storylet1 storylet) {
        bool returnValue = false;
        string current_posn = scene.current_position;
        string required_posn = storylet.Precondition.position;


        if (compareList(scene.actors, storylet.Precondition.actors) && current_posn.Equals(required_posn) && scene.current_Level == storylet.Precondition.level) {
            if (storylet.Precondition.storyletRequirements.Count > 0) {
                foreach (KeyValuePair<string, int> storyReq in storylet.Precondition.storyletRequirements)
                {
                    if (character.charachterQualities.ContainsKey(storyReq.Key))
                    {
                        if (character.charachterQualities[storyReq.Key] >= storyReq.Value) { returnValue = true; }
                        else { returnValue = false; }
                    }
                    else { returnValue = false; }
                }
            }
            else {
                returnValue = true;
            }
        }
        return returnValue;
    }

    /// <summary>
    /// Helper Function: Compares two Lists
    /// </summary>
    /// <param name="sceneList"></param>
    /// <param name="storyletList"></param>
    /// <returns></returns>
    bool compareList(List<string> sceneList, List<string> storyletList) {
        int count = sceneList.Count;
        if (count < storyletList.Count) { return false; }

        bool returnValue = true;
        
        foreach (string actor in storyletList) {
            if (sceneList.Contains(actor))
            {
                returnValue = true;
            }
            else {
                returnValue = false;
                break;
            }
        }

        return returnValue;
    }

    void PrintList(List<string> list, string indicator) {
        foreach (string item in list) {
            Debug.Log("<color=red>"+indicator+ " - "+item+"</color>");  
        }
    }


    /// <summary>
    /// Updates the character list that are currently present in the scene
    /// </summary>
    /// <param name="str"></param>
    void Updatecharachter(string str)
    {
        if (str.Contains("SUB"))
        {
            string actorName = str.Substring(3);
            if(scene.actors.Contains(actorName))
                scene.actors.Remove(actorName);
        }
        else if (str.Contains("ADD")) {
            scene.actors.Add(str.Substring(3));
        }
    }

    /// <summary>
    /// Loads Assests from the Excel file.
    /// </summary>
    public void AssetLoad()
    {
        var file = Resources.Load<TextAsset>(inputFile);

        string[] data = file.text.Split(new char[] { '\n' });
        

        for (int i = 3; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Storylet1 storylet = new Storylet1();
            PreConditions preConditions = new PreConditions();
            PostCondition postConditions = new PostCondition();
            //Display Text
            storylet.TileDisplayText = row[1];

            //Story Text
            row[2] = row[2].Replace(";", ",");
            row[2] = row[2].Replace("new-l", "\n");
            row[2] = row[2].Replace("DQ", "\"");
            row[2] = row[2].Replace("INSERT_NAME", scene.player_name);
            storylet.StoryletText = row[2];

            //Precondition Level
            if (int.TryParse(row[3], out int pre_level))
            {
                preConditions.level = pre_level;
            }

            //Morality Level
            if (int.TryParse(row[4], out int val)) {
                storylet.MoralityScale = val;
            }

            //Affected Charachteristics
            if (row[5].Contains(";") || row[5].Contains(":")){
                string[] attributes = row[5].Split(new char[] { ';' });
                foreach (string attribute in attributes) {
                    string[] value = attribute.Split(new char[] { ':' });
                    KeyValuePair<string, int> pair = new KeyValuePair<string, int>(value[0], int.Parse(value[1]));
                    storylet.AddCharachterUpdates(pair);
                }
            }

            //Precondition Actor
            string[] actors = row[6].Split(new char[] { ';' });
            foreach (string actor in actors) {
                if (!string.IsNullOrEmpty(actor) && !actor.Equals("*")) {
                    preConditions.AddActor(actor);
                }
            }


            // Pre Required Player Conditions
            if (row[7].Contains(";") || row[7].Contains(":"))
            {
                string[] reqd_qualities = row[7].Split(new char[] { ';' });
                foreach (string quality in reqd_qualities)
                {
                    string[] value = quality.Split(new char[] { ':' });
                    KeyValuePair<string, int> pair = new KeyValuePair<string, int>(value[0], int.Parse(value[1]));
                    preConditions.AddStoryletReq(pair);
                }
            }

            // Pre Required Position
            row[8] = row[8].Trim();
            preConditions.position = row[8];


            // Post Actors
            if ((row[9].Contains(";") || row[9].Contains(":")) && !row[9].Contains("*")) {
                string[] updateActors = row[9].Split(new char[] { ';' });
                foreach (string update in updateActors) {
                    if (!string.IsNullOrEmpty(update) && !update.Contains("*")) 
                    {
                        postConditions.actors.Add(update);
                    }
                }
            }

            //Post Condition Position
            row[10] = row[10].Trim();
            postConditions.position = row[10];

            //Post Level
            if(int.TryParse(row[11], out int post_level)){ 
                postConditions.level_post = post_level;
            }

            //Dead People
            if ((row[12].Contains(";") || row[12].Contains(":")) && !row[12].Contains("*"))
            {
                string[] updateDeadActors = row[12].Split(new char[] { ';' });
                postConditions.deadActor = updateDeadActors[0];

            }

            //Interactable Obj
            if ((row[13].Contains(";") || row[13].Contains(":")) && !row[13].Contains("*")) {
                string[] obj = row[13].Split(new char[] { ';' });
                        postConditions.interactableObj = obj[0];
            }

            storylet.Precondition = preConditions;
            storylet.Postcondition = postConditions;
            storylets.Add(storylet.TileDisplayText, storylet);
        }

        //foreach (KeyValuePair<string, Storylet1> storylet in storylets)
        //{
        //    Debug.Log(storylet.Key);
        //}
    }
}

