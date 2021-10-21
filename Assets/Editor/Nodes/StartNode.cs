using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : BaseNode
{
    public StartNode() { 
    
    }

    public StartNode(Vector2 _position, StoryletEditorWindow _editorWindow, StoryletGraphView _graphView) {
        editorWindow = _editorWindow;
        graphView = _graphView;
        title = "Start";
        SetPosition(new Rect(_position, defaultNodeSize));
        NodeGUI_id = Guid.NewGuid().ToString();

        AddOutputPort("Output");

        RefreshExpandedState();
        RefreshPorts();
    }
}
