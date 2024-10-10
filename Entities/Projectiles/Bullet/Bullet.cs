using System;
using Godot;
using Zoink.scripts;

namespace Zoink.scenes.Projectiles.Bullet;

public partial class Bullet : Area2D
{
	private PackedScene _bulletImpactScene;
	private AudioStreamPlayer _shootSound;
	private Sprite2D _sprite;

	private readonly Random _random = new();
	public Vector2 Direction { get; set; } = Vector2.Right;

	[Export]
	public int Speed { get; set; } = 200;

	public override void _Ready()
	{
		_bulletImpactScene = PackedSceneLoader.Load("Effects", "BulletImpact");
		_shootSound = GetNode<AudioStreamPlayer>("ShootSound");
		_sprite = GetNode<Sprite2D>("Sprite");
		PlayShootSound();

		BodyEntered += SelfDestruct;
	}

	public override void _Process(double delta)
	{
		Position += Direction * (float)(Speed * delta);
	}

	private void SelfDestruct(Node body)
	{
		if (body is TileMap tileMap) SpawnImpact(tileMap);

		if (!_shootSound.Playing)
		{
			QueueFree();
		}
		else
		{
			_sprite.Visible = false;
			Speed = 0;
			_shootSound.Finished += QueueFree;
		}
	}

	private void SpawnImpact(TileMap tileMap)
	{
		var tileCoordinates = tileMap.LocalToMap(Position);
		var tileOrigin = new Vector2(tileCoordinates.X * 16, tileCoordinates.Y * 16);
		var tileCenter = tileMap.MapToLocal(tileCoordinates);

		var direction = tileCenter - GlobalPosition;
		var directionNormalized = direction.Normalized();
		var directionAbs = new Vector2(Math.Abs(directionNormalized.X), Math.Abs(directionNormalized.Y));

		var bx = GlobalPosition.X;
		var by = GlobalPosition.Y;
		var rx = tileOrigin.X;
		var ry = tileOrigin.Y;

		var isLeft = false;
		var isRight = false;
		var isTop = false;
		var isBottom = false;

		if (rx <= bx && bx <= rx + 8 && directionAbs.X > directionAbs.Y) isLeft = true;
		else if (rx + 8 <= bx && bx <= rx + 16 && directionAbs.X > directionAbs.Y) isRight = true;
		else if (ry <= by && by <= ry + 8 && directionAbs.Y > directionAbs.X) isTop = true;
		else if (ry + 8 <= by && by <= ry + 16 && directionAbs.Y > directionAbs.X) isBottom = true;

		var x = isLeft || isRight
			? -Direction.X
			: Direction.X;

		var y = isTop || isBottom
			? -Direction.Y
			: Direction.Y;

		var newDirection = Direction with { X = x, Y = y };
		Direction = newDirection;
		var s = _bulletImpactScene.Instantiate<Node2D>();
		s.RotationDegrees = Mathf.RadToDeg(newDirection.Angle()) + 90;
		s.GlobalPosition = Position;
		GetTree().CurrentScene.AddChild(s);
	}

	private void PlayShootSound()
	{
		SetRandomPitch(_shootSound);
		_shootSound.Play();
	}

	private void SetRandomPitch(AudioStreamPlayer streamPlayer)
	{
		var pitch = _random.NextSingle() * 0.99 + 1;
		streamPlayer.PitchScale = (float)pitch;
	}
}
