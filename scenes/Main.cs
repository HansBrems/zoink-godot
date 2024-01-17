using Godot;

public partial class Main : Node2D
{
	// Todo: temporary reference to beetle until enemy spawner exists
	private Beetle _beetle;
	private Player _player;
	private ShootController _shootController;
	
	public override void _Ready()
	{
		_beetle = GetNode<Beetle>("Beetle");
		_player = GetNode<Player>("Player");
		_shootController = GetNode<ShootController>("ShootController");

		_beetle.OnShoot += _shootController.OnShoot;
		_player.OnShoot += _shootController.OnShoot;
	}
}
