using Godot;
using System;

public class ship_ctrl : RigidBody
{
	private float acceleration = 4000.0f;
	private float accelerationDampening = 50.0f;
	private float angularAcceleration = 5000.0f;
	private float angularDampening = 50.0f;
	
	private float phase = 0.0f;
	private float mouseSensitivity = 0.2f;
	private Vector3 torque = new Vector3();
	private Vector3 rotEffect = new Vector3();
	private AudioStreamPlayer impactSfx;
	private AudioStreamPlayer fireSfx;
	
	private float rateDelay = 0;
	private float fireRate = 10;
	private PackedScene bullet;
	private Node effects;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		impactSfx = (AudioStreamPlayer)GetNode("Impact");
		fireSfx = (AudioStreamPlayer)GetNode("Fire");
		bullet = ResourceLoader.Load("res://effects/bullet.tscn") as PackedScene;
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
	}
	    
	private void ApplyTorque(float delta) 
	{
		var torque = GetTorqueAction();
		var vtorque = Transform.basis.Xform(torque);
		ApplyTorqueImpulse(vtorque * angularAcceleration * delta);
		ApplyTorqueImpulse(AngularVelocity * -angularDampening);
		torque = new Vector3(0,0,0);
	}
	
	private void ApplyLinear(float delta)
	{
		Vector3 force = GetLinearAction();
		Vector3 dirForce = Transform.basis.Xform(force);
		ApplyCentralImpulse(dirForce * acceleration * delta);
		Vector3 lin_vel = LinearVelocity.Normalized();
		Vector3 remainder = lin_vel - dirForce.Normalized();
		ApplyCentralImpulse(remainder * -accelerationDampening);
	}
	
	public override void _PhysicsProcess(float delta) 
	{
	
		if ((GetCollidingBodies().Count > 0) && (!impactSfx.Playing))
		{
			impactSfx.Play();
		}
	
		if (Input.IsActionPressed("fire")) 
		{
			rateDelay += delta;
			if (rateDelay > 1.0f / fireRate) 
			{
				Shoot();
				rateDelay -= 1.0f / fireRate;
			}
		}

		ApplyLinear(delta);
		ApplyTorque(delta);
	}
	
	public override void _Input(InputEvent ev)
	{
	if (ev is InputEventMouseMotion) 
		{
			var mouseEvent = (InputEventMouseMotion)ev;
			torque = torque + new Vector3(
				mouseEvent.Relative.y * -mouseSensitivity, 
				mouseEvent.Relative.x * -mouseSensitivity, 
				0
			);
		}	
	}

// Player controllable:

	private Vector3 GetLinearAction() 
	{
		Vector3 force = new Vector3(0,0,0);
		
		if (Input.IsActionPressed("move_forward"))
		{
			force.z += -1;
		}
		
		if (Input.IsActionPressed("move_backward"))
		{
			force.z += 1;
		}
		
		if (Input.IsActionPressed("move_left"))
		{
			force.x += -1;
		}
	
		if (Input.IsActionPressed("move_right"))
		{
			force.x += 1;
		}
		return force;
	}
	
	private Vector3 GetTorqueAction()
	{
		if (Input.IsActionPressed("rotate_left"))
		{
			torque.z += 1;
		}
	
		if (Input.IsActionPressed("rotate_right"))
		{
			torque.z += -1;
		}
		Vector3 ret = torque;
		torque = new Vector3();
		return ret;
	}
	
	
	private void Shoot() 
	{
	
		//if (!fireSfx.Playing)
		fireSfx.Play();

		var obj = bullet.Instance() as bullet;
		Vector3 normal = -Transform.basis.z;
		Vector3 mnormal = normal * -100;
		obj.Init(this.Transform.origin + (normal * -6), mnormal, Rotation);
		effects.AddChild(obj);
		rateDelay -= 1.0f / fireRate;
	}

}
