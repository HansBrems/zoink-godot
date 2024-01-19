using Godot;

public partial class Tower : Node2D
{
	private Node2D _enemy;

	private Line2D _laser;
	private RayCast2D _rayCast;
	private Timer _shootTimer;

	[Export]
	public double AttackSpeed { get; set; } = 0.5;

	[Export]
	public Node2D EnemiesNode { get; set; }

	[Signal]
	public delegate void OnShootEventHandler(OnShootEventArgs args);

	public override void _Ready()
	{
		_laser = GetNode<Line2D>("Laser");
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_shootTimer = GetNode<Timer>("ShootTimer");
		_shootTimer.WaitTime = AttackSpeed;
		_shootTimer.Timeout += Shoot;
	}

	public override void _Process(double delta)
	{
		if (_rayCast == null || _rayCast.IsColliding() || _enemy == null)
		{
			_laser.Visible = false;
		}
		else
		{
			_laser.Visible = true;
			_laser.Points = new[] { ToLocal(Position), ToLocal(_enemy.Position) };
		}
	}

	private Node2D FindClosestEnemy()
	{
		Node2D closestNode = null;
		float minDistance = float.MaxValue;

		foreach (Node2D node in EnemiesNode.GetChildren())
		{
			float distance = Position.DistanceTo(node.Position);
			if (distance < minDistance)
			{
				minDistance = distance;
				closestNode = node;
			}
		}

		return closestNode;
	}

	private void Shoot()
	{
		_enemy = FindClosestEnemy();
		if (_enemy == null) return;

		_rayCast.TargetPosition = _enemy.GlobalPosition - GlobalPosition;
		if (_rayCast.IsColliding()) return;

		var direction = _enemy.Position - Position;
		EmitSignal("OnShoot", new OnShootEventArgs
		{
			Direction = direction.Normalized(),
			Position = Position,
			ProjectileType = ProjectileType.Bullet
		});
	}
}
