using UnityEngine;
using UnityEngine.EventSystems;
namespace UITree {
	
public abstract class NodeRoot : UIBehaviour {
	[SerializeField] protected NodeCollection nodeCollection;
	[SerializeField] protected Transform childrenTransform;
	[SerializeField] protected TreeView treeView;
	[SerializeField] protected NodeRoot _parent;
	public Transform childrenParentTransform => childrenTransform;
	public NodeCollection nodes => nodeCollection;

	public TreeView tree {
		get => treeView;
		set => treeView = value;
	}

	public virtual void Redraw() {}

	public NodeRoot parentNode {
		get => _parent;
		set {
			_parent = value;
			if (_parent != null)
				transform.SetParent(_parent.childrenParentTransform.transform, false);
		}
	}
	public void Initialize() {
		nodeCollection.SetOwner(this);
	}
}
}
