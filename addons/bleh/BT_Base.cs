using Godot;
using System;


public class BT_Base : Node
{
	public enum State { Success, Failure, Running }

	public State tick(Node entity) 
	{
		var children = GetChildren();
		foreach (Node child in children) 
		{
			if (child is BT_Selector) 
				((BT_Selector)child).tick(entity);
		}
	}
}
