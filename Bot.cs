using Godot;
using System;

public class Bot : RigidBody
{

	[Signal]
	delegate void BotDies(Bot bot);

	private float acceleration = 4000.0f;
	private float accelerationDampening = 50.0f;
	private float angularAcceleration = 5000.0f;
	private float angularDampening = 5000.0f;
	
	private Vector3 torque = new Vector3();
	private Vector3 rotEffect = new Vector3();
	private Vector3 force = new Vector3();
	
	private float rateDelay = 0;
	private float fireRate = 5;
	private PackedScene bullet;
	private PackedScene missile;
	private PackedScene explosion;
	private Node effects;
	private int health = 100;
	
	private Spatial lockedOn = null;
	private bool shootAlt = false;
	private bool shoot = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bullet = ResourceLoader.Load("res://effects/bullet.tscn") as PackedScene;
		missile = ResourceLoader.Load("res://entities/Missile/Missile.tscn") as PackedScene;
		explosion = ResourceLoader.Load("res://effects/explosion.tscn") as PackedScene;
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
	}
	    
	private void ApplyTorque(float delta) 
	{
		var vtorque = Transform.basis.Xform(GetTorqueAction());
		ApplyTorqueImpulse(vtorque * angularAcceleration * delta);
		ApplyTorqueImpulse(AngularVelocity * -angularDampening * delta);
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
		
	public override void _Process(float delta)
	{
		if (shoot) {
			rateDelay += delta;
			if (rateDelay > 1.0f / fireRate) 
			{
				var obj = bullet.Instance() as bullet;
				var x = GetGlobalTransform().basis.z;
				obj.Init(this.Transform.origin + (x * 4),  x * 300, Rotation);
				effects.AddChild(obj);
				((AudioStreamPlayer3D)GetNode("Shiit")).Play();
				rateDelay -= 1.0f / fireRate;
			}
		}
		
		if (shootAlt) {
			if (IsInstanceValid(lockedOn)) {
				var obj = missile.Instance() as Missile;
				var x = GetGlobalTransform().basis.z;
				var y = GetGlobalTransform().basis.y;
				obj.Init(this.Transform.origin + (y * -4),  x * 10, Rotation, lockedOn);
				effects.AddChild(obj);
				((AudioStreamPlayer3D)GetNode("Shiit")).Play();
			} else {
				lockedOn = null;
			}
		}
		
		shootAlt = false;
		shoot = false;
	}

	public override void _PhysicsProcess(float delta) 
	{
		ApplyLinear(delta);
		ApplyTorque(delta);
	}

	private Vector3 GetLinearAction() 
	{
		Vector3 ret = force;
		force = new Vector3(0, 0, 0);
		return ret;
	}
	
	private Vector3 GetTorqueAction()
	{
		Vector3 ret = torque;
		torque = new Vector3(0, 0, 0);
		return ret;
	}

	public void turn(Vector3 xyz) {
		torque += xyz;
	}
	
	public void boost(Vector3 xyz) {
		force += xyz;
	}
	
	public void Shoot() 
	{
		shoot = true;
	}
	
	public void ShootAlt() 
	{
		shootAlt = true;
	}	
	
	public void LockOn(Spatial target) {
		lockedOn = target;
	}
	
	public void Damage(int dmg)
	{
		this.health -= dmg;
		GD.Print("Bot damaged for " + dmg + " points");
		if (this.health <= 0) 
		{
			var obj = explosion.Instance() as explosion;
			obj.Init(this.Transform.origin);
			effects.AddChild(obj);
			this.health = 0;
			this.EmitSignal(nameof(BotDies), this);
			this.Free();
		}
	}
	

}

