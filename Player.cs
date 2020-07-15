using Godot;
using System;

public class Player : KinematicBody2D
{
	/*
	 * This is a C# implementation of the platformer tutorial by UmaiPixel:
	 * https://www.youtube.com/watch?v=MMsMtPVUtUE&list=PLyckz_-Rzq6ClGevL2fneJ5YJnMPKWa4M
	 */
	
	public PackedScene Fireball;
	public PackedScene SuperFireball;
	// [Signal] public delegate void IsAttacking();
	private Vector2 _velocity;
	private int _speed = 60;
	private int _gravity = 10;
	private int _jumpPower = -220;
	private Vector2 _floor = new Vector2(0, -1);
	private bool _onGround = false;
	// This is the C# alternative to GDScript's preload(). C# cannot preload resources in Godot (yet).
	private PackedScene _fireBall = (PackedScene) ResourceLoader.Load("res://Fireball.tscn");
	private PackedScene _superFireBall = (PackedScene) ResourceLoader.Load("res://SuperFireball.tscn");
	private bool _isAttacking = false;
	private bool _isDead = false;
	private int _fireballPower = 1;
	
	// From UmaiPixel's Double Jump tutorial (see: https://www.youtube.com/watch?v=9lwI-dGvxk8)
	private int _jumpCount = 0;
	
	public override void _PhysicsProcess(float delta)
	{
		if (_isDead == false)
		{
			// var playerPosition = GetNode<Position2D>("Position2D");
			var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

			// Move right, left or idle
			if (Input.IsActionPressed("ui_right"))
			{
				if (_isAttacking == false || IsOnFloor() == false)
				{
					 _velocity.x = _speed;
					 if (_isAttacking == false)
					 {
						   animatedSprite.Play("run");
						   animatedSprite.FlipH = false;
						   
						   /* I found that it's better to get the fireBallPosition every keypress 
							* instead of creating a variable outside of the condition. Otherwise
							* the position may not be read correctly and will glitch out causing
							* fireball shooting position to flip spontaneously as the player moves.
							*/
						   var fireBallPosition = GetNode<Position2D>("Position2D");
						   
						   /* In the original code, you can simply modify the position directly with 
							* fireBallPostion.Position.x *= -1. But that doesn't work in C#. Instead
							* I create a vector with explicit settings for x & y, and assign that to
							* position.
							*/ 
						   if (Mathf.Sign(fireBallPosition.Position.x) == -1)
						   {
								 fireBallPosition.Position = new Vector2(
										x: fireBallPosition.Position.x * -1,
										y: fireBallPosition.Position.y
								 );
						   }
								
					 }
				}
			}
			else if (Input.IsActionPressed("ui_left"))
			{
				if (_isAttacking == false || IsOnFloor() == false)
				{
					 _velocity.x = -_speed;
					 if (_isAttacking == false)
					 {
						  animatedSprite.Play("run");
						  animatedSprite.FlipH = true;

						  var fireBallPosition = GetNode<Position2D>("Position2D");
						  if (Mathf.Sign(fireBallPosition.Position.x) == 1)
						  {
							  fireBallPosition.Position = new Vector2(
								   x: fireBallPosition.Position.x * -1,
								   y: fireBallPosition.Position.y
							  );
						  }
					 }
				}
			}
			else
			{
				_velocity.x = 0;

				if (_onGround && _isAttacking == false)
					 animatedSprite.Play("idle");
			}
			
			// Jump
			if (Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("ui_up"))
			{
				// Original tutorial code
				// if (_isAttacking == false)
				// {
				// 	 if (_onGround)
				// 	 {
				// 		   _velocity.y = _jumpPower;
				// 		   _onGround = false;
				// 	 }
				// }
				
				// Double jump code
				if (_isAttacking == false)
				{
					 if (_jumpCount < 1)
					 {
						 _jumpCount += 1;
						 _velocity.y = _jumpPower;
						 _onGround = false;
					 }
				}
			}
			
			// Attack 
			if (Input.IsActionJustPressed("ui_focus_next") && _isAttacking == false)
			{
				// GD.Print("Fireball");
				if (IsOnFloor())
					 _velocity.x = 0;
				
				_isAttacking = true;
				animatedSprite.Play("attack");
				
				/* Note: Translating this from GDScript into C# was not straightforward.
				 * You must first define the fireball as a PackedScene, then cast the
				 * instance to FireBall. Only then do you have access to the .Position
				 * attributes. Without doing this, there's no way to set the position!
				 */
				// var fireBallInstance = new Fireball(); // Not sufficient for instancing!
				if (_fireballPower == 1)
				{
					/* TODO: Fix how fireballs are instanced
					 * The scope of the if condition here interferes with shooting fireballs.
					 * If I have the variable declaration inside the if, I can't use it outside
					 * the if. But I also have trouble declaring the variable outside the scope,
					 * because there are two different kinds of types being instanced, a Fireball
					 * and a SuperFireball. For the meantime I've just duplicated the code for
					 * setting the fireball direction and position, but this could be refactored
					 * into something more DRY.
					 */
					var fireBallInstance = (Fireball) _fireBall.Instance();
					var fireBallPosition = GetNode<Position2D>("Position2D");
					
					if (Mathf.Sign(fireBallPosition.Position.x) == 1)
						fireBallInstance.SetFireballDirection(1);
					else
						fireBallInstance.SetFireballDirection(-1);
					
					GetParent().AddChild(fireBallInstance);
					fireBallInstance.Position = fireBallPosition.GlobalPosition;
				}
				else
				{
					var fireBallInstance = (SuperFireball) _superFireBall.Instance();
					var fireBallPosition = GetNode<Position2D>("Position2D");
					
					if (Mathf.Sign(fireBallPosition.Position.x) == 1)
						fireBallInstance.SetFireballDirection(1);
					else
						fireBallInstance.SetFireballDirection(-1);
					
					GetParent().AddChild(fireBallInstance);
					fireBallInstance.Position = fireBallPosition.GlobalPosition;
				}
			}

			_velocity.y += _gravity;

			if (IsOnFloor())
			{
				if (_onGround == false)
					 _isAttacking = false;
				// _onGround = true;
				// _jumpCount = 0;
				if (_onGround == false)
				{
					_onGround = true;
					_jumpCount = 0;
				}
			}
			else
			{
				if (_isAttacking == false)
				{
					_onGround = false;
					 
					// This condition allows for a single jump after falling off a ledge,
					// but not a double jump.
					if (_velocity.y < 0)
						animatedSprite.Play("jump");
					else
						animatedSprite.Play("fall");
				}
			}

			_velocity = MoveAndSlide(_velocity, _floor);

			if (GetSlideCount() > 0)
			{
				for (var i = 0; i < GetSlideCount(); i++)
				{
					KinematicCollision2D collision = GetSlideCollision(i);
					Godot.Object other = collision.Collider;
					if (other.HasMethod("Dead"))
						Dead();
				}
			}
		}
	}

	private void Dead()
	{
		_isDead = true;
		_velocity.x = 0;
		_velocity.y = 0;
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		animatedSprite.Play("dead");
		GetNode<Timer>("Timer").Start();
	}
	
	private void OnTimerTimeout()
	{
		GetTree().ChangeScene("TitleScreen.tscn");
	}
	
	private void OnAnimatedSpriteAnimationFinished()
	{
		_isAttacking = false;
	}

	private void CrownPowerUp()
	{
		_fireballPower = 2;
	}
}
