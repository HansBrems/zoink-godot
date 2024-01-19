using Godot;
using System;

public partial class ShootController : Node2D
{
	private PackedScene _bulletScene;

	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Bullet.tscn");
	}

	public void OnShoot(OnShootEventArgs args)
	{
		if (args.ProjectileType != ProjectileType.Bullet) return;

		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.Direction = args.Direction;
		bullet.Position = args.Position;
		bullet.RotationDegrees = (float)(args.Direction.Angle() * 180 / Math.PI);
		AddChild(bullet);
	}
}
