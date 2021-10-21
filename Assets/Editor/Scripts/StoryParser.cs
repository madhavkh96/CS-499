using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class StoryParser : MonoBehaviour
{

    public static string inputFile;

    public StoryletEditorWindow editorWindow;

    public Button button = new Button (){
        text = "Create"
    };
   
    public void ParseToGraph() {
        StoryletAsset storyletAsset = new StoryletAsset();
        
        storyletAsset = DramaManager1.AssetLoad(inputFile);
        
        

        foreach (Storylet1 storylet in storyletAsset.storylets.Values) { 
        
        }

    }
}

