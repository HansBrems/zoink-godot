using Godot;
using Zoink.scenes.Core.Interactions;

namespace Zoink.scenes.Objects.Console;

public partial class Console : StaticBody2D
{
	private AnimationPlayer _animationPlayer;
	private InteractionArea _interactionArea;
	private PointLight2D _light;

	[Export] public bool Enabled { get; set; }
	[Signal] public delegate void OnStateChangedEventHandler(bool enabled);

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_interactionArea = GetNode<InteractionArea>("InteractionArea");
		_interactionArea.Interact = Callable.From(ToggleEnabled);
		_light = GetNode<PointLight2D>("PointLight2D");
		UpdateVisuals();
	}

	private void ToggleEnabled()
	{
		Enabled = !Enabled;
		UpdateVisuals();
		EmitSignal("OnStateChanged", Enabled);
	}

	private void UpdateVisuals()
	{
		_animationPlayer.Play(Enabled ? "On" : "Off");
		_light.Enabled = Enabled;
	}
}
