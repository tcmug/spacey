using Godot;
using System;

public class explosion : Particles 
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AudioStreamPlayer3D explosionSfx;
	private float ttl;
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.explosionSfx = (AudioStreamPlayer3D)GetNode("Explosion");
		this.ttl = 0;
	}
	// Called when the node enters the scene tree for the first time.
	public void Init(Vector3 origin)
	{
		this.Translation = origin;
	}
	
	public override void _Process(float delta) 
	{
		if (ttl == 0) {
			this.explosionSfx.Play();
			this.SetEmitting(true);
			var parts = this.GetNode<Particles>("Particles2");
			if (parts != null)
				parts.SetEmitting(true);
		}
		ttl += delta;
		if (ttl > 4)
			Free();
	}


}
