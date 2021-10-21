using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Storylet/New Storylet")]
[System.Serializable]
public class StoryletSO : ScriptableObject
{
    

}


[System.Serializable]
public class LanguageGeneric<T> {
    public LanguageType language;
    public T LanguageGenericType;
}

[System.Serializable]
public class StoryletGeneric<T> {
    public StoryBeat storyBeat;
    public T StoryBeatGenericType;
}

[System.Serializable]
public class DialogueNodePort {
    public string InputGuid;
    public string OutputGuid;
    public Port MyPort;
    public TextField TextField;
    public List<LanguageGeneric<string>> TextLanguages = new List<LanguageGeneric<string>>();
}

[System.Serializable]
public class StoryletAsset {
    public string actorName;
    public Dictionary<string, Storylet1> storylets = new Dictionary<string, Storylet1>();
}