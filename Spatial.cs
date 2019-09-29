using Godot;
using System;


public class Spatial : Godot.Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

	public override void _PhysicsProcess(float delta) 
	{
		try 
		{
			RigidBody player = (RigidBody)GetParent().GetNode("ship");
			var up = new Vector3(0, 1, 0);
			this.Transform = this.Transform.LookingAt(player.Transform.origin, up);
		}
		catch (Exception e) {
			GD.Print(e.Message);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
