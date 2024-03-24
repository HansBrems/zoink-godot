using System;
using Godot;

namespace Zoink.scenes.Components.ShootController;

public partial class ShootController : Node2D
{
	private PackedScene _bulletScene;

	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>(scripts.SceneUris.Get("Projectiles", "Bullet"));
	}

	public void OnShoot(OnShootEventArgs args)
	{
		if (args.ProjectileType != ProjectileType.Bullet) return;

		var bullet = _bulletScene.Instantiate<Projectiles.Bullet.Bullet>();
		bullet.Direction = args.Direction;
		bullet.Position = args.Position;
		bullet.RotationDegrees = (float)(args.Direction.Angle() * 180 / Math.PI);
		AddChild(bullet);
	}
}
