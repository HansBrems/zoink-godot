using Godot;

public partial class Bullet : Area2D
{
	public Vector2 Direction { get; set; } = Vector2.Right;

	[Export]
	public int Speed { get; set; } = 200;

	public override void _Ready()
	{
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
}
