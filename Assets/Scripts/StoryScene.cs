
using System.Collections.Generic;

public class StoryScene
{
    public string player_name;
    public List<string> actors = new List<string>();
    public List<string> deadActors = new List<string> { "None" };
    public string interactionObject;
    public string current_position;
    public Storylet1 currentStorylet;
    public int current_Level;
}
