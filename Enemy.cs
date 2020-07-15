using Godot;
using System;

public class Enemy : KinematicBody2D
{
	
	[Export] public int _speed = 60;
	[Export] public int _hp = 1;
	[Export] public Vector2 Size = new Vector2(1, 1);
	
	private Vector2 _velocity;
	private int _gravity = 10;
	private readonly Vector2 _floor = new Vector2(0, -1);
	private int _direction = 1;
	private bool _isDead = false;

	public override void _Ready()
	{
		Scale = Size;
	}

	public void Dead(int damage)
	{
		_hp = _hp - damage;
		if (_hp <= 0)
		{
			var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
			var enemyTimer = GetNode<Timer>("Timer");
			
			_isDead = true;
			_velocity.x = 0;
			_velocity.y = 0;
			animatedSprite.Play("dead");
			// NOTE: Setting .Disabled = true directly results in an error.
			// Use the SetDeferred method so Godot waits until it's safe to disable.
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
			enemyTimer.Start();
			
			// Shake the screen when a large enemy dies.
			if (Scale > new Vector2(1, 1))
			{
				var screenShake = GetParent().GetNode<Node2D>("ScreenShake");
				screenShake.Call("ShakeScreen", 1, 10, 100);
			}
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (_isDead == false)
		{
			var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
			
			if (_direction == 1)
				animatedSprite.FlipH = false;
			else
				animatedSprite.FlipH = true;
			
			animatedSprite.Play("walk");
			
			_velocity.x = _speed * _direction;
			_velocity.y += _gravity;
			_velocity = MoveAndSlide(_velocity, _floor);
			
			var enemyRayCast = GetNode<RayCast2D>("RayCast2D");

			if (IsOnWall())
			{
				_direction *= -1;
				enemyRayCast.Position = new Vector2(
							 x: enemyRayCast.Position.x * -1,
							 y: enemyRayCast.Position.y
				);
			}
			
			if (enemyRayCast.IsColliding() == false)
			{
				_direction *= -1;
				enemyRayCast.Position = new Vector2(
							 x: enemyRayCast.Position.x * -1,
							 y: enemyRayCast.Position.y
				);
			}
			
			if (GetSlideCount() > 0)
			{
				for (var i = 0; i < GetSlideCount(); i++)
				{
					KinematicCollision2D collision = GetSlideCollision(i);
					Godot.Object other = collision.Collider;
					if (other.HasMethod("Dead"))
						other.Call("Dead");
				}
			}
		}
	}
		
	private void OnTimerTimeout()
	{
		QueueFree();
	}

}

