using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine.Tilemaps;

public class Storylet
{
    private StoryArc storyArc;
    private PreConditions preconditions;
    private PostCondition postCondition;
    private string tileDisplayText;
    private string text;
    private string storyletName;

    public Storylet(string storyletName, StoryArc storyArc, PreConditions preConditions) {
        this.storyArc = storyArc;
        this.preconditions = preConditions;
        this.storyletName = storyletName;
    }

    public Storylet() {
        this.storyArc = StoryArc.ACTI;
    }

    public Storylet(string storyletName, StoryArc storyArc)
    {
        this.storyArc = storyArc;
        this.storyletName = storyletName;
    }

    public void SetText(string text) {
        this.text = text;
    }

    public string GetText() {
        return text;
    }

    public string GetName() {
        return storyletName;
    }

    public void SetName(string name) {
        this.storyletName = name;
    }

    public bool StoryletAvailable(CharachterManager charachter) {
        bool is_available = false;
        if (preconditions.storyletRequirements.Count == 0) {
            return true;
        }

        foreach (KeyValuePair<string, int> quality in charachter.charachterQualities) {
            if (preconditions.storyletRequirements.ContainsKey(quality.Key)) {
                if (preconditions.storyletRequirements[quality.Key] >= quality.Value)
                {
                    is_available = true;
                }
                else { is_available = false; }
            }
        }

        return is_available;
    }

    public StoryArc GetStoryArc() {
        return storyArc;
    }

    public void AddPreconditions(PreConditions preConditions) {
        this.preconditions = preConditions;
    }

    public PreConditions GetPrecondition() {
        return this.preconditions;
    }


    public PostCondition GetPostCondition() {
        return this.postCondition;
    }
    public void AddPostcondition(PostCondition postCondition) {
        this.postCondition = postCondition;
    }

    public void AddTileDisplayText(string text) {
        tileDisplayText = text;
    }

    public string GetTileDisplayText() {
        return tileDisplayText;
    }

}

