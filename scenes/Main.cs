using System.Linq;
using Godot;

public partial class Main : Node2D
{
	private Line2D _square;
	private ShootController _shootController;
	private TileMap _tileMap;

	private PackedScene _towerScene;
	private Node2D _towers;

	public override void _Ready()
	{
		_square = GetNode<Line2D>("Square");
		_tileMap = GetNode<TileMap>("Map01/TileMap");

		_towerScene = ResourceLoader.Load<PackedScene>("res://scenes/objects/tower/Tower.tscn");
		_towers = GetNode<Node2D>("Towers");

		_shootController = GetNode<ShootController>("ShootController");
		GetNode<Player>("Player").OnShoot += _shootController.OnShoot;
		GetNode("Towers")
			.GetChildren()
		 	.Cast<Tower>()
		 	.ToList()
		 	.ForEach(tower => tower.OnShoot += _shootController.OnShoot);
	}

	public override void _Process(double delta)
	{
		var mousePosition = GetGlobalMousePosition();
		var localPosition = _tileMap.LocalToMap(mousePosition);

		var width = 16;
		_square.Points = new Vector2[]
		{
			new Vector2(localPosition.X * width, localPosition.Y * width),
			new Vector2(localPosition.X * width + width, localPosition.Y * width),
			new Vector2(localPosition.X * width + width, localPosition.Y * width + width),
			new Vector2(localPosition.X * width, localPosition.Y * width + width),
			new Vector2(localPosition.X * width, localPosition.Y * width),
		};

		if (Input.IsActionJustPressed("interact"))
		{
			var tower = _towerScene.Instantiate<Tower>();
			tower.Position = new Vector2(localPosition.X * width + width / 2, localPosition.Y * width + width / 2);
			_towers.AddChild(tower);
			tower.SetAttackSpeed(0.2);
			tower.EnemiesNode = GetNode<Node2D>("Enemies");
			tower.OnShoot += _shootController.OnShoot;
		}
	}
}
