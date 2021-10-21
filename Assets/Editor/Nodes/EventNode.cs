using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{
    private DialogueEventSO dialogueEvent;
    private ObjectField objectField; 

    public DialogueEventSO DialogueEvent { get => dialogueEvent; set => dialogueEvent = value; }

    public EventNode() { }
    public EventNode(Vector2 _position, StoryletEditorWindow _editorWindow, StoryletGraphView _graphView) {
        editorWindow = _editorWindow;
        graphView = _graphView;
        title = "Event";
        SetPosition(new Rect(_position, defaultNodeSize));
        NodeGUI_id = Guid.NewGuid().ToString();

        AddInputPort("Input");
        AddOutputPort("Output");

        objectField = new ObjectField()
        {
            objectType = typeof(DialogueEventSO),
            allowSceneObjects = false,
            value = dialogueEvent
        };

        objectField.RegisterValueChangedCallback(value =>
        {
            dialogueEvent = objectField.value as DialogueEventSO;
        });


        objectField.SetValueWithoutNotify(dialogueEvent);

        mainContainer.Add(objectField);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void DefaultFieldValues()
    {
        objectField.SetValueWithoutNotify(dialogueEvent);
    }
}
