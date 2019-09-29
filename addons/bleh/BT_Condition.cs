using Godot;
using System;

public class BT_Condition : Node
{
	
	public enum State { Success, Failure, Running }
	
	public override void _Ready()
	{        
		ticks = 0;
	}
	
	int ticks;

	public State tick(Node entity) 
	{
		ticks++;
		if (ticks == 2) {
			ticks = 0;
			return State.Success;
		}
		return State.Running;
	}

}
