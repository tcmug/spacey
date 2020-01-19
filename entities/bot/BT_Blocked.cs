using Godot;
using System;

public class BT_Blocked : BT_Base 
{
	private RayCast ray;
	
	public override void _Ready()
	{        
		ray = (RayCast)GetOwner().GetNode("ForwardSensor");
	}
	
	public override State tick(Node entity) 
	{
		if (ray != null && ray.GetCollider() != null) {
			return State.Failure;
		}
		return State.Success;
	}
}
