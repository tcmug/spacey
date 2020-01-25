using Godot;
using System;

public class PlayerController : Spatial
{

	private Vector3 torque;
	private float mouseSensitivity = 0.2f;
	private RayCast aimingRay;
	private Spatial aimingSight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		aimingRay = GetNode<RayCast>("AimingRay");
		aimingSight = GetNode<Spatial>("AimingSight");
	}

	public override void _Input(InputEvent ev)
	{
	if (ev is InputEventMouseMotion)
		{
			var mouseEvent = (InputEventMouseMotion)ev;
			torque = torque + new Vector3(
				mouseEvent.Relative.y * -mouseSensitivity,
				mouseEvent.Relative.x * -mouseSensitivity,
				0
			);
		}
	}

	public override void _Process(float delta)
	{
		Node parent = GetParent();
		
		if (aimingRay.IsColliding()) {
			Vector3 cpoint = ToLocal(aimingRay.GetCollisionPoint());
			aimingSight.Translation = cpoint;
			aimingSight.Visible = true;
		} else {
			aimingSight.Visible = false;
		}

		if (parent != null) {

			Bot entity = (Bot)parent;

			Vector3 force = new Vector3(0,0,0);

			if (Input.IsActionPressed("fire"))
				entity.Shoot();

			if (Input.IsActionJustPressed("fire_alt"))
				entity.ShootAlt();

			if (Input.IsActionPressed("move_forward"))
			{
				force.z += 1;
			}

			if (Input.IsActionPressed("move_backward"))
			{
				force.z += -1;
			}

			if (Input.IsActionPressed("move_left"))
			{
				force.x += 1;
			}

			if (Input.IsActionPressed("move_right"))
			{
				force.x += -1;
			}

			entity.boost(force);

			if (Input.IsActionPressed("rotate_left"))
			{
				torque.z += -1;
			}

			if (Input.IsActionPressed("rotate_right"))
			{
				torque.z += 1;
			}
			Vector3 ret = torque;
			torque = new Vector3();
			entity.turn(ret);
		}
	}

	
	private void _on_AimingThing_area_entered(object area)
	{
		Area a = area as Area;
		if (a != null) {
			a.GetNode<Spatial>("Rectangle").Visible = true;
			(GetParent() as Bot).LockOn((a.GetParent() as AI_Controller).GetEntity());
			GD.Print(a.GetName());
		}
	}
	
	
	private void _on_AimingThing_area_exited(object area)
	{
		Area a = area as Area;
		if (a != null) {
			a.GetNode<Spatial>("Rectangle").Visible = false;
		}
	}

}

