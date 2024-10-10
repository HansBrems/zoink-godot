using Godot;
using Zoink.scenes.Core.Interactions;
using Zoink.scenes.Environment;

namespace Zoink.scenes.Objects.Console;

public partial class Console : StaticBody2D
{
	private AnimationPlayer _animationPlayer;
	private InteractionArea _interactionArea;
	private PointLight2D _light;

	private bool _enabled;
	[Export]
	public bool Enabled
	{
		get => _enabled;
		set
		{
			_enabled = value;
			EmitSignal(SignalName.OnStateChanged, _enabled);
		}
	}

	[Export]
	public Power PowerSource { get; set; }

	[Signal]
	public delegate void OnStateChangedEventHandler(bool enabled);

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_interactionArea = GetNode<InteractionArea>("InteractionArea");
		_interactionArea.Interact = Callable.From(ToggleEnabled);
		_light = GetNode<PointLight2D>("PointLight2D");

		UpdateVisuals();
		PowerSource.OnStatusChanged += UpdateVisuals;
	}

	private void ToggleEnabled()
	{
		Enabled = !Enabled;
		UpdateVisuals();
	}

	private void UpdateVisuals()
	{
		var on = PowerSource != null && PowerSource.IsEnabled && Enabled;
		_animationPlayer.Play(on ? "On" : "Off");
		_light.Enabled = on;
	}
}
