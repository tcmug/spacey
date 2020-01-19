using Godot;
using System;

public class AI_Controller : Spatial
{
	
	Bot bot;
	RayCast frontSensor;
	public enum State { Idle, Attacking, Evading, Avoiding }
	
	public State AIState;
	public Vector3 avoidTarget;
	public Spatial target;
	private AudioStreamPlayer3D destroy;
	private AudioStreamPlayer3D detect;
	
	public override void _Ready()
	{       
		bot = (Bot)GetParent();
		frontSensor = (RayCast)GetNode("Sensors/Front");
		this.destroy = (AudioStreamPlayer3D)GetNode("SfxDestroy");
		this.detect = (AudioStreamPlayer3D)GetNode("SfxDetect");
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
	
	public override void _Process(float delta)
	{
		if (!IsInstanceValid(target)) {
			AIState = State.Idle;
			target = null;
		}
		
		Spatial sensors = (Spatial)GetNode("Sensors");
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
		
		switch (AIState) {
			case State.Attacking: {
				float speed = 0.25f;
				if (TurnTowards(target.Translation)) {
					speed = 0.5f;
				}
				if (DistanceTo(target.Translation) > 10) {
					bot.boost(new Vector3(0, 0, speed));
				} else {
					AIState = State.Evading;
				}
				if (AngleTowards(target.Translation) > 0.999f) {
					bot.Shoot();
					bot.ShootAlt();
				}
			}
			break;
			case State.Evading: {
				Vector3 evasionTarget = bot.Translation - (target.Translation - bot.Translation);
				float speed = 2.0f;
				if (TurnTowards(evasionTarget)) {
					speed = 1.0f;
				}
				if (DistanceTo(target.Translation) > 15) {
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
	
	private void _on_Area2_HeardSomething()
	{
		this.detect.Play();
	}

	private void _on_Chatter_finished()
	{
		// conversation object ?
	    // get next in conversation
		// conversation target -> next line
		
	}
	

	
}




