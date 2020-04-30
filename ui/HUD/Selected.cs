using Godot;
using System;

public class Selected : Node2D
{
	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		Godot.Camera cam = GetTree().GetRoot().GetCamera();
		if (cam != null) {
			var target = GetParent<PhysicalEntity>();
			GetNode<Label>("Info").SetText(target.GetTargettingInfoText());
			SetPosition(cam.UnprojectPosition(GetParent<Spatial>().GetGlobalTransform().origin));
			
			Spatial player = GetTree().GetRoot().GetNode<Spatial>("Spatial/Player");
			string distanceStr = "";
			if (player != null) {
				float distance = player.GetGlobalTransform().origin.DistanceTo(target.GetGlobalTransform().origin);
				if (distance >= 1000) {
					distance /= 1000;
					distanceStr = string.Format("{0:0.0}km", distance);
				} 
			 	else {
					distanceStr = string.Format("{0:0}m", distance);
				}
			}
			GetNode<Label>("Distance").SetText(distanceStr);
		}
	}
}
