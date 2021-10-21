using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryletNode : BaseNode
{
    private string name = "";
    private Dictionary<string, int> preRequirements = new Dictionary<string, int>();


    public string Name { get => name; set => name = value; }


    /*
    PreCondition:
        - Actors
        - Player Requirements [Player's Social Level and other stuff?]
        - Inventory Requirements
     */



    /*PostCondition
        - Changes to Player's Social Level
        - Changes to Inventory
     */  
}
