using Godot;

namespace Zoink.scenes.Objects.Console;

public partial class Console : StaticBody2D
{
	private AnimationPlayer _animationPlayer;
	private Components.Interaction.InteractionArea.InteractionArea _interactionArea;
	private PointLight2D _light;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_interactionArea = GetNode<Components.Interaction.InteractionArea.InteractionArea>("InteractionArea");
		_interactionArea.Interact = Callable.From(ToggleEnabled);
		_light = GetNode<PointLight2D>("PointLight2D");
	}

	private void ToggleEnabled()
	{
		_light.Enabled = !_light.Enabled;

		if (_light.Enabled) _animationPlayer.Play("On");
		else _animationPlayer.Play("Off");
	}
}
