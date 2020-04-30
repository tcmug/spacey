using Godot;
using System;

public class bullet : RayCast
{
	private PackedScene hitEffect;
	private Node effects;
	private float TTL;
	private Vector3 movementNormal;

	public override void _Ready()
	{
		TTL = 1.0f;
		hitEffect = ResourceLoader.Load("res://effects/hit.tscn") as PackedScene;
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
	}
	
	public void Init(Vector3 origin, Vector3 normal, Vector3 rot) 
	{
		Translation = origin;
		movementNormal = normal;
		CastTo = new Vector3(0.0f, 0.0f, (movementNormal.Length() / 60));
		Rotation = rot;
	}

	public override void _PhysicsProcess(float delta) 
	{
		TTL -= delta;
		if (TTL < 0) {
			Free();
		} 
		else if (IsColliding()) {
			Vector3 cpoint = GetCollisionPoint();
			Particles obj = (Particles)hitEffect.Instance();
			var dir = movementNormal.Normalized().Reflect(GetCollisionNormal());
			effects.AddChild(obj);
			obj.LookAt(dir, new Vector3(0.0f, 1.0f, 0.0f));
			obj.Translation = cpoint;
			var collider = GetCollider();
			if (collider is PhysicalEntity) {
				if (GD.Randi() % 100 > 20) {
					float len = movementNormal.Length();
					this.movementNormal = -dir;
					this.Translation = cpoint;
					movementNormal += new Vector3(
						(float)GD.RandRange(-0.2f, 0.2f),
						(float)GD.RandRange(-0.2f, 0.2f),
						(float)GD.RandRange(-0.2f, 0.2f)
					);
					movementNormal = movementNormal.Normalized() * len;
					Translation += movementNormal * delta;
					this.LookAt(movementNormal, new Vector3(0, 1, 0));
				} else {
					(collider as PhysicalEntity).Damage(10);
					Free();
				}
			}
			obj.Emitting = true;
		} else {
			Translation += movementNormal * delta;
		}

	}

}
