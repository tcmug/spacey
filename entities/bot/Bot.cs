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

	private PackedScene explosion;
	private Node effects;
	private int health = 100;
	
	private Spatial lockedOn = null;
	
	bool engaged = false;
	Node armament;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		armament = GetNode("_Armament");
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
		if (health <= 0) {
			Free();
			return;
		}
		
		// Engage attachments.
		if (engaged) {
			for (var at = 0; at < armament.GetChildCount(); at++)
			{
				Attachment wpn = armament.GetChild<Attachment>(at);
				wpn.Engage();
			}
			engaged = false;
		}
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
		engaged = true;
	}
	
	public void ShootAlt() 
	{
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
			effects.AddChild(obj);
			obj.Init(this.Transform.origin);
			this.health = 0;
			this.EmitSignal(nameof(BotDies), this);
		}
	}
	

}

