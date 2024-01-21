using System;
using Godot;

public partial class Bullet : Area2D
{
	private AudioStreamPlayer2D _shootSound;
	private Random _random = new();

	public Vector2 Direction { get; set; } = Vector2.Right;

	[Export]
	public int Speed { get; set; } = 200;

	public override void _Ready()
	{
		_shootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
		PlayShootSound();

		BodyEntered += SelfDestruct;
		AreaEntered += SelfDestruct;
	}

	public override void _Process(double delta)
	{
		Position += Direction * (float)(Speed * delta);
	}

	private void SelfDestruct(Node body)
	{
		QueueFree();
	}

	private void PlayShootSound()
	{
		SetRandomPitch(_shootSound);
		_shootSound.Play();
	}

	private void SetRandomPitch(AudioStreamPlayer2D streamPlayer)
	{
		var pitch = _random.NextSingle() * 0.8 + 1.2;
		streamPlayer.PitchScale = (float)pitch;
	}
}
