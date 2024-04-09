using Godot;
using System;
using System.Reflection;

public partial class ninjaGo : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.X > 1 || velocity.X < -1)
			sprite.Animation = "Running";
		else
			sprite.Animation = "Idle";

		if (velocity.Y > 0)		
			sprite.Animation = "Down";	
		else if(velocity.Y < 0)
			sprite.Animation = "Jumpping";

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
			
		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 50);

		}
		sprite.FlipH = velocity.X < 0;
		Velocity = velocity;
		MoveAndSlide();
	}
}
