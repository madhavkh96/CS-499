using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Xml.XPath;

public class DramaManager : MonoBehaviour
{

    public Dictionary<string, Storylet> storylets = new Dictionary<string, Storylet>();
    public List<string> supportActors;
    public static DramaManager instance;
    public CharachterManager charachter = new CharachterManager();
    public bool initialStartUp = true;
    private Storylet currentStory = new Storylet();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }

        //StorySetup();
        AssetLoad();
        supportActors.Add("Player");

    }

    public void UpdateStoryScreen(Storylet currentStorylet) {

        currentStory = currentStorylet;
        GameObject choicesObjectParent = GameObject.Find("Choices");


        //Removing already present elements
        for (int i = 0; i < choicesObjectParent.transform.childCount; i++) {
            Transform child = choicesObjectParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        storylets.Remove(currentStorylet.GetName());

        //Updating the current position of the Story
        GameManager.instance.chapterTitle.text = currentStorylet.GetName();
        charachter.position = currentStorylet.GetPostCondition().position;
        GameManager.instance.text.text = ProcessText(currentStorylet.GetText());
        AddCurrentSupportActors(currentStorylet);



        //Displaying next possible storylets
        List<Storylet> nextStorylets = new List<Storylet>();
        foreach (KeyValuePair<string, Storylet> storylet in storylets) {
            Debug.Log("Current Post :" + currentStory.GetPostCondition().position);
            Debug.Log("<color=red>Next Pre: </color>" + storylet.Value.GetPrecondition().position);
            if (storylet.Value.GetPrecondition().position.Equals(currentStorylet.GetPostCondition().position)) {
                nextStorylets.Add(storylet.Value);
            }
        }


        foreach (Storylet storylet in nextStorylets) {
            GameObject choice = Instantiate(GameManager.instance.choicePrefab);
            choice.transform.parent = choicesObjectParent.transform;
            choice.GetComponentInChildren<TextMeshProUGUI>().text = storylet.GetTileDisplayText();
            choice.GetComponent<Button>().onClick.AddListener(() => UpdateStoryScreen(storylet));
        }
    }



    void StorySetup() {
        Storylet introduction = new Storylet("Introduction", StoryArc.ACTI);
        PostCondition introPost = new PostCondition();
        introPost.position = "Kids Room";
        introduction.AddTileDisplayText("Introduction");
        introduction.SetText("'Huf.. huf... huf..', You wake up sweating inside your room. You look at the walls and feel them closing in.");
        introduction.AddPostcondition(introPost);


        PreConditions part1aPre = new PreConditions("Kids Room");
        Storylet part1a = new Storylet("Escape1", StoryArc.ACTI);
        part1a.AddTileDisplayText("Crawl deeper in your blanket");
        part1a.SetText("You cover yourself with your blanket, and start counting down from 100." +
            " Trying to calm yourself.\n You suddenly hear a loud boom sound. Curious to what is happening, You grab your flashlight and step outside, your room to see what's happening ");


        PostCondition part1aPostC = new PostCondition();
        part1aPostC.updateToInventory.Add("FlashLight", 1);
        part1aPostC.position = "Outside";
        part1aPostC.charachterAttributes.Add("Adventurous", 2);
        part1a.AddPostcondition(part1aPostC);


        //Completed Part1A: -> Crawl Deeper in your blanker -> Outside with FlashLight
        part1a.AddPreconditions(part1aPre);
        part1a.AddPostcondition(part1aPostC);

        PreConditions part1bPre = new PreConditions("Kids Room");
        Storylet part1b = new Storylet("Escape2", StoryArc.ACTI);
        part1b.AddTileDisplayText("Run to your parents room");
        part1b.SetText("You grab your flashlight and run to your parents room, feeling safe only when you reach there, you climb on their bed and drift off to sleep. " +
            "\n You are woken up to commotion and see your dad walking outside with your flashlight, \n" +
            "curious you follow him outside.");

        PostCondition part1bPostC = new PostCondition();
        part1bPostC.actors.Add("Dad");
        part1bPostC.position = "Outside";

        //Completed Part1B: -> Run to Parents -> Outside with Dad without FlashLight
        part1b.AddPreconditions(part1bPre);
        part1b.AddPostcondition(part1bPostC);

        storylets.Add(introduction.GetName(), introduction);
        storylets.Add(part1a.GetName(), part1a);
        storylets.Add(part1b.GetName(), part1b);

    }

    string ProcessText(string textData) {
        Dictionary<string, string> storyletsTexts = new Dictionary<string, string>();
        string[] differentTexts = textData.Split(new char[] {  ':' });

        Debug.Log(supportActors.Count);
        Debug.Log(storyletsTexts.Count);
        foreach (string s in supportActors) {
            Debug.Log(s);
        }

        string current_actors;
        current_actors = supportActors[0];
        if (supportActors.Count > 1)
        {
            for (int i = 1; i < supportActors.Count; i++)
            {
                current_actors += "_" + supportActors[i];
                foreach (string storyText in differentTexts)
                {
                    if (storyText.Contains(current_actors))
                    {
                        string n = RemoveActorNames(storyText);
                        storyletsTexts.Add(current_actors, n);
                    }
                }
            }
        }
        else {
           if (differentTexts[1].Contains(current_actors))
            {
                string n = RemoveActorNames(differentTexts[1]);
                storyletsTexts.Add(current_actors, n);
            }
        }

        //foreach (KeyValuePair<string, string> keyValuePair in storyletsTexts) {
        //    Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value);
        //}

        if (supportActors.Count > 1 && storyletsTexts.TryGetValue(current_actors, out string returnString))
        {
            return returnString;
        }
        else {
            storyletsTexts.TryGetValue(current_actors, out returnString);
            return returnString;
        }
    }


    string RemoveActorNames(string text) {

        if (!text.Contains(")") && !text.Contains("("))
            return text;

        int startIndex = text.IndexOf("(");
        int endIndex = text.IndexOf(")");
        string modded = text.Remove(startIndex, endIndex - startIndex + 1);
        string returnString = RemoveActorNames(modded);
        return returnString;
    }
    void AddCurrentSupportActors(Storylet storylet) {
        foreach (string s in storylet.GetPostCondition().actors) {
            if (!supportActors.Contains(s)) { supportActors.Add(s); }
        }
    }

    //Loads Asset Text at the beginning of the Game Start from a .csv file
    void AssetLoad()
    {
        var file = Resources.Load<TextAsset>("sdata");

        string[] data = file.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Storylet storylet = new Storylet();
            PreConditions preConditions = new PreConditions();
            PostCondition postConditions = new PostCondition();
            storylet.SetName(row[1]);

            row[2] = row[2].Replace(";", ",");
            row[2] = row[2].Replace("new-l", "\n");
            storylet.SetText(row[2]);
            storylet.AddTileDisplayText(row[3]);
            preConditions.position = row[4];

            int pre_mood;
            if (int.TryParse(row[5], out pre_mood))
            {
                preConditions.SetCharachterMood(pre_mood);
            }
            if (row[6].Contains(";") || row[6].Contains(":")) {
                string[] storyReq = row[6].Split(new char[] { ';' });
                foreach (string req in storyReq) {
                    string[] point = req.Split(new char[] { ':' });
                    int value;
                    if (int.TryParse(point[1],out value)) {
                        KeyValuePair<string, int> pair = new KeyValuePair<string, int>(point[0], value);
                        preConditions.AddStoryletReq(pair);
                    }
                }
            }
            postConditions.position = row[7];

            int post_mood;
            if (int.TryParse(row[8], out post_mood))
            {
            }

            if (row[9].Contains(";") || row[9].Contains(":")) {
                string[] charAttib = row[9].Split(new char[] { ';' });
                foreach (string attrib in charAttib) {
                    string[] point = attrib.Split(new char[] { ':' });
                    int value;
                    if (int.TryParse(point[1], out value)) {
                        postConditions.charachterAttributes.Add(point[0], value);
                    }
                }
            }

            if (row[10].Contains(";") || row[10].Contains(":"))
            {
                string[] inventory = row[10].Split(new char[] { ';' });
                foreach (string item in inventory)
                {
                    string[] point = item.Split(new char[] { ':' });

                    int value;
                    if (int.TryParse(point[1], out value))
                    {
                        charachter.UpdateInventory(point[0], value);
                    }
                }
            }

            string[] actors = row[11].Split(new char[] { ';' });
            foreach (string actor in actors) {
                string result = actor.Trim();
                if (!result.Equals("*")) 
                {
                    postConditions.actors.Add(result);
                }
            }

            storylet.AddPreconditions(preConditions);
            storylet.AddPostcondition(postConditions);
            storylets.Add(storylet.GetName(), storylet);
        }

        Debug.Log(storylets.Count);
        foreach (KeyValuePair<string, Storylet> pair in storylets)
        {
            Debug.Log(pair.Key);
        }
    }
}
