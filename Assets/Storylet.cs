using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

public class Storylet
{
    private StoryArc storyArc;
    private List<StoryletMotivation> storyletMotivations = new List<StoryletMotivation>();
    private string text;
    private string storyletName;

    public Storylet(string storyletName, StoryArc storyArc, List<StoryletMotivation> storyletMotivations) {
        this.storyArc = storyArc;
        this.storyletMotivations = storyletMotivations;
        this.storyletName = storyletName;
    }

    public Storylet(string storyletName, StoryArc storyArc, StoryletMotivation storyletMotivation){
        this.storyArc = storyArc;
        storyletMotivations.Add(storyletMotivation);
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

    public void AddStoryLetMotivation(StoryletMotivation storyletMotivation) {
        storyletMotivations.Add(storyletMotivation);
    }

    public List<StoryletMotivation> GetStoryletMotivations() {
        return storyletMotivations;
    }

    public StoryArc GetStoryArc() {
        return storyArc;
    }
}

