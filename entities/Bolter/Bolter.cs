using Godot;
using System;

public class Bolter : Spatial
{
	private RigidBody owner;
	
	private PackedScene bullet;
	private Node effects;
	private float counter = 0;
	private float delay = 2.0f;

	public override void _Ready()
	{
		Node parent = this;
		do {
			parent = parent.GetParent();
		} while (!(parent is RigidBody));
		owner = (RigidBody)parent;
		effects = GetNode<Node>("/root/Spatial/_Effects");
		bullet = ResourceLoader.Load("res://effects/bullet.tscn") as PackedScene;
	
	}
	
	public override void _Process(float delta)
	{
		if (counter > 0) {
			counter -= delta;
		}
		if (counter <= 0) {
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
