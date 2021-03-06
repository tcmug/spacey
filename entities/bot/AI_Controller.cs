using Godot;
using System;

public class AI_Controller : Spatial
{
	
	PhysicalEntity bot;
	RayCast frontSensor;
	public enum State { Idle, Attacking, Evading, Avoiding }
	
	public State AIState;
	public Vector3 avoidTarget;
	public Spatial target;
	private Spatial sensors;
	private AudioStreamPlayer3D destroy;
	private AudioStreamPlayer3D detect;
	
	public override void _Ready()
	{       
		bot = (PhysicalEntity)GetParent();
		frontSensor = (RayCast)GetNode("Sensors/Front");
		this.destroy = (AudioStreamPlayer3D)GetNode("SfxDestroy");
		this.detect = (AudioStreamPlayer3D)GetNode("SfxDetect");
		this.sensors = (Spatial)GetNode("Sensors");
		AudioStreamPlayer3D engine = (AudioStreamPlayer3D)GetNode("Engine");
		engine.Play();
	}
	
	private bool TurnTowards(Vector3 target) {
		var target_normal = (target - bot.Translation).Normalized();
		var forward = bot.Transform.basis.z.Dot(target_normal);
		if (forward < 0.9999) {
			var right = bot.Transform.basis.x.Dot(target_normal);
			var up = bot.Transform.basis.y.Dot(target_normal);
			bot.turn(new Vector3(-up * 2, right * 2, 0));
			return false;
		}
		return true;
	}
	
	private float AngleTowards(Vector3 target) {
		var target_normal = (target - bot.Translation).Normalized();
		return bot.Transform.basis.z.Dot(target_normal);
	}
	
	private float Speed() {
		return bot.LinearVelocity.Length();
	}
	
	private float DistanceTo(Vector3 target) {
		return bot.Translation.DistanceTo(target);
	}	
	
	public override void _PhysicsProcess(float delta)
	{
		if (!IsInstanceValid(target)) {
			AIState = State.Idle;
			target = null;
		}
		
		if (sensors != null) {
			for (int at = 0; at < sensors.GetChildCount(); at++) 
			{
				RayCast ray = (RayCast)sensors.GetChild(at); 
				if (ray != null && ray.IsColliding()) {
					Vector3 cpoint = ray.GetCollisionPoint();
					Vector3 distance = (cpoint - bot.Translation);
					distance = bot.Transform.basis.Xform(distance);
					bot.boost(distance * -0.5f);
				}
			}
		}
		
		var targetOrigin = new Vector3();
		
		if (target is Bot) {
			targetOrigin = target.GetGlobalTransform().origin;
			var distanceToTarget = (targetOrigin - GetGlobalTransform().origin).Length();
			var relativeVelocity = (target as RigidBody).LinearVelocity - bot.LinearVelocity;
			targetOrigin += relativeVelocity * (distanceToTarget  / 300.0f);
		}
		
		switch (AIState) {
			case State.Attacking: {
				float speed = 0.25f;
				if (TurnTowards(targetOrigin)) {
					speed = 0.5f;
				}
				if (DistanceTo(targetOrigin) > 10) {
					bot.boost(new Vector3(0, 0, speed));
				} else {
					AIState = State.Evading;
				}
				if (AngleTowards(target.Translation) > 0.999f) {
					bot.Engage();
				}
			}
			break;
			case State.Evading: {
				Vector3 evasionTarget = bot.Translation - (targetOrigin - bot.Translation);
				float speed = 2.0f;
				if (TurnTowards(evasionTarget)) {
					speed = 1.0f;
				}
				if (DistanceTo(targetOrigin) > 15) {
					AIState = State.Attacking;
				} else {
					bot.boost(new Vector3(0, 0, speed));
				}
			}
			break;
			case State.Avoiding:
			break;
		}

	}

	public PhysicalEntity GetEntity() 
	{
		return bot;
	}

	private void _on_Area_body_entered(object body)
	{
		if (this.target == null && body is Bot) {
			Bot node = (Bot)body;
			if (node != bot) {
				AIState = State.Attacking;
				this.destroy.Play(); 
				bot.LockOn((Spatial)body);
				this.target = (Spatial)body;
			}
		}
	}
	
	private void _on_Area_body_exited(object body)
	{
		/*if (body is Spatial) {
			var node = (Spatial)body;
			if (node == this.target) {
				this.target = null;
			}
		}*/
	}
	
	
}




