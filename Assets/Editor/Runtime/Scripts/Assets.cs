using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Assets
{
    public static StoryletAsset AssetLoad(string _inputFile)
    {
        var file = File.ReadAllText(_inputFile);

        StoryletAsset storyletAsset = new StoryletAsset();

        string[] data = file.Split(new char[] { '\n' });


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
            row[2] = row[2].Replace("INSERT_NAME", storyletAsset.actorName);
            storylet.StoryletText = row[2];

            //Precondition Level
            if (int.TryParse(row[3], out int pre_level))
            {
                preConditions.level = pre_level;
            }

            //Morality Level
            if (int.TryParse(row[4], out int val))
            {
                storylet.MoralityScale = val;
            }

            //Affected Charachteristics
            if (row[5].Contains(";") || row[5].Contains(":"))
            {
                string[] attributes = row[5].Split(new char[] { ';' });
                foreach (string attribute in attributes)
                {
                    string[] value = attribute.Split(new char[] { ':' });
                    KeyValuePair<string, int> pair = new KeyValuePair<string, int>(value[0], int.Parse(value[1]));
                    storylet.AddCharachterUpdates(pair);
                }
            }

            //Precondition Actor
            string[] actors = row[6].Split(new char[] { ';' });
            foreach (string actor in actors)
            {
                if (!string.IsNullOrEmpty(actor) && !actor.Equals("*"))
                {
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
            if ((row[9].Contains(";") || row[9].Contains(":")) && !row[9].Contains("*"))
            {
                string[] updateActors = row[9].Split(new char[] { ';' });
                foreach (string update in updateActors)
                {
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
            if (int.TryParse(row[11], out int post_level))
            {
                postConditions.level_post = post_level;
            }

            //Dead People
            if ((row[12].Contains(";") || row[12].Contains(":")) && !row[12].Contains("*"))
            {
                string[] updateDeadActors = row[12].Split(new char[] { ';' });
                postConditions.deadActor = updateDeadActors[0];

            }

            //Interactable Obj
            if ((row[13].Contains(";") || row[13].Contains(":")) && !row[13].Contains("*"))
            {
                string[] obj = row[13].Split(new char[] { ';' });
                postConditions.interactableObj = obj[0];
            }

            storylet.Precondition = preConditions;
            storylet.Postcondition = postConditions;
            storyletAsset.storylets.Add(storylet.TileDisplayText, storylet);
        }

        //foreach (KeyValuePair<string, Storylet1> storylet in storylets)
        //{
        //    Debug.Log(storylet.Key);
        //}

        return storyletAsset;
    }

    public static void GenerateNodes(StoryletAsset _asset, StoryletEditorWindow _editorWindow, StoryletGraphView _graphView)
    {
        int _level = 0;
        Vector2 _position = new Vector2(702, 388);

        int[] nodesAtEachLevel = new int[50];

        foreach (KeyValuePair<string, Storylet1> storylet in _asset.storylets)
        {
            nodesAtEachLevel[storylet.Value.Precondition.level]++;
        }



        //Super Unoptimized O(n^2) Time Complexity [Make better System Design]              
        //Try making references to children nodes in the story for O(n)                  ]----------- Fix THIS SHIIEEET

        for (int i = 0; i < _asset.storylets.Count; i++)
        {
            int j = -1;
            foreach (KeyValuePair<string, Storylet1> storylet in _asset.storylets)
            {
                if (storylet.Value.Precondition.level == _level && nodesAtEachLevel[_level] != 0)
                {
                    j++;
                    DialogueNode node = _graphView.CreateDialogueNode(new Vector2(_position.x + (_level * 300), _position.y + (j * 450)));
                    _graphView.AddElement(node);

                    //Add the Title
                    node.ChangeName(storylet.Value.TileDisplayText);

                    //Add the Dialogue Text
                    LanguageGeneric<string> _lg = new LanguageGeneric<string>();
                    _lg.language = LanguageType.English;
                    string cleaned = Regex.Replace(storylet.Value.StoryletText, ".{35}", "$0\n");
                    _lg.LanguageGenericType = cleaned;
                    node.ChangeText(_lg);

                    node.RefreshExpandedState();
                    node.RefreshPorts();

                    foreach (KeyValuePair<string, Storylet1> childStorylet in _asset.storylets)
                    {
                        Debug.Log("Here");
                        if (childStorylet.Value.Precondition.level == _level + 1)
                        {
                            Debug.Log($"Adding Choices, {childStorylet.Key}");
                            Port port = node.AddChoicePort(node);

                            //Removes Content Container
                            port.contentContainer.RemoveAt(2);

                            port.portName = childStorylet.Value.TileDisplayText;
                            node.RefreshPorts();
                            node.RefreshExpandedState();
                        }
                    }

                    nodesAtEachLevel[_level]--;
                }
            }

            _level++;
        }
    }
}
