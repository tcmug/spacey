using Godot;
using System;

public class BT_Sequence : Node 
{
	
	public enum State { Success, Failure, Running }
	
	private int at;
	
	public override void _Ready()
	{        
		at = 0;
	}

	public State tick(Node entity) 
	{
		var children = GetChildren();
		for (; at < GetChildCount(); at++) 
		{
			Node child = GetChild(at); 
			/*
			if (child is BT_Condition) {
				switch (((BT_Condition)child).tick(entity)) { 
				case State.Running:
					return Running;
				case State.Failure:
					state = childState;
					return State.Failure;
				}
			}
			*/
		}
		at = 0;
		return State.Success;
	}
}
