using System.Drawing;
using Godot;

public partial class ninjaGo : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D sprite;

	private CollisionShape2D collisionShape2D;
	private RectangleShape2D rectangleShape2D;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite2D");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		rectangleShape2D = (RectangleShape2D)collisionShape2D.Shape;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.X > 1 || velocity.X < -1)
			sprite.Animation = "Running";
		else
			sprite.Animation = "Idle";				
		if(velocity.Y < 0)
			sprite.Animation = "Jumpping";

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			if(velocity.Y > 0)
				sprite.Animation = "Down";
		}

			
		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Handle down.
		if (Input.IsActionJustPressed("ui_down") && IsOnFloor())
		{
			rectangleShape2D.Set("size",  new Vector2(36,25));
		}
		if (Input.IsActionJustReleased("ui_down") && IsOnFloor())
		{
			rectangleShape2D.Set("size",  new Vector2(36,50));
		}

		GD.Print(Position.X);
		GD.Print("y"+Position.Y);
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		var direction = Input.GetAxis("ui_left", "ui_right");
		if (direction!=0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 12);

		}
		sprite.FlipH = velocity.X < 0;
		Velocity = velocity;
		MoveAndSlide();
	}
}
