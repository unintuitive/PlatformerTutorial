using Godot;
using System;

public class Crown : Area2D
{
	public override void _Ready()
	{
		var crownSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		crownSprite.Play("idle");
	}
	
	private void OnCrownBodyEntered(Godot.Object body)
	{
		if (body.HasMethod("CrownPowerUp"))
		{
			body.Call("CrownPowerUp");
			QueueFree();
		}
	}
}


