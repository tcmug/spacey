using Godot;
using System;

public class PlayerLabel : Node2D
{
	float t = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Process(float delta)
	{
		t += delta;
		GetNode<Label>("Text").Visible = ((t * 1000) % 500) > 250;
		PhysicalEntity player = GetTree().GetRoot().GetNode<PhysicalEntity>("Spatial/Player");
		Spatial target = player.GetLockedOnTarget();
		
		if (target != null) {
			Vector3 point = target.GetGlobalTransform().origin;
			Godot.Camera cam = GetTree().GetRoot().GetCamera();
			float distance = player.GetGlobalTransform().origin.DistanceTo(point);
			if (distance >= 1000) {
				distance /= 1000;
				GetNode<Label>("Distance").SetText(string.Format("{0:0.0}km", distance));
			} 
		 	else {
				GetNode<Label>("Distance").SetText(string.Format("{0:0}m", distance));
			}
			if (!cam.IsPositionBehind(point)) {
				Viewport vp = cam.GetViewport();
				Vector2 pos = cam.UnprojectPosition(point);
				Viewport vp2d = GetViewport();
				Vector2 posx = new Vector2(
					pos.x / (vp.GetSize().x / vp2d.GetSize().x),
					pos.y / (vp.GetSize().y / vp2d.GetSize().y)
				);
				this.SetPosition(posx);
				this.Visible = true;
			} else {
				this.Visible = false;
			}
		}
    }

}
