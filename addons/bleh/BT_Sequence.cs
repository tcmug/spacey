using Godot;
using System;

public class BT_Sequence : BT_Base
{
	
	private int at;
	
	public override void _Ready()
	{        
		at = 0;
	}

	public override State tick(Node entity) 
	{
		for (; at < GetChildCount(); at++) 
		{
			Node child = GetChild(at); 
			if (child is BT_Base) {
				var childState = ((BT_Base)child).tick(entity);
				switch (childState) { 
				case State.Running:
					return State.Running;
				case State.Failure:
					return State.Failure;
				}
			}
		}
		at = 0;
		return State.Success;
	}
}
