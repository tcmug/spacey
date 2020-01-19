using Godot;
using System;

public class BT_Shoot : BT_Base 
{
	public override State tick(Node _entity) 
	{
		var entity = (Bot)_entity;
		entity.Shoot();
		return State.Success;
	}
}
