using System.Linq;
using Godot;

public partial class Main : Node2D
{
	private const int TileWidth = 16;

	private Player _player;
	private ShootController _shootController;
	private Line2D _selector;
	private TileMap _tileMap;
	private PackedScene _towerScene;
	private Node2D _towers;

	private bool _showSelector;

	public override void _Ready()
	{
		_player = GetNode<Player>("Player");
		_shootController = GetNode<ShootController>("ShootController");
		_selector = GetNode<Line2D>("Square");
		_tileMap = GetNode<TileMap>("Map01/TileMap");
		_towerScene = ResourceLoader.Load<PackedScene>("res://scenes/objects/tower/Tower.tscn");
		_towers = GetNode<Node2D>("Towers");

		_player.OnBuildingStarted += () => _showSelector = true;
		_player.OnBuildingCancelled += () => _showSelector = false;
		_player.OnBuildingConfirmed += () =>_showSelector = false;
		_player.OnBuildingFinished += PlaceTurret;;
		_player.OnShoot += _shootController.OnShoot;
	}

	public override void _Process(double delta)
	{
		if (_showSelector)
		{
			DrawSelector();
		}
		else
		{
			HideSelector();
		}
	}

	private void DrawSelector()
	{
		var mouseMapPosition = _tileMap.LocalToMap(GetGlobalMousePosition());

		_selector.Points = new []
		{
			new Vector2(mouseMapPosition.X * TileWidth, mouseMapPosition.Y * TileWidth),
			new Vector2(mouseMapPosition.X * TileWidth + TileWidth, mouseMapPosition.Y * TileWidth),
			new Vector2(mouseMapPosition.X * TileWidth + TileWidth, mouseMapPosition.Y * TileWidth + TileWidth),
			new Vector2(mouseMapPosition.X * TileWidth, mouseMapPosition.Y * TileWidth + TileWidth),
			new Vector2(mouseMapPosition.X * TileWidth, mouseMapPosition.Y * TileWidth),
		};

		_selector.Visible = true;
	}

	private void HideSelector()
	{
		_selector.Visible = false;
	}

	private void PlaceTurret(Vector2 buildingPosition)
	{
		var mouseMapPosition = _tileMap.LocalToMap(buildingPosition);
		var tower = _towerScene.Instantiate<Tower>();
		tower.Position = new Vector2(mouseMapPosition.X * TileWidth + TileWidth / 2, mouseMapPosition.Y * TileWidth + TileWidth / 2);
		_towers.AddChild(tower);
		tower.EnemiesNode = GetNode<Node2D>("Enemies");
		tower.OnShoot += _shootController.OnShoot;
	}
}
