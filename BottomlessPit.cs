using Godot;
using System;

public class BottomlessPit : Area2D
{
	private void OnBottomlessPitBodyEntered(Godot.Object body)
	{
		if (body.HasMethod("Dead"))
		{
			body.Call("Dead");
		}
	}
}


