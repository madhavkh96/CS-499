using System.Collections.Generic;

public class PostCondition {
    public string position;
    public List<string> actors = new List<string>();
    public string deadActor;
    public string interactableObj;
    public Dictionary<string, int> charachterAttributes = new Dictionary<string, int>();
    public Dictionary<string, int> updateToInventory = new Dictionary<string, int>();
    public int level_post;
}