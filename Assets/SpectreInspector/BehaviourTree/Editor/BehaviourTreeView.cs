using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using Unity.VisualScripting;

using Edge = UnityEditor.Experimental.GraphView.Edge;
using System.CodeDom;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> onNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
    BehaviourTree tree;

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) 
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }
        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });
        }
        return graphViewChange;
    }

    public BehaviourTreeView() {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        tree.nodes.ForEach(n => CreateNodeView(n));

        tree.nodes.ForEach(n => {
            var childen = tree.GetChildren(n);
            childen.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);

            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var types = TypeCache.GetTypesDerivedFrom<Node>();
        foreach (var type in types)
        {
            var attrs = type.GetCustomAttributes(typeof(NodeCategoryAttribute), false);
            if (attrs.Length > 0)
            {
                foreach (var attr in attrs)
                {
                    NodeCategoryAttribute nodeCategory = (NodeCategoryAttribute)attr;
                    evt.menu.AppendAction($"{type.BaseType.Name + "/"}{nodeCategory.Category}/[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
                }
            }
            else
            {  
                evt.menu.AppendAction($"{type.BaseType.Name + "/"}Other/[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
            }
        }



        //{

        //    var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
        //    foreach (var type in types)
        //    {
        //        evt.menu.AppendAction($"Action Nodes/[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
        //    }
        //}
        //{
        //    var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        //    foreach (var type in types)
        //    {
        //        evt.menu.AppendAction($"Composite Nodes/[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
        //    }
        //}
        //{
        //    var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        //    foreach (var type in types)
        //    {
        //        evt.menu.AppendAction($"Decorator Nodes/[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
        //    }
        //}


    }


    void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = onNodeSelected;
        AddElement(nodeView);
    }


}

