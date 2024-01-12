using System;
using System.Linq;
using Godot;

public partial class Dungeon01 : Node2D
{
	private PackedScene _bulletScene;
	private PackedScene _beetleScene;
	private readonly Random _random = new Random();

	private Player _player;
	private Label _bitcoinLabel;
	private Marker2D[] _spawnLocations;
	private TileMap _tileMap;

	public override void _Ready()
	{
		_bitcoinLabel = GetNode<Label>("CanvasLayer/Label");
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Bullet.tscn");
		_beetleScene = ResourceLoader.Load<PackedScene>("res://scenes/enemies/Beetle.tscn");
		_player = GetNode<Player>("Player");
		_spawnLocations = GetNode<Node2D>("World/SpawnLocations").GetChildren().Cast<Marker2D>().ToArray();
		_tileMap = GetNode<TileMap>("World/TileMap");

		GetNode<Timer>("SpawnEnemyTimer").Timeout += SpawnEnemy;
		_player.OnBitcoinsReceived += UpdateBitcoinLabel;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			var mouseLocation = GetGlobalMousePosition();
			var location = _tileMap.LocalToMap(ToLocal(mouseLocation));
			_tileMap.SetCell(0, location, 2, new Vector2I(3, 0));
		}
	}

	private void SpawnEnemy()
	{
		var beetle = _beetleScene.Instantiate<Beetle>();
		var spawnIndex = _random.Next(0, _spawnLocations.Length);
		var spawnLocation = _spawnLocations[spawnIndex];
		beetle.Position = spawnLocation.Position;
		AddChild(beetle);
		beetle.Target = _player;
	}

	private void OnPlayerShoot(Vector2 direction)
	{
		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.Position = _player.Position;
		bullet.Direction = direction;
		bullet.RotationDegrees = (float)(direction.Angle() * 180 / Math.PI);
		AddChild(bullet);
	}

	private void UpdateBitcoinLabel(int bitcoins)
	{
		var word = bitcoins == 1 ? "bitcoin" : "bitcoins";
		_bitcoinLabel.Text = $"{bitcoins} {word}";
	}
}

