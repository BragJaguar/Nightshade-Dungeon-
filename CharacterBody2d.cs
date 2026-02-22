using System;
using Godot;

public partial class CharacterBody2d : CharacterBody2D
{
	private const float MaxSpeed = 384.0f;
	private const float JumpVelocity = -512.0f;
	
	//State machine: 0=ground, 1=air,
	public int State;
	
	private double CoyoteTime;
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (State == 0)
		{
			MoveHorizontal(1.0f,128.0f);
			GroundedJump();
			
			if (!IsOnFloor()) { CoyoteTime += delta; if (CoyoteTime >= 0.2) { State = 1; } } else { CoyoteTime = 0; }
		}else if (State == 1)
		{
			MoveHorizontal(0.2f,8.0f);
			
			if (IsOnFloor()){State = 0;}
		}
		
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		void GroundedJump()
		{
			if (Input.IsActionJustPressed("ui_up"))
			{
				velocity.Y = JumpVelocity;
				State = 1;
			}
		}
		

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		void MoveHorizontal(float Acceleration, float Friction)
		{
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "0", "0");
			if (direction != Vector2.Zero)
			{
				velocity.X += ((MaxSpeed*direction.X - velocity.X)/8)*Acceleration;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Friction);
			}
		}
		

		Velocity = velocity;
		MoveAndSlide();
	}
}
