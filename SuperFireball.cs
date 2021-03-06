using Godot;
using System;

public class SuperFireball : Area2D
{
	private int _speed = 100;
	private Vector2 _velocity;
	private int _direction = 1;
	
	public void SetFireballDirection(int dir)
	{
		_direction = dir;
		if (dir == -1)
		{
			var animatedFireball = GetNode<AnimatedSprite>("AnimatedSprite");
			animatedFireball.FlipH = true;
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		_velocity.x = _speed * delta * _direction;
		Translate(_velocity);
		// My game art is not animated, but if it were, this is what we'd use:
		// var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		// animatedSprite.Play("shoot");
	}

	private void OnVisibilityNotifier2DScreenExited()
	{
		QueueFree();
	}

	private void OnSuperFireballBodyEntered(Godot.Object body)
	{
		if (body.GetType() == typeof(Enemy))
			body.Call("Dead", 2);

		QueueFree();
	}
}

