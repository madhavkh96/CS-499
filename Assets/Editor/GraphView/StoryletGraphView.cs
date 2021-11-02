using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StoryletGraphView : GraphView
{
    private string styleSheetName = "StoryletViewStyleSheet";
    private StoryletEditorWindow editorWindow;
    private NodeSearchWindow searchWindow;
    private List<BaseNode> graphNodes = new List<BaseNode>();

    public List<BaseNode> GraphNodes { get => graphNodes; set => graphNodes = value; }

    public StoryletGraphView(StoryletEditorWindow _editorWindow) {
        editorWindow = _editorWindow;

        StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(styleSheetName);
        styleSheets.Add(tmpStyleSheet);

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddSearchWindow();
    }

    private void AddSearchWindow() 
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Configure(editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port _startPort, NodeAdapter _nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = _startPort;

        ports.ForEach((port) =>
        {
            Port portView = port;

            if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != portView.direction) {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public void LanguageReload() {
        List<DialogueNode> dialogueNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            dialogueNode.ReloadLanguage();
        }
    }

    public StartNode CreateStartNode(Vector2 _pos) {
        StartNode temp = new StartNode(_pos, editorWindow, this);
        graphNodes.Add(temp);
        return temp;
    }
    public DialogueNode CreateDialogueNode(Vector2 _pos)
    {
        DialogueNode temp = new DialogueNode(_pos, editorWindow, this);
        graphNodes.Add(temp);
        return temp;
    }
    public EventNode CreateEventNode(Vector2 _pos)
    {
        EventNode temp = new EventNode(_pos, editorWindow, this);
        graphNodes.Add(temp);
        return temp;
    }
    public EndNode CreateEndNode(Vector2 _pos)
    {
        EndNode temp = new EndNode(_pos, editorWindow, this);
        graphNodes.Add(temp);
        return temp;
    }
}
