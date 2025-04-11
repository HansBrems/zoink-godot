using Godot;
using Zoink.Common.Interactions.InteractionArea;
using Zoink.Entities.Systems.Power;

namespace Zoink.Entities.Systems.Console;

public partial class ConsoleScene : StaticBody2D
{
	private AnimationPlayer _animationPlayer;
	private InteractionAreaScene _interactionArea;
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
	public PowerScene PowerSource { get; set; }

	[Signal]
	public delegate void OnStateChangedEventHandler(bool enabled);

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_interactionArea = GetNode<InteractionAreaScene>("InteractionArea");
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
