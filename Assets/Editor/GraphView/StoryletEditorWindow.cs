using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.Experimental.GraphView;
using System.Globalization;
using System.Text;

public class StoryletEditorWindow : EditorWindow
{

    private StoryletSO currentStorylet;
    private StoryletGraphView graphView;


    private StoryBeat beatType = StoryBeat.Main;
    private LanguageType language = LanguageType.English;


    private ToolbarMenu storyBeatMenu;
    private ToolbarMenu languageMenu;
    private Label storyletName;

    private StoryletAsset asset;
    public StoryBeat BeatType { get => beatType; set => beatType = value; }
    public LanguageType Language { get => language; set => language = value; }
    public StoryletAsset Asset { get => asset; set => asset = value; }

    [OnOpenAsset(1)]
    public static bool ShowWindow(int _instanceId, int line) {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceId);

        if (item is StoryletSO) 
        {
            StoryletEditorWindow window = (StoryletEditorWindow)GetWindow(typeof(StoryletEditorWindow));
            window.titleContent = new GUIContent("Storylet Editor");
            window.currentStorylet = item as StoryletSO;
            window.minSize = new Vector2(500, 250);
            window.Load();
        }

        return false;
    }
    

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView() {
        graphView = new StoryletGraphView(this);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar() 
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>("StoryletViewStyleSheet");
        rootVisualElement.styleSheets.Add(styleSheet);
        Toolbar toolbar = new Toolbar();


        //Save buitton
        Button saveBtn = new Button(() => Save())
        {
            text = "Save"
        };

        toolbar.Add(saveBtn);

        //Load buitton
        Button loadBtn = new Button(() => Load())
        {
            text = "Load"
        };

        toolbar.Add(loadBtn);

        //Dropdown menu for StoryBeat Select
        storyBeatMenu = new ToolbarMenu();
        foreach (StoryBeat beat in (StoryBeat[])Enum.GetValues(typeof(StoryBeat))) 
        {
            storyBeatMenu.menu.AppendAction(beat.ToString(), new Action<DropdownMenuAction>(x => BeatSelect(beat, storyBeatMenu)));
        }

        toolbar.Add(storyBeatMenu);

        //Dropdown menu for LanguageSelect
        languageMenu = new ToolbarMenu();
        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType))) 
        {
            languageMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => LanguageSelect(language, languageMenu)));
        }

        toolbar.Add(languageMenu);

        //Name of current Storylet we Have open.
        storyletName = new Label("");
        toolbar.Add(storyletName);

        Button importBtn = new Button(() => ImportFile())
        {
            text = "Import File"
        };

        toolbar.Add(importBtn);
        

        //Adding to the stylesheet
        storyBeatMenu.AddToClassList("List");
        languageMenu.AddToClassList("List");
        storyletName.AddToClassList("storyletName");
        loadBtn.AddToClassList("Btn");
        toolbar.AddToClassList("toolbar");
        saveBtn.AddToClassList("Btn");
        importBtn.AddToClassList("Btn");


        rootVisualElement.Add(toolbar);

    }

    private void BeatSelect(StoryBeat _beat, ToolbarMenu _toolbarMenu) {
        _toolbarMenu.text = "Current Story Beat: " + _beat.ToString();
    }

    private void LanguageSelect(LanguageType _language, ToolbarMenu _toolbarMenu) {
        _toolbarMenu.text = "Language: " + _language.ToString();
        language = _language;
        graphView.LanguageReload();
    }

    private void ImportFile() {
        string path = EditorUtility.OpenFilePanel("Load Storylet File", Application.dataPath, "csv");

        if (!String.IsNullOrEmpty(path))
        {
            Asset = Assets.AssetLoad(path);
            Assets.GenerateNodes(Asset, this, graphView);
        }
    }

    private void Load() { 
        Debug.Log("Load");

        if (storyletName != null)
        {
            BeatSelect(StoryBeat.Main, storyBeatMenu);
            LanguageSelect(LanguageType.English, languageMenu);
            storyletName.text = "Storylet Name: " + currentStorylet.name;
        }
    }

    private void Save() {
        Debug.Log("Save");

        var _nodes = graphView.GraphNodes;
        Debug.Log(_nodes.Count);
        PopulateData(_nodes);
    }

    private void PopulateData(List<BaseNode> _nodes)
    {
        string path = Application.dataPath + "/Editor/Resources/" + $"{this.beatType}" + ".csv";
        
        File.Create(path).Dispose();

        var csv = new StringBuilder();


        foreach (var n in _nodes) 
        {
            if (n.NodeType is NodeType.StartNode)
            {
                StartNode _node = (StartNode)n;
                String str = String.Format("{0}, {1}", _node.name);
            }
            else if (n.NodeType is NodeType.EventNode)
            {
                //TODO
            }
            else if (n.NodeType is NodeType.DialogueNode)
            {
                DialogueNode _node = (DialogueNode)n;
                string dialogue = _node.Texts.Find(text => text.language == this.Language).LanguageGenericType;
                String str = String.Format("{0}, {1}", _node.Name, dialogue);
                Debug.Log(str);
                csv.AppendLine(str);
            }
            else if (n.NodeType is NodeType.EndNode) { 
                //TODO
            }
        }

        File.WriteAllText(path, csv.ToString());

        //using (var writer = new StreamWriter(path))
        //{
        //    foreach (Node n in _nodes)
        //    {
        //        string str = String.Format("{0}, {1}", n.name, n.contentContainer);
        //        writer.WriteLine(str);
        //        writer.Flush();
        //    }

        //    writer.Close();
        //}
    }
}
