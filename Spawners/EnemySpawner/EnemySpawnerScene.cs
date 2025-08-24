using System;
using Godot;

namespace Zoink.Spawners.EnemySpawner;

public partial class EnemySpawnerScene : Node2D
{
	private Timer _spawnTimer;

	[Export] public Node Container { get; set; }
	[Export] public PackedScene EnemyScene { get; set; }
	[Export] public double SpawnRate { get; set; } = 2;

	public override void _Ready()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnTimer.WaitTime = SpawnRate;
		_spawnTimer.Timeout += SpawnEnemy;
	}

	private void SpawnEnemy()
	{
		if (Container == null)
			throw new ArgumentException("Missing container for enemy spawner");

		if (EnemyScene == null)
			throw new ArgumentException("Missing enemy scene");

		if (EnemyScene.Instantiate() is not Node2D enemy)
			throw new Exception("Failed to instantiate enemy");

		enemy.Position = Position;

		Container.AddChild(enemy);
	}
}
