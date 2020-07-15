using Godot;
using System;

public class TitleScreen : Node2D
{
	public override void _Ready()
	{
		var startButton = GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/Start");
		startButton.GrabFocus();
	}

	public override void _PhysicsProcess(float delta)
	{
		var startButton = GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/Start");
		var exitButton = GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/Exit");
		
		if (startButton.IsHovered())
			startButton.GrabFocus();
		
		if (exitButton.IsHovered())
			exitButton.GrabFocus();
	}
	
	private void OnStartPressed()
	{
		GetTree().ChangeScene("StageOne.tscn");
	}
	
	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}





