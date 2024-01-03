using System;
using System.Linq;
using Godot;

public partial class Dungeon01 : Node2D
{
	private PackedScene _bulletScene;
	private PackedScene _beetleScene;

	private Player _player;
	private Random _random = new Random();

	private Marker2D[] _spawnLocations = Array.Empty<Marker2D>();

	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Bullet.tscn");
		_beetleScene = ResourceLoader.Load<PackedScene>("res://scenes/enemies/Beetle.tscn");
		_player = GetNode<Player>("Player");

		_spawnLocations = GetNode<Node2D>("SpawnLocations").GetChildren().Cast<Marker2D>().ToArray();
	}

	private void SpawnEnemy()
	{
		var beetle = _beetleScene.Instantiate<Beetle>();
		var spawnIndex = _random.Next(0, _spawnLocations.Length - 1);
		var spawnLocation = _spawnLocations[spawnIndex];
		GD.Print("Location: " + spawnIndex + " of locations " + _spawnLocations.Length);
		beetle.Player = _player;
		beetle.Position = spawnLocation.Position;
		AddChild(beetle);
	}

	private void OnPlayerShoot(Vector2 direction)
	{
		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.Position = _player.Position;
		bullet.Direction = direction;
		AddChild(bullet);
	}
}
