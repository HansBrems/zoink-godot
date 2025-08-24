using Godot;

namespace Zoink.UI.Menus.PauseMenu;

public partial class PauseMenuScene : ColorRect
{
	private Button _exitGameButton;
	private Button _quitButton;

	private bool _isPaused;
	private bool IsPaused
	{
		get => _isPaused;
		set
		{
			_isPaused = value;
			GetTree().Paused = _isPaused;
			Visible = _isPaused;
		}
	}

	[Signal]
	public delegate void OnExitGameEventHandler();

	public override void _Ready()
	{
		_exitGameButton = GetNode<Button>("CenterContainer/VBoxContainer/ExitGameButton");
		_quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		_exitGameButton.Pressed += ExitGame;
		_quitButton.Pressed += () => GetTree().Quit();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Cancel"))
		{
			IsPaused = !IsPaused;
		}
	}

	private void ExitGame()
	{
		IsPaused = false;
		EmitSignal(SignalName.OnExitGame);
	}
}
