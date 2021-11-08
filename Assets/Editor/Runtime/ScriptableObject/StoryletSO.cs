using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Storylet/New Storylet Template")]
[System.Serializable]
public class StoryletSO : ScriptableObject
{

    
}

[System.Serializable]
public class StoryletPreconditions {
    public HashSet<string> storyletLocation;
    
}