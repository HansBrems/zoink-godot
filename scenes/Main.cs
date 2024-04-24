using Godot;

namespace Zoink.scenes;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		GetTree().ChangeSceneToFile("res://scenes/Maps/Ship/Ship.tscn");
	}
}
