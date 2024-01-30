using Godot;

public partial class EnemySpawner : Node2D
{
	private PackedScene _beetleScene;
	private Timer _spawnTimer;

	[Export]
	public Node2D ChaseTarget { get; set; }

	[Export]
	public double SpawnRate { get; set; } = 2;

	public override void _Ready()
	{
		_beetleScene = ResourceLoader.Load<PackedScene>(SceneUris.Get("Enemies", "Beetle"));
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnTimer.WaitTime = SpawnRate;
		_spawnTimer.Timeout += SpawnEnemy;
	}

	private void SpawnEnemy()
	{
		var beetle = _beetleScene.Instantiate<Beetle>();
		beetle.Position = Position;
		GetTree().CurrentScene.GetNode<Node2D>("Enemies").AddChild(beetle);
		beetle.Target = ChaseTarget;
	}
}