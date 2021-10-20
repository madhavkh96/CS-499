using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private string name = " ";
    private Sprite faceImage;

    public string Name { get => name; set => name = value; }
    public Sprite FaceImage { get => faceImage; set => faceImage = value; }

    private string dialogue_text;
    private TextField texts_Field;
    private ObjectField faceImage_Field;
    private TextField name_Field;
    
    public DialogueNode() { }

    public DialogueNode(Vector2 _position, StoryletEditorWindow _editorWindow, StoryletGraphView _graphView) {
        editorWindow = _editorWindow;
        graphView = _graphView;
        title = "Dialogue";
        
        SetPosition(new Rect(_position, defaultNodeSize));
        NodeGUI_id = Guid.NewGuid().ToString();

        AddInputPort("Input");

        //Face Image
        faceImage_Field = new ObjectField
        {
            objectType = typeof(Sprite),
            allowSceneObjects = false,
            value = faceImage
        };

        faceImage_Field.RegisterValueChangedCallback(value =>
        {
            faceImage = value.newValue as Sprite;
        });
        faceImage_Field.SetValueWithoutNotify(faceImage);
        mainContainer.Add(faceImage_Field);


        // Text Name
        Label label_name = new Label("Name");
        label_name.AddToClassList("labelName");
        label_name.AddToClassList("Label");
        mainContainer.Add(label_name);


        name_Field = new TextField("Name");
        texts_Field.RegisterValueChangedCallback(value => {
            name = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(name);
        texts_Field.AddToClassList("TextName");
        mainContainer.Add(texts_Field);



        //Text Box
        Label label_texts = new Label("Text Box");
        label_texts.AddToClassList("labelTexts");
        label_texts.AddToClassList("Label");
        mainContainer.Add(label_texts);

        texts_Field = new TextField("");
        texts_Field.RegisterValueChangedCallback(value => {
            dialogue_text = value.newValue;
        });
        texts_Field.multiline = true;

        texts_Field.AddToClassList("TextBox");
        mainContainer.Add(texts_Field);


        Button button = new Button()
        {
            text = "Add Choice"
        };
        button.clickable.clicked += () =>
        {
            //TODO: Add a new Choice Output Port.
        };
    }
}
