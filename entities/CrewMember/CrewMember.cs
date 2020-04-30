using Godot;
using System;


public class CrewMember : Node
{
	
	static string[] firstNames = {
		"Cassidy",
		"John",
		"Marian",
		"Snoop",
		"Xavier",
		"Errol",
		"Carl",
		"Steve",
		"My",
		"Lee",
		"Anne",
		"Eggbert",
		"Lozar",
		"Jim",
		"Ariel",
		"Fudge",
		"Laura",
		"Ginny",
		"Cat",
		"Pepe"
	};
	
	static Random rnd = new Random();
	
	public string name;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		name = firstNames[rnd.Next(0, firstNames.Length)];
		GD.Print(name);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
