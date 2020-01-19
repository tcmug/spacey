using Godot;
using System;

public class Turret : Spatial
{
	private Spatial barrelX, barrelY;
	private Spatial target = null;

	public override void _Ready() {
		barrelY = GetNode<Spatial>("BarrelY");
		barrelX = barrelY.GetNode<Spatial>("BarrelX");
	}

	public override void _Process(float delta) 
	{
		if (target != null && IsInstanceValid(target)) {
			var targetOrigin = target.GetGlobalTransform().origin;
			var pos = barrelY.GetGlobalTransform().XformInv(targetOrigin).Normalized();
			var right = pos.Dot(new Vector3(1, 0, 0));
			right = Math.Min(0.02f, Math.Max(-0.02f, right));
			barrelY.RotateY(right);
			
			var pos2 = barrelX.GetGlobalTransform().XformInv(targetOrigin).Normalized();
			var up = pos2.Dot(new Vector3(0, 1, 0));
			up = Math.Min(0.02f, Math.Max(-0.02f, up));
			barrelX.RotateX(-up);
			
			// Clamp barrel rotation.
			if (barrelX.Rotation.x > 0) {
				var rot = barrelX.Rotation;
				rot.x = 0;
				barrelX.Rotation = rot;
			}
		}
	}
	
	private void _on_Area_body_entered(object body)
	{
	 	if (body is Bot) {
			target = body as Spatial;
			GD.Print("Turret targeting");
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}


