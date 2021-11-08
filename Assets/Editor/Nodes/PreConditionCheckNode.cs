using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PreConditionCheckNode : BaseNode
{
    private string name = "";
    private Dictionary<string, int> preRequirements = new Dictionary<string, int>();

    private List<RectField> conditionContainer = new List<RectField>();


    private TextField nameField;
    private PopupWindow w = new PopupWindow();
    public string Name { get => name; set => name = value; }
    public List<RectField> ConditionContainer { get => conditionContainer; set => conditionContainer = value; }


    public PreConditionCheckNode() { }
    public PreConditionCheckNode(Vector2 _position, GraphEditorWindow _ediorWindow, StoryletGraphView _graphView) {
        editorWindow = _ediorWindow;
        graphView = _graphView;
        title = "Pre Condition Check";

        AddInputPort("Input");
        AddOutputPort("Output", Port.Capacity.Multi);
        SetPosition(new Rect(_position, defaultNodeSize));

        NodeGUI_id = Guid.NewGuid().ToString();

        Label label_name = new Label("Name");
        label_name.AddToClassList("labelName");
        label_name.AddToClassList("Label");
        mainContainer.Add(label_name);

        nameField = new TextField("");
        nameField.RegisterValueChangedCallback(value =>
        {
            name = value.newValue;
        });
        nameField.SetValueWithoutNotify(name);
        nameField.AddToClassList("TextName");
        mainContainer.Add(nameField);

        Label preConditionLabel = new Label("Conditions");
        preConditionLabel.AddToClassList("labelName");
        label_name.AddToClassList("Label");
        mainContainer.Add(preConditionLabel);

        Button btn = new Button(() => AddCondition(this))
        {
            text = "Add Condition"
        };

        mainContainer.Add(btn);

        RefreshExpandedState();
        RefreshPorts();
    }

    private class PreConditions{
        public PopupWindow name;
        public TextField value;
        public PreConditions() { name = new PopupWindow(); value = new TextField("0"); }
    }

    public void AddCondition(BaseNode _baseNode)
    {
        EnumField enumField = new EnumField(PreConditionType.Angry);
        enumField.AddToClassList("enumField");

        RectField container = new RectField();

        TextField value = new TextField();
        value.SetValueWithoutNotify("Insert Value");
        value.AddToClassList("enumValue");

        Button deleteBtn = new Button(() => DeleteCondition(_baseNode, container))
        {
            text = "X",
        };

        container.RemoveAt(0);

        deleteBtn.AddToClassList("deleteBtn");
        container.contentContainer.Add(deleteBtn);
        container.contentContainer.Add(enumField);
        container.contentContainer.Add(value);

        _baseNode.mainContainer.Add(container.contentContainer);

        conditionContainer.Add(container);

        _baseNode.RefreshExpandedState();
        _baseNode.RefreshPorts();
    }

    public void DeleteCondition(BaseNode _baseNode, RectField _container) {
        RectField temp = conditionContainer.Find(container => container == _container);
        conditionContainer.Remove(temp);

        _baseNode.mainContainer.Remove(temp);

        _baseNode.RefreshExpandedState();
        _baseNode.RefreshPorts();
    }

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
