using Godot;
using System;

public class BT_Fly : BT_Base 
{
	public override State tick(Node _entity) 
	{
		var entity = (Bot)_entity;
		var forward_actual = entity.Transform.basis.z;
		entity.boost(new Vector3(0, 0, -1));
		return State.Success;
	}
}
