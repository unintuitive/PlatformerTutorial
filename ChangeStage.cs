using Godot;
using System;

public class ChangeStage : Area2D
{
	[Export(PropertyHint.File, "*.tscn")] public string TargetStage;
	
	private void OnChangeStageBodyEntered(Godot.Object body)
	{
		if (body.GetType() == typeof(Player))
		{
			GetTree().ChangeScene(TargetStage);
		}
	}
}


