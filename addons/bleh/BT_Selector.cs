using Godot;
using System;

public class BT_Selector : BT_Base
{
	
	private int at;
	private State state;
	
	public override void _Ready()
	{        
		at = 0;
	}

	public override State tick(Node entity) 
	{
		var state = State.Success;
		for (; at < GetChildCount(); at++) 
		{
			Node child = GetChild(at); 
			if (child is BT_Base) {
				state = ((BT_Base)child).tick(entity);
				if (state != State.Failure)
					break;
			}
		}
		if (at == GetChildCount()) {
			GD.Print("restarting selector");
			at = 0;
		}
		return state;
	}
}
