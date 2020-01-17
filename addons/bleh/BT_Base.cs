using Godot;
using System;

public class BT_Base : Node
{
	public enum State { Success, Failure, Running }

	public virtual State tick(Node entity) 
	{
		return State.Failure;
	}
}
