using System;
using Godot;
using Zoink.scripts;

namespace Zoink.scenes.Core.Projectiles;

public partial class ProjectileManager : Node2D
{
	private PackedScene _bulletScene;

	public override void _Ready()
	{
		_bulletScene = PackedSceneLoader.Load("Entities", "Projectiles", "Bullet");
	}

	public void OnShoot(OnShootEventArgs args)
	{
		if (args.ProjectileType != ProjectileType.Bullet) return;

		var bullet = _bulletScene.Instantiate<scenes.Projectiles.Bullet.Bullet>();
		bullet.Direction = args.Direction;
		bullet.Position = args.Position;
		bullet.RotationDegrees = (float)(args.Direction.Angle() * 180 / Math.PI);
		AddChild(bullet);
	}
}
