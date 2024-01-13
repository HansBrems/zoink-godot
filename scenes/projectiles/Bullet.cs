using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class Bullet : Area2D
{
	private int _speed = 250;

	private PointLight2D _light;
	private Timer _selfDestructTimer;
	private Sprite2D _sprite;

	public Vector2 Direction { get; set; } = Vector2.Right;

	public override void _Ready()
	{
		_light = GetNode<PointLight2D>("Light");
		_selfDestructTimer = GetNode<Timer>("SelfDestructTimer");
		_sprite = GetNode<Sprite2D>("Sprite");

		BodyEntered += InitializeSelfDestruct;
		AreaEntered += InitializeSelfDestruct;
		_selfDestructTimer.Timeout += SelfDestruct;
	}

	private void OnAreaEntered(Area2D area)
	{
		InitializeSelfDestruct(area);
	}

	public override void _Process(double delta)
	{
		Position += Direction * (float)(_speed * delta);
	}

	private void InitializeSelfDestruct(Node2D node)
	{
		Direction = Vector2.Zero;
		_sprite.Visible = false;
		_selfDestructTimer.Start();
	}

	private void SelfDestruct()
	{
		QueueFree();
	}
}
