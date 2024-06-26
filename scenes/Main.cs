using Godot;

namespace Zoink.scenes;

public partial class Main : Node2D
{
	private PauseMenu _pauseMenu;

	public override void _Ready()
	{
		_pauseMenu = GetNode<PauseMenu>("CanvasLayer/PauseMenu");
		_pauseMenu.OnExitGame += () => GetTree().ChangeSceneToFile("res://scenes/Menus/StartMenu.tscn");

		// var new_scene = load("res://scenes/Maps/Ship/Ship.tscn").instance()
		// var old_scene = get_node("OldChildScenePath")
		// get_node("ParentNodePath").replace_child(old_scene, new_scene)
		// old_scene.queue_free()  # Optionally, remove the old scene from mem
	}
}
