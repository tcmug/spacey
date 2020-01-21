using Godot;
using System;

public class Missile : RigidBody
{
	
	private PackedScene explosion;
	private float TTL = 15.0f;
	private int damageCaused = 0;
	private Vector3 torque = new Vector3();
	private Vector3 rotEffect = new Vector3();
	private Vector3 force = new Vector3();
	
	private Node effects;
	private Spatial target = null;
	
	public override void _Ready()
	{
		
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
		explosion = ResourceLoader.Load("res://effects/explosion.tscn") as PackedScene;
	}
	
	private void LookFollow(PhysicsDirectBodyState state, Transform currentTransform, Vector3 targetPosition)
	{
		var myPosition = currentTransform.origin;

		var dist = (myPosition - targetPosition).Length();
		var _targetDir = myPosition - (myPosition - targetPosition).Normalized();
		var targetDir = currentTransform.XformInv(_targetDir);
		
		var forward = targetDir.Dot(new Vector3(0, 0, 1));
		var rightn = targetDir.Dot(new Vector3(1, 0, 0));
		var upn = targetDir.Dot(new Vector3(0, 1, 0));
		
		currentTransform.origin = new Vector3();
		float spiral = dist * 0.01f;
		var worldVelDir = LinearVelocity.Normalized();
		var worldTargetDir = currentTransform.Xform(targetDir);
		var turnRate = 30.0f;
		if (forward > 0.99f) {
			GetNode<Particles>("Particles").Emitting = true;
			var correction = ((worldTargetDir - worldVelDir) * 20.0f) + (Transform.basis.z * 100);
			state.ApplyCentralImpulse(correction * state.GetStep());
		} else {
			GetNode<Particles>("Particles").Emitting = false;
			spiral = 0;
			turnRate = 5;
			var correction = (worldTargetDir - worldVelDir) * 40.0f;
			state.ApplyCentralImpulse(correction * state.GetStep());
		}
		float ss = (float)Math.Sin(TTL * 10.0f) * spiral;
		float cs = (float)Math.Cos(TTL * 10.0f) * spiral;
		var turn = new Vector3(-upn + ss, rightn - cs, 0) / turnRate;
		turn = Transform.Xform(turn) - Transform.origin;
		state.SetAngularVelocity(turn / state.GetStep());

	}

	public override void _IntegrateForces(PhysicsDirectBodyState state) 
	{	
		if (IsInstanceValid(target) && TTL > 5) {
			var targetPosition = target.GetGlobalTransform().origin;
			LookFollow(state, GetGlobalTransform(), targetPosition);
		} else {
			Disable();
		}
	} 
	
	private void Disable() {
		target = null;
		GetNode<Particles>("Particles").Emitting = false;
	}
		
	public override void _Process(float delta) {
		TTL -= delta;
		if (TTL < 0) {
			Free();
		}
	}
	


	private void _on_Missile_body_entered(object body)
	{
		if (IsInstanceValid(target) && body is Bot && body == target) {
			var obj = explosion.Instance() as explosion;
			var node = (Bot)target;
			node.Damage(damageCaused);
			effects.AddChild(obj);
			obj.Init(this.GetGlobalTransform().origin);
			TTL = 0;
		} else {
			Disable();
		}
	}


	public void Init(Vector3 origin, Vector3 normal, Vector3 rot, Spatial targ, Vector3 Velocity) 
	{
		target = targ;
		LinearVelocity = Velocity;
		Translation = origin;
		Rotation = rot;
	}

}

