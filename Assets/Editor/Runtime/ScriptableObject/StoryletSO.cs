using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Storylet/New Storylet")]
[System.Serializable]
public class StoryletSO : ScriptableObject
{
    

}


public class StoryletGeneric<T> {
    public StoryBeat storyBeat;
    public T StoryBeatGenericType;
}
