using Godot;
using Vector2 = Godot.Vector2;

public partial class Bullet : Area2D
{
	private int _speed = 250;
	public Vector2 Direction { get; set; } = Vector2.Right;

	private GpuParticles2D _splash;
	private Sprite2D _sprite;
	private PointLight2D _light;

	public override void _Ready()
	{
		_splash = GetNode<GpuParticles2D>("Splash");
		_light = GetNode<PointLight2D>("Light");
		_sprite = GetNode<Sprite2D>("Sprite");
		
		BodyEntered += Splash;
	}

	public override void _Process(double delta)
	{
		Position += Direction * (float)(_speed * delta);
	}

	private void Splash(Node2D body)
	{
		//if (body.HasMethod("Hit"))
		//    body.

		_speed = -100;
		_sprite.Visible = false;
		_splash.Emitting = true;
	}

	private void SelfDestruct()
	{
		QueueFree();
	}
}
