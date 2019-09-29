using Godot;
using System;

public class bullet : RayCast
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	private PackedScene hitEffect;
	private Node effects;
	private Vector3 movementNormal;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		hitEffect = ResourceLoader.Load("res://effects/hit.tscn") as PackedScene;
		CastTo = new Vector3(0.0f, 0.0f, -50.0f * 0.06f);
		effects = GetTree().GetRoot().GetNode("Spatial").GetNode("World").GetNode("Effects");
//		GetTree().GetRoot().GetNode("Spatial").GetNode("World").get_node("Effects")
    }
	
	public void Init(Vector3 origin, Vector3 normal, Vector3 rot) 
	{
		Translation = origin;
		movementNormal = normal;
		Rotation = rot;
	}

	
	public override void _PhysicsProcess(float delta) 
	{
		if (IsColliding()) {
			Particles obj = (Particles)hitEffect.Instance();
			var dir = movementNormal.Normalized().Reflect(GetCollisionNormal());
			obj.LookAt(dir, new Vector3(0.0f,1.0f,0.0f));
			obj.Translation = GetCollisionPoint();
			effects.AddChild(obj);
			obj.Emitting = true;
			Free();
		}
		else 
		{
			Translation += movementNormal * delta;
		}
	}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
