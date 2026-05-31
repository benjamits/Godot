using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Movement")]
	[Export] private float _baseMovementSpeed = 5f;
	[Export] private float _rotationSpeed = 6f;
	
	[ExportGroup("References")]
	[Export] private Node3D _playerModel;
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

		Velocity = _moveDirection * _baseMovementSpeed;
	}

	private void HandleRotation()
	{
		if (_playerModel == null || _moveDirection == Vector3.Zero)
		{
			return;
		}

		float targetAngle = -Mathf.Atan2(_moveDirection.X, -_moveDirection.Z);
		float smoothAngle = Mathf.LerpAngle(_playerModel.Rotation.Y, targetAngle, (float)(_rotationSpeed * GetProcessDeltaTime()));

		_playerModel.Rotation = new Vector3(_playerModel.Rotation.X, smoothAngle, _playerModel.Rotation.Z);
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleRotation();
		HandleMovement();
		MoveAndSlide();
	}
}