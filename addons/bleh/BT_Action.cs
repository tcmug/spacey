using Godot;
using System;

public class BT_Action : BT_Base 
{
	public override State tick(Node entity) 
	{
		GD.Print("Doing something");
		return State.Success;
	}
}
