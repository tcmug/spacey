using Godot;
using System;

public class BT_Halfer : BT_Action 
{
	
	int n = 0;
	public override State tick(Node entity) 
	{
		this.n++;
		if (n < 5) {
			GD.Print("Busy doing x");
			return State.Running;
		}
		if (n == 10) 
			n = 0;
		if (n == 5) {
			GD.Print("Did!");
			return State.Success;
		}
		GD.Print("Failing");
		return State.Failure;
	}
}
