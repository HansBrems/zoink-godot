using Godot;

public partial class Bitcoin : Area2D
{
	private bool _isPlayerNear = false;

	[Export]
	public Player Player { get; set; }

	[Signal]
	public delegate void OnBitcoinCollectedEventHandler();

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_isPlayerNear && Input.IsActionJustPressed("interact"))
		{
			Player?.ReceiveBitcoin();
			QueueFree();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player) _isPlayerNear = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player) _isPlayerNear = false;
	}
}
