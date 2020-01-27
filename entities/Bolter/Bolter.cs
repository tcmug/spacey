using Godot;
using System;

public class Bolter : Attachment
{
	
	private PackedScene bullet;
	private Node effects;
	private float counter = 0;
	private float delay = 0.5f;

	public override void _Ready()
	{
		base._Ready();
		effects = GetNode<Node>("/root/Spatial/_Effects");
		bullet = ResourceLoader.Load("res://effects/bullet.tscn") as PackedScene;
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
		var obj = bullet.Instance() as bullet;
		obj.Init(tf.origin, (tf.basis.z * 300) + owner.LinearVelocity, tf.basis.GetEuler());
		effects.AddChild(obj);
		((AudioStreamPlayer3D)GetNode("Shiit")).Play();
	}
	
}
