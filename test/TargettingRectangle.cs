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
				var target = GetParent().GetParent<PhysicalEntity>();
				GetNode<Label>("Node2D/Info").SetText(target.GetTargettingInfoText());
				GetNode<Node2D>("Node2D").SetPosition(cam.UnprojectPosition(GetGlobalTransform().origin));
			}
		} else {
			GetNode<Node2D>("Node2D").Visible = false;
		}
	}
}
