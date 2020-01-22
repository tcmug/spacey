using Godot;
using System;

public class MissileLauncher : Attachment
{
	
	private PackedScene missile;
	private Node effects;
	private float counter = 0;
	private float delay = 5.0f;

	public override void _Ready()
	{
		base._Ready();
		effects = GetNode<Node>("/root/Spatial/_Effects");
		missile = ResourceLoader.Load("res://entities/Missile/Missile.tscn") as PackedScene;
	}

	public override void _ProcessAttachment(float delta)
	{
		if (counter > 0) {
			counter -= delta;
		}
		if (engaged && counter <= 0) {
			Fire();
			counter += delay;
		}
	}
	
	private void Fire() {
		var tf = GetGlobalTransform();
		var x = tf.basis.z;
		var y = tf.basis.y;
		
		var obj = missile.Instance() as Missile;
		obj.Init(tf.origin, x * 10, tf.basis.GetEuler(), owner, owner.LinearVelocity - y * 2);
		effects.AddChild(obj);
		//((AudioStreamPlayer3D)GetNode("Shiit")).Play();
	}
	
}
