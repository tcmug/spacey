using Godot;
using System;

public class TargettingRectangle : Area
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Visible) {
			GetNode<Node2D>("Node2D").Visible = true;
			Godot.Camera cam = GetTree().GetRoot().GetCamera();
			if (cam != null) {
				var target = GetParent<PhysicalEntity>();
				GetNode<Label>("Node2D/Info").SetText(target.GetTargettingInfoText());
				GetNode<Node2D>("Node2D").SetPosition(cam.UnprojectPosition(GetGlobalTransform().origin));
				
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
				GetNode<Label>("Node2D/Distance").SetText(distanceStr);
			}
		} else {
			GetNode<Node2D>("Node2D").Visible = false;
		}
	}
}
