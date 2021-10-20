using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class StoryletEditorWindow : EditorWindow
{

    private StoryletSO currentStorylet;
    private StoryletGraphView graphView;


    private StoryBeat beatType = StoryBeat.Main;

    private ToolbarMenu toolbarMenu;
    private Label storyletName;


    public StoryBeat BeatType { get => beatType; set => beatType = value; }

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
        Button saveBtn = new Button()
        {
            text = "Save"
        };
        
        saveBtn.clickable.clicked += () =>
        {
            Save();
        };
        toolbar.Add(saveBtn);

        //Load buitton
        Button loadBtn = new Button()
        {
            text = "Load"
        };

        loadBtn.clickable.clicked += () =>
        {
            Save();
        };
        toolbar.Add(loadBtn);

        //Dropdown menu for Characterbeat
        toolbarMenu = new ToolbarMenu();
        foreach (StoryBeat beat in (StoryBeat[])Enum.GetValues(typeof(StoryBeat))) 
        {
            toolbarMenu.menu.AppendAction(beat.ToString(), new Action<DropdownMenuAction>(x => BeatSelect(beat, toolbarMenu)));
        }

        toolbar.Add(toolbarMenu);

        //Name of current Storylet we Have open.
        storyletName = new Label("");
        toolbar.Add(storyletName);

        //Adding to the stylesheet
        toolbarMenu.AddToClassList("beatList");
        storyletName.AddToClassList("storyletName");
        loadBtn.AddToClassList("loadBtn");
        toolbar.AddToClassList("toolbar");
        saveBtn.AddToClassList("saveBtn");


        rootVisualElement.Add(toolbar);
    }

    private void BeatSelect(StoryBeat beat, ToolbarMenu toolbarMenu) {
        toolbarMenu.text = "Current Story Beat: " + beat.ToString();
    }


    private void Load() { 
        Debug.Log("Load");

        if (storyletName != null)
        {
            BeatSelect(StoryBeat.Main, toolbarMenu);
            storyletName.text = "Storylet Name: " + currentStorylet.name;
        }
    }

    private void Save() {
        Debug.Log("Save");
    }
}
