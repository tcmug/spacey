using Godot;
using System;

public class BT_Target : BT_Base 
{
	
	private Spatial target;

	public override State tick(Node _entity) 
	{
		var entity = (Bot)_entity;
		if (target != null) {
			
			var target_normal = (target.Translation - entity.Translation).Normalized();
			var forward = entity.Transform.basis.z.Dot(target_normal);
			//GD.Print(forward);
			if (forward < -0.99) {
				return State.Success;
			}
			
			var right = entity.Transform.basis.x.Dot(target_normal);
			var up = entity.Transform.basis.y.Dot(target_normal);
			entity.turn(new Vector3(up, -right, 0));
		}
		return State.Failure;
	}
	

	private void _on_Area_body_entered(object body)
	{
		if (body is Spatial) {
			var node = (Spatial)body;
			if (node.Name == "Player") {
				this.target = (Spatial)node;
			}
		}
	}
	
	private void _on_Area_body_exited(object body)
	{
		if (body is Spatial) {
			var node = (Spatial)body;
			if (node.Name == "Player") {
				this.target = null;
			}
		}
	}
	
	
	
}
