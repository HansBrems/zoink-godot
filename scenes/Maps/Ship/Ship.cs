using Godot;
using Zoink.scenes.Core.Projectiles;
using Zoink.scenes.Environment;
using Zoink.scenes.Objects.Turret;
using Zoink.scripts;

namespace Zoink.scenes.Maps.Ship;

public partial class Ship : Node2D
{
	private EnvironmentManager _environmentManager;
	private Area2D _doorToOutside;
	private Player.Player _player;
	private Camera2D _camera;
	private TileMap _tileMap;
	private ProjectileManager _shootController;
	private PackedScene _turretScene;
	private Node _turrets;

	// UI
	private Node2D _placementIndicator;

	private bool _showPlacementIndicator;

	public override void _Ready()
	{
		_environmentManager = GetNode<EnvironmentManager>("EnvironmentManager");
		_doorToOutside = GetNode<Area2D>("DoorToOutside");
		_player = GetNode<Player.Player>("Player");
		_camera = GetNode<Camera2D>("Player/PlayerCam");
		_tileMap = GetNode<TileMap>("TileMap");
		_shootController = GetNode<ProjectileManager>("ProjectileManager");
		_turretScene = PackedSceneLoader.Load("Objects", "Turret");
		_turrets = GetNode<Node>("Turrets");

		// UI
		_placementIndicator = GetNode<Node2D>("HUD/PlacementIndicator");

		// Events
		_doorToOutside.BodyEntered += GoOutside;
		_player.OnBuildingStarted += () => _showPlacementIndicator = true;
		_player.OnBuildingCancelled += () => _showPlacementIndicator = false;
		_player.OnBuildingConfirmed += () =>_showPlacementIndicator = false;
		_player.OnBuildingFinished += PlaceTurret;;
		_player.OnShoot += _shootController.OnShoot;
	}

	public override void _Process(double delta)
	{
		if (_showPlacementIndicator) ShowPlacementIndicator();
		else HidePlacementIndicator();
	}

	private void GoOutside(Node2D node)
	{
		if (node is Player.Player)
		{
			GetTree().ChangeSceneToFile("res://scenes/Maps/Outside.tscn");
		}
	}

	private void ShowPlacementIndicator()
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

	private void HidePlacementIndicator()
	{
		_placementIndicator.Visible = false;
	}

	private void PlaceTurret(Vector2 buildingPosition)
	{
		const int tileWidth = 16;
		var mouseMapPosition = _tileMap.LocalToMap(buildingPosition);
		var turret = _turretScene.Instantiate<Turret>();
		turret.Position = new Vector2(mouseMapPosition.X * tileWidth + tileWidth / 2, mouseMapPosition.Y * tileWidth + tileWidth / 2);
		_turrets.AddChild(turret);
		turret.EnemiesNode = GetNode<Node>("Enemies");
		turret.OnShoot += _shootController.OnShoot;
	}
}