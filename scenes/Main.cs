using System.Linq;
using Godot;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		var shootController = GetNode<ShootController>("ShootController");

		GetNode<Player>("Player").OnShoot += shootController.OnShoot;
		GetNode("Towers")
			.GetChildren()
		 	.Cast<Tower>()
		 	.ToList()
		 	.ForEach(tower => tower.OnShoot += shootController.OnShoot);
	}
}
