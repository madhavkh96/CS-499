
using System.Collections.Generic;

public class Storylet1 {

    public PreConditions Precondition { get; set; }
    public PostCondition Postcondition { get; set; }
    public string TileDisplayText { get; set; }
    public string StoryletText { get; set; }
    public int MoralityScale { get; set; }

    public Dictionary<string, int> charachterQualityUpdates = new Dictionary<string, int>();

    private Dictionary<string, int> characterRequiements = new Dictionary<string, int>();

    public void SetCharacterReq(Dictionary<string, int> value) {
        characterRequiements = value;
    }

    public void AddCharacterReq(KeyValuePair<string, int> pair) { 
        if (!characterRequiements.ContainsKey(pair.Key)) {
            characterRequiements.Add(pair.Key, pair.Value);
        }
    }

    public Dictionary<string, int> GetCharacterReq() {
        return characterRequiements;
    }


    public void AddCharachterUpdates(KeyValuePair<string, int> pair) {
        if (!charachterQualityUpdates.ContainsKey(pair.Key)) {
            charachterQualityUpdates.Add(pair.Key, pair.Value);
        }
    }
}
