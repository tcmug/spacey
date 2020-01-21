using Godot;
using System;

public class Turret : Spatial
{
	private Spatial barrelX, barrelY, emitPoint, muzzle;
	private Spatial target = null;
	private bool targetInSight = true;
	private float projectileSpeed = 200.0f;
	private float maxTurnSpeed = 0.01f;
	
	public override void _Ready() {
		barrelY = GetNode<Spatial>("BarrelY");
		barrelX = barrelY.GetNode<Spatial>("BarrelX");
		muzzle = barrelX.GetNode<Spatial>("Muzzle");
	}

	public override void _Process(float delta) 
	{
		if (target != null && IsInstanceValid(target)) {
			
			var targetOrigin = target.GetGlobalTransform().origin;
			var distanceToTarget = (targetOrigin - barrelX.GetGlobalTransform().origin).Length();
			targetOrigin += (target as RigidBody).LinearVelocity * (distanceToTarget  / projectileSpeed);
	
			var ypos = barrelY.GetGlobalTransform().XformInv(targetOrigin).Normalized();
			var right = ypos.Dot(new Vector3(1, 0, 0));
			barrelY.RotateY(Math.Min(maxTurnSpeed, Math.Max(-maxTurnSpeed, right)));
			
			var xpos = barrelX.GetGlobalTransform().XformInv(targetOrigin).Normalized();
			barrelX.GetNode<Spatial>("Target").Translation = targetOrigin;
			var up = xpos.Dot(new Vector3(0, 1, 0));
			barrelX.RotateX(-Math.Min(maxTurnSpeed, Math.Max(-maxTurnSpeed, up)));
			
			// Clamp barrel rotation.
			if (barrelX.Rotation.x > 0) {
				var rot = barrelX.Rotation;
				rot.x = 0;
				barrelX.Rotation = rot;
			}

			targetInSight = Math.Abs(up) < 0.01 && Math.Abs(right) < 0.01;
			/*
			if (fireTimer > 0) {
				fireTimer -= delta;
				Vector3 target = muzzle.Translation;
				target.z = 2.5f;
				muzzle.Translation = muzzle.Translation.LinearInterpolate(target, fireTimer * 0.05f);
			} else if (targetInSight) {
				fireTimer += fireDelay;
				Vector3 target = muzzle.Translation;
				target.z = 2.0f;
				muzzle.Translation = target;
				fire();
			}
			*/
		}
	}

	private void _on_Area_body_entered(object body)
	{
	 	if (body is Bot) {
			target = body as Spatial;
			GD.Print("Turret targeting");
		}
	}

}


