using System;
using Godot;
using Zoink.scenes.Core.Projectiles;

namespace Zoink.scenes.Objects.Turret;

public partial class Turret : StaticBody2D
{
	private Enemies.Beetle.Beetle _enemy;
	private Line2D _laser;
	private Marker2D _laserOrigin;
	private Random _random = new();
	private RayCast2D _rayCast;
	private Timer _shootTimer;
	private Sprite2D _turret;

	[Export]
	public double AttackSpeed { get; set; } = 0.5;

	[Export]
	public Node EnemiesNode { get; set; }

	[Signal]
	public delegate void OnShootEventHandler(OnShootEventArgs args);

	public override void _Ready()
	{
		_laser = GetNode<Line2D>("Laser");
		_laserOrigin = GetNode<Marker2D>("Turret/LaserOrigin");
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_shootTimer = GetNode<Timer>("ShootTimer");
		var variance = _random.NextDouble() * 0.4 - 0.2;
		_shootTimer.WaitTime = AttackSpeed + variance;
		_shootTimer.Timeout += Shoot;
		_turret = GetNode<Sprite2D>("Turret");
	}

	public override void _Process(double delta)
	{
		if (_rayCast == null || _rayCast.IsColliding() || _enemy == null)
		{
			_laser.Visible = false;
		}
		else
		{
			var direction = _enemy.Position - Position;
			_turret.RotationDegrees = (float)(direction.Angle() * 180 / Math.PI) + 90;
			_laser.Visible = true;
			_laser.Points = new[] { ToLocal(_laserOrigin.GlobalPosition), ToLocal(_enemy.Position) };
		}
	}

	public void SetAttackSpeed(double attackSpeed)
	{
		AttackSpeed = attackSpeed;
		_shootTimer.WaitTime = AttackSpeed;
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
		_enemy = FindClosestEnemy() as Enemies.Beetle.Beetle;
		if (_enemy == null) return;
		_enemy.OnKilled += () => _enemy = null;

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
