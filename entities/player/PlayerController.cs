using Godot;
using System;

public class PlayerController : Spatial
{

	private Vector3 torque;
	private float mouseSensitivity = 0.2f;
	private RayCast aimingRay;
	private Spatial aimingSight;
	private PhysicalEntity target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		aimingRay = GetNode<RayCast>("AimingRay");
		aimingSight = GetNode<Spatial>("AimingSight");
	}


	public PhysicalEntity get_object_under_mouse() {
		var mouse_pos = GetViewport().GetMousePosition();
		var camera = GetViewport().GetCamera();
		var ray_from = camera.ProjectRayOrigin(mouse_pos);
		var ray_to = ray_from + camera.ProjectRayNormal(mouse_pos) * 1000.0f;
		var space_state = GetWorld().DirectSpaceState;
		var selection = space_state.IntersectRay(ray_from, ray_to);
		
		try {
			return (selection["collider"] as PhysicalEntity);
		} catch {}
		return null;
	}
	
	public override void _Input(InputEvent ev)
	{
		if (Input.GetMouseMode() == Input.MouseMode.Captured) {
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

			PhysicalEntity entity = (PhysicalEntity)parent;

			Vector3 force = new Vector3(0,0,0);

			if (Input.IsActionJustPressed("fire")) {
				var ent = get_object_under_mouse();
				if (ent != null) {
					ent.Select();
					target = ent;
					(GetParent() as PhysicalEntity).LockOn(ent);
				} else {
					(GetParent() as PhysicalEntity).LockOn(null);
					target = null;
				}
			}
	
			if (target != null) {
				entity.Engage();
			}
			
			if (Input.IsActionPressed("fire")) {
				entity.Shoot();
			}

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
			a.Visible = true;
			var tar = a.GetParent() as AI_Controller;
			if (tar != null)
				(GetParent() as PhysicalEntity).LockOn(tar.GetEntity());
		}
	}
	
	
	private void _on_AimingThing_area_exited(object area)
	{
		Area a = area as Area;
		if (a != null) {
			a.Visible = false;
		}
	}

}

