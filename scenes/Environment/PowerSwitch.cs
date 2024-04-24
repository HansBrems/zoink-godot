using Godot;
using Zoink.scenes.Core.Interactions;
using Zoink.scenes.Environment;

public partial class PowerSwitch : StaticBody2D
{
	private InteractionArea _interactionArea;

	[Export]
	public Power Power { get; set; }

	public override void _Ready()
	{
		_interactionArea = GetNode<InteractionArea>("InteractionArea");
		_interactionArea.Interact = Callable.From(Power.ToggleEnabled);
	}
}
