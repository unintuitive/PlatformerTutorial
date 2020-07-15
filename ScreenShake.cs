using Godot;
using System;
using Object = Godot.Object;

public class ScreenShake : Node2D
{
	private int _currentShakePriority = 0;
	private Random _random = new Random();
	
	private float RandRange(float min, float max)
	{
		return (float) _random.NextDouble() * (max - min) + min;
	}

	public void MoveCamera(Vector2 move)
	{
		GetParent()
			.GetNode<Player>("Player")
			.GetNode<Camera2D>("Camera2D")
			.Offset = new Vector2(RandRange(-1, 1), RandRange(-1, 1));
	}

	public void ShakeScreen(int shakeLength, int shakePower, int shakePriority)
	{
		if (shakePriority > _currentShakePriority)
		{
			_currentShakePriority = shakePriority;
			var tween = GetNode<Tween>("Tween");
			tween.InterpolateMethod(
				this,
				"MoveCamera",
				new Vector2(shakePower, shakePower),
				new Vector2(0, 0),
				shakeLength,
				Tween.TransitionType.Sine,
				Tween.EaseType.Out,
				0);
			tween.Start();
		}
	}
	
	public void OnTweenTweenCompleted(Object obj, String key)
	{
		_currentShakePriority = 0;
	}
}
