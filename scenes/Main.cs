using Godot;
using Zoink.scenes.Core.Projectiles;
using Zoink.scenes.Objects.Console;
using Zoink.scenes.Systems;

namespace Zoink.scenes;

public partial class Main : Node2D
{
	private const int TileWidth = 16;

	private Player.Player _player;
	private ProjectileManager _shootController;
	private Line2D _selector;
	private TileMap _tileMap;
	private PackedScene _turretScene;
	private Node2D _turrets;
	private EnvironmentManager _environmentManager;

	private Console _oxygenConsole;
	private Label _oxygenValue;

	private Console _powerConsole;
	private Label _powerValue;

	private bool _showSelector;

	public override void _Ready()
	{
		_player = GetNode<Player.Player>("Player");
		_shootController = GetNode<ProjectileManager>("ProjectileManager");
		_selector = GetNode<Line2D>("Selector");
		_tileMap = GetNode<TileMap>("Map01/TileMap");
		_turretScene = ResourceLoader.Load<PackedScene>(scripts.SceneUris.Get("Objects", "Turret"));
		_turrets = GetNode<Node2D>("Turrets");
		_oxygenValue = GetNode<Label>("HUD/GridContainer/OxygenValue");
		_powerValue = GetNode<Label>("HUD/GridContainer/PowerValue");
		_environmentManager = GetNode<EnvironmentManager>("Systems/EnvironmentManager");
		_environmentManager.OnOxygenChanged += (oxygen) => _oxygenValue.Text = $"{oxygen}%";
		_environmentManager.OnPowerChanged += (power) => _powerValue.Text = $"{power} %";

		_oxygenConsole = GetNode<Console>("Systems/OxygenConsole");
		_oxygenConsole.OnStateChanged += (enabled) => _environmentManager.IsOxygenOn = enabled;

		_powerConsole = GetNode<Console>("Systems/PowerConsole");
		_powerConsole.OnStateChanged += (enabled) => _environmentManager.IsPowerOn = enabled;

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
		var turret = _turretScene.Instantiate<Objects.Turret.Turret>();
		turret.Position = new Vector2(mouseMapPosition.X * TileWidth + TileWidth / 2, mouseMapPosition.Y * TileWidth + TileWidth / 2);
		_turrets.AddChild(turret);
		turret.EnemiesNode = GetNode<Node2D>("Enemies");
		turret.OnShoot += _shootController.OnShoot;
	}
}
