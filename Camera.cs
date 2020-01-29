using Godot;
using System;

public class Camera : Godot.Spatial
{
	public enum CameraMode { Chase, Orbit }

	private CameraMode Mode;
	private Vector3 eul;
	private float mouseSensitivity = 0.02f;
	
	public override void _Ready() {
		Spatial following = (Spatial)GetParent().GetNode("Player");
		if (following != null) {
			this.Translate((following.Translation - this.Translation) + (following.GlobalTransform.basis.z * 10));
		}
		Mode = CameraMode.Chase;
	}
	
	public override void _Input(InputEvent ev)
	{
		if (ev is InputEventMouseMotion && Input.IsActionPressed("move_camera"))
		{
			var mouseEvent = (InputEventMouseMotion)ev;
			eul += new Vector3(
				mouseEvent.Relative.y * -mouseSensitivity,
				mouseEvent.Relative.x * -mouseSensitivity,
				0);
		}
	}	

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
		Spatial following = GetParent().GetNode<Spatial>("Player");
		Vector3 up = new Vector3(0, 1, 0);
		Vector3 camPos = GetGlobalTransform().origin;
		var targetTf = following.GetGlobalTransform();
		Vector3 target = targetTf.origin;
		targetTf.origin = new Vector3();
		if (following == null) {
			return;
		}
		switch (Mode) 
		{
			case CameraMode.Chase: 
			{
				Vector3 offset_target = target + (following.Transform.basis.z * -20)
					+ (following.Transform.basis.x * 0.0f) 
					+ (following.Transform.basis.y * 5.0f);
				Translation += (offset_target - camPos) * delta * 10;
				LookAt(target - (following.GlobalTransform.basis.z * -10), following.Transform.basis.y);
			}
			break;
			case CameraMode.Orbit:
			{
				Quat rotation = new Quat(eul);
				Vector3 orbit = new Vector3(0, 0, -20.0f);
				orbit = rotation.Xform(orbit);
				
				Translation += ((target + orbit) - camPos) * delta * 10;
				LookAt(target, up);
			}
			break;
		}
    }

}
