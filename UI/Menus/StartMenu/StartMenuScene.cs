using Godot;

namespace Zoink.UI.Menus.StartMenu;

public partial class StartMenuScene : ColorRect
{
	private Button _startButton;
	private Button _quitButton;

	public override void _Ready()
	{
		_startButton = GetNode<Button>("CenterContainer/VBoxContainer/StartButton");
		_quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		_startButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
		_quitButton.Pressed += () => GetTree().Quit();
	}
}
