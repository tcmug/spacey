using Godot;
using System;

public class PhysicalEntity: RigidBody
{

	private float acceleration = 4000.0f;
	private float accelerationDampening = 50.0f;
	private float angularAcceleration = 5000.0f;
	private float angularDampening = 5000.0f;
	
	private Vector3 torque = new Vector3();
	private Vector3 rotEffect = new Vector3();
	private Vector3 force = new Vector3();

	private PackedScene explosion;
	private Node effects;
	
	[Export] private int health = 100;
	[Export] private int crew = 5;
	
	private RigidBody lockedOn = null;
	Node armament;
	Node crewMembers;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		armament = GetNode<Node>("_Armament");
		crewMembers = GetNode<Node>("_CrewMembers");
		explosion = ResourceLoader.Load("res://effects/explosion.tscn") as PackedScene;
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
		
		if (crewMembers != null) {
			var member = ResourceLoader.Load("res://entities/CrewMember/CrewMember.tscn") as PackedScene;
			for (int i = 0; i < crew; i++)
			{
				var m = member.Instance() as CrewMember;
				crewMembers.AddChild(m);
			}
		}
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
		Attachment wpn = armament.GetChild<Attachment>(0);
		if (wpn != null)
			wpn.Engage();
	}
	
	public void ShootAlt() 
	{
		Attachment wpn = armament.GetChild<Attachment>(1);
		if (wpn != null)
			wpn.Engage();
	}
	
	public void LockOn(Spatial target) {
		if (target is RigidBody) {
			lockedOn = target as RigidBody;
			for (var at = 0; at < armament.GetChildCount(); at++)
			{
				Attachment wpn = armament.GetChild<Attachment>(at);
				wpn.Target(lockedOn);
			}
		} else {
			
		}
	}
	
	public void Damage(int dmg)
	{
		this.health -= dmg;
		GD.Print("Entity damaged for " + dmg + " points");
		if (this.health <= 0) 
		{
			var obj = explosion.Instance() as explosion;
			effects.AddChild(obj);
			obj.Init(this.Transform.origin);
			this.health = 0;
			// this.EmitSignal(nameof(BotDies), this);
		}
	}
	

}

