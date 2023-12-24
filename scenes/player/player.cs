using Godot;
using System;
using System.Drawing;

public partial class Player : CharacterBody2D
{
	private const int Speed = 50;

	public override void _Process(double delta)
	{
		var direction = Input.GetVector("left", "right", "up", "down");
		Velocity = direction * Speed;
		MoveAndSlide();
	}
}
