using System.Collections.Generic;

public class PreConditions {

    public string position;
    public CharachterMood charachterMood;
    public List<string> actors = new List<string>();
    public int level;
    public Dictionary<string, int> storyletRequirements = new Dictionary<string, int>();

    public PreConditions() {
        this.position = null;
        this.charachterMood = CharachterMood.None;
    }

    public PreConditions(string char_position) {
        this.position = char_position;
        this.charachterMood = CharachterMood.None;
    }

    public PreConditions(string char_position, CharachterMood char_mood) {
        this.position = char_position;
        this.charachterMood = CharachterMood.None;
    }

    public void AddActor(string actor) {
        this.actors.Add(actor);
    }

    public void RemoveActor(string actor) {
        actors.Remove(actor);
    }

    public void AddStoryletReq(KeyValuePair<string, int> req) {
        storyletRequirements.Add(req.Key, req.Value);
    }

    public void SetCharachterMood(int mood) {
        charachterMood = (CharachterMood)mood;
    }


    public void AddStoryletReq(Dictionary<string, int> reqs) {
        foreach (KeyValuePair<string, int> req in reqs) {
            if (!storyletRequirements.ContainsKey(req.Key))
            {
                storyletRequirements.Add(req.Key, req.Value);
            }
        }
    }

}