using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private string name = " ";
    private Sprite faceImage;

    public string Name { get => name; set => name = value; }
    public Sprite FaceImage { get => faceImage; set => faceImage = value; }

    private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
    private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();

    private TextField texts_Field;
    private ObjectField faceImage_Field;
    private TextField name_Field;

    private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();

    public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
    public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }

    public DialogueNode() { }

    public DialogueNode(Vector2 _position, StoryletEditorWindow _editorWindow, StoryletGraphView _graphView) {
        editorWindow = _editorWindow;
        graphView = _graphView;
        title = "Dialogue";

        AddInputPort("Input");
        SetPosition(new Rect(_position, defaultNodeSize));
        NodeGUI_id = Guid.NewGuid().ToString();


        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType))) {

            texts.Add(new LanguageGeneric<string>
            {
                language = language,
                LanguageGenericType = "",
            });

              
        }

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


        name_Field = new TextField("");
        name_Field.RegisterValueChangedCallback(value => {
            name = value.newValue;
        });
        name_Field.SetValueWithoutNotify(name);
        name_Field.AddToClassList("TextName");
        mainContainer.Add(name_Field);


        //Text Box
        Label label_texts = new Label("Text Box");
        label_texts.AddToClassList("labelTexts");
        label_texts.AddToClassList("Label");
        mainContainer.Add(label_texts);

        texts_Field = new TextField("");
        texts_Field.RegisterValueChangedCallback(value => {
            texts.Find(text => text.language == editorWindow.Language).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.language == editorWindow.Language).LanguageGenericType);
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
            AddChoicePort(this);
        };


        titleButtonContainer.Add(button);

    }


    public void ReloadLanguage() {
        texts_Field.RegisterValueChangedCallback(value => {
            texts.Find(text => text.language == editorWindow.Language).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.language == editorWindow.Language).LanguageGenericType);

        //Add the audio functionality here As well if we change that
        
        //
        foreach (DialogueNodePort dialogueNodePort in dialogueNodePorts) {
            dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                dialogueNodePort.TextLanguages.Find(language => language.language == editorWindow.Language).LanguageGenericType = value.newValue;
            });
            dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguages.Find(language => language.language == editorWindow.Language).LanguageGenericType);
        }

    }

    public override void DefaultFieldValues()
    {
        texts_Field.SetValueWithoutNotify(texts.Find(language => language.language == editorWindow.Language).LanguageGenericType);
        faceImage_Field.SetValueWithoutNotify(faceImage);
        name_Field.SetValueWithoutNotify(name);
    }

    /// <summary>
    /// Adds an Output Choice Port from a Dialogue Node
    /// </summary>
    /// <param name="_baseNode"></param>
    /// <param name="_dialogeNodePort"></param>
    /// <returns></returns>
    public Port AddChoicePort(BaseNode _baseNode, DialogueNodePort _dialogeNodePort = null) {
        Port port = GetPortInstance(Direction.Output);
        int outputPortCount = _baseNode.outputContainer.Query("connector").ToList().Count();
        string outputPortName = $"Choice {outputPortCount + 1}";

        DialogueNodePort dialogueNodePort = new DialogueNodePort();
        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            dialogueNodePort.TextLanguages.Add(new LanguageGeneric<string>()
            {
                language = language,
                LanguageGenericType = outputPortName
            }); 
        }

        if (_dialogeNodePort != null) {
            dialogueNodePort.InputGuid = _dialogeNodePort.InputGuid;
            dialogueNodePort.OutputGuid = _dialogeNodePort.OutputGuid;

            foreach (LanguageGeneric<string> item in _dialogeNodePort.TextLanguages) 
            {
                dialogueNodePort.TextLanguages.Find(language => language.language == item.language).LanguageGenericType = item.LanguageGenericType;
            }
        
        }

        //Text for the port
        dialogueNodePort.TextField = new TextField();
        dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
        {
            dialogueNodePort.TextLanguages.Find(language => language.language == editorWindow.Language).LanguageGenericType = value.newValue;
        });

        dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguages.Find(language => language.language == editorWindow.Language).LanguageGenericType);
        port.contentContainer.Add(dialogueNodePort.TextField);

        //Delete button
        Button deleteBtn = new Button(() => DeletePort(_baseNode, port))
        {
            text = "X",
        };
        port.contentContainer.Add(deleteBtn);
        
        dialogueNodePort.MyPort = port;


        port.portName = "";

        dialogueNodePorts.Add(dialogueNodePort);

        _baseNode.outputContainer.Add(port);

        //Refresh
        _baseNode.RefreshPorts();
        _baseNode.RefreshExpandedState();

        return port;
    }


    /// <summary>
    /// Deletes the Output Port from a Dialogue Container
    /// </summary>
    /// <param name="_node"></param>
    /// <param name="_port"></param>
    private void DeletePort(BaseNode _node, Port _port)
    {
        DialogueNodePort temp = dialogueNodePorts.Find(port => port.MyPort == _port);
        dialogueNodePorts.Remove(temp);

        IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == _port);

        if (portEdge.Any()) {
            Edge edge = portEdge.First();
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);
            graphView.RemoveElement(edge);
        }

        _node.outputContainer.Remove(_port);

        _node.RefreshPorts();
        _node.RefreshExpandedState();
    }
}
