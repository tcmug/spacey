using Godot;
using System;

public class Bot : PhysicalEntity
{

	[Signal]
	delegate void BotDies(Bot bot);

			// this.EmitSignal(nameof(BotDies), this);
	

}

