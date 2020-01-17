using Godot;
using System;

public class HUD : Camera
{

	public override void _Process(float delta)
	{
		this.Transform = (GetParent().GetParent().GetParent() as Camera).Transform;
	}
}
