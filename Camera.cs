using Godot;
using System;

public class Camera : Godot.Spatial
{

	public override void _Ready() {
		Spatial following = (Spatial) GetParent().GetNode("Player");
		if (following != null) {
			this.Translate((following.Translation - this.Translation) + (following.GlobalTransform.basis.z * 10));
		}
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        Spatial following = (Spatial) GetParent().GetNode("Player");
		if (following != null) {
			Vector3 up = new Vector3(0, 1, 0);
			Vector3 camPos = this.Translation;
			Vector3 target = following.Translation;
			Vector3 offset_target = target + (following.Transform.basis.z * -6)
				+ (following.Transform.basis.x * -2.5f) 
				+ (following.Transform.basis.y * 2.5f);
			this.Translation += (offset_target - camPos) * delta * 10;
			this.LookAt(following.Translation - (following.GlobalTransform.basis.z * -10), following.Transform.basis.y);
		}
    }

}
