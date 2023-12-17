using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> children = new List<Node>();

    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }

    public override void AttachGameObject(GameObject gameObj)
    {
        base.AttachGameObject(gameObj);

        foreach(Node node in children)
        {
           node.AttachGameObject(gameObj);
        }
    }
}
