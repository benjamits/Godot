using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Movement")]
	[Export] private float _baseMovementSpeed = 5f;
	[Export] private float _rotationSpeed = 6f;
	
	[ExportGroup("References")]
	[Export] private Node3D _cameraPivot;
	
	private Vector3 _moveDirection = Vector3.Zero;

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

		_moveDirection = new Vector3(inputDirection.X, 0, inputDirection.Y).Normalized();
	} 

	private void HandleMovement()
	{
		if (_moveDirection == Vector3.Zero)
		{
			Velocity = Vector3.Zero;
			return;
		}

		Vector3 rotatedDirection = Transform.Basis.Z * _moveDirection.Z;

		Velocity = rotatedDirection * _baseMovementSpeed;
	}

	private void HandleRotation()
	{
		if (_moveDirection == Vector3.Zero)
		{
			return;
		}

		float currentY = Rotation.Y;
		float targetY = currentY - (_moveDirection.X * _rotationSpeed * (float)GetProcessDeltaTime());

		Rotation = new Vector3(Rotation.X, targetY, Rotation.Z);
	}

	private void HandleCameraFollow()
	{
		if (_cameraPivot == null) return;

		_cameraPivot.GlobalPosition = GlobalPosition + new Vector3(0, 2, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleRotation();
		HandleMovement();
		MoveAndSlide();
		HandleCameraFollow();
	}
}