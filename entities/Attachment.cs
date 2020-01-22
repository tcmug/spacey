using Godot;
using System;

public class Attachment : Spatial
{
	protected RigidBody owner;
	protected bool engaged = false;

	public override void _Ready()
	{
		Node parent = this;
		do {
			parent = parent.GetParent();
		} while (!(parent is RigidBody));
		owner = (RigidBody)parent;
	}
	
	public void Engage() 
	{
		engaged = true;
	}
	
	public override void _Process(float delta)
	{
		_ProcessAttachment(delta);
		engaged = false;
	}
	
	public virtual void _ProcessAttachment(float delta) {
	}
	
	public float GetRange() {
		return 0;
	}
}
