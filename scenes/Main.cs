using Godot;
using Zoink.scenes.Core.Projectiles;
using Zoink.scenes.Objects.Console;
using Zoink.scenes.Systems;

namespace Zoink.scenes;

public partial class Main : Node2D
{
	private const int TileWidth = 16;

	private Camera2D _camera;
	private Player.Player _player;
	private ProjectileManager _shootController;
	private Line2D _selector;
	private TileMap _tileMap;
	private PackedScene _turretScene;
	private Node2D _turrets;
	private EnvironmentManager _environmentManager;

	private Node2D _placementIndicator;
	private bool _showPlacementIndicator;

	private Console _oxygenConsole;
	private Label _oxygenValue;

	private Console _powerConsole;
	private Label _powerValue;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Player/Camera2D");
		_player = GetNode<Player.Player>("Player");
		_shootController = GetNode<ProjectileManager>("ProjectileManager");
		_tileMap = GetNode<TileMap>("Ship/TileMap");
		_turretScene = ResourceLoader.Load<PackedScene>(scripts.SceneUris.Get("Objects", "Turret"));
		_turrets = GetNode<Node2D>("Turrets");
		_oxygenValue = GetNode<Label>("HUD/GridContainer/OxygenValue");
		_powerValue = GetNode<Label>("HUD/GridContainer/PowerValue");
		_environmentManager = GetNode<EnvironmentManager>("Systems/EnvironmentManager");
		_environmentManager.OnOxygenChanged += (oxygen) => _oxygenValue.Text = $"{oxygen}%";
		_environmentManager.OnPowerChanged += (power) => _powerValue.Text = $"{power} %";

		_placementIndicator = GetNode<Node2D>("HUD/PlacementIndicator");

		_oxygenConsole = GetNode<Console>("Systems/OxygenConsole");
		_oxygenConsole.OnStateChanged += (enabled) => _environmentManager.IsOxygenOn = enabled;

		_powerConsole = GetNode<Console>("Systems/PowerConsole");
		_powerConsole.OnStateChanged += (enabled) => _environmentManager.IsPowerOn = enabled;

		_player.OnBuildingStarted += () => _showPlacementIndicator = true;
		_player.OnBuildingCancelled += () => _showPlacementIndicator = false;
		_player.OnBuildingConfirmed += () =>_showPlacementIndicator = false;
		_player.OnBuildingFinished += PlaceTurret;;
		_player.OnShoot += _shootController.OnShoot;
	}

	public override void _Process(double delta)
	{
		if (_showPlacementIndicator) DrawSelector();
		else HideSelector();
	}

	private void DrawSelector()
	{
		// This assumes the viewport size is 320x180.
		// Todo (Hans): Make this dynamic.
		var cameraOriginX = _camera.GlobalPosition.X - 160F;
		var cameraOriginY = _camera.GlobalPosition.Y - 90F;
		var mouseMapPosition = _tileMap.LocalToMap(GetGlobalMousePosition());
		var x = (mouseMapPosition.X * 16) - (cameraOriginX);
		var y = (mouseMapPosition.Y * 16) - (cameraOriginY);
		_placementIndicator.GlobalPosition = new Vector2(x, y);
		_placementIndicator.Visible = true;
	}

	private void HideSelector()
	{
		_placementIndicator.Visible = false;
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
