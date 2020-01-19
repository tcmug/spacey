using Godot;
using System;

public class UILock : Area
{

	private void _on_Area3_area_entered(Area area)
	{
	    (GetNode("Rectangle") as Spatial).Visible = true;
	}
	
	private void _on_Area3_area_exited(Area area)
	{
	    (GetNode("Rectangle") as Spatial).Visible = false;
	}

}

