using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EndNode : BaseNode
{
    private EndNodeType endNodeType = EndNodeType.End;
    private EnumField enumField;

    public EndNodeType EndNodeType { get => endNodeType; set => endNodeType = value; }

    public EndNode() { }

    public EndNode(Vector2 _position, GraphEditorWindow _editorWindow, StoryletGraphView _graphView) 
    {
        editorWindow = _editorWindow;
        graphView = _graphView;
        title = "End";
        SetPosition(new Rect(_position, defaultNodeSize));
        NodeGUI_id = Guid.NewGuid().ToString();
        NodeType = NodeType.EndNode;


        AddInputPort("Input");

        enumField = new EnumField()
        {
            value = endNodeType
        };

        enumField.Init(endNodeType);

        enumField.RegisterValueChangedCallback((value) =>
        {
            endNodeType = (EndNodeType)value.newValue;
        });

        enumField.SetValueWithoutNotify(endNodeType);

        mainContainer.Add(enumField);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void DefaultFieldValues()
    {
        enumField.SetValueWithoutNotify(endNodeType);
    }
}
