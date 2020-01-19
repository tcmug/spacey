using Godot;
using System;

public class SixthSense : Area
{
	
	[Signal]
	delegate void HeardSomething();

	private void _on_Area2_body_entered(object body)
	{
		if (body is Spatial) {
			var node = (Spatial)body;
			if (node.Name == "Player") {
				this.EmitSignal(nameof(HeardSomething));
			}
		}
	}
}