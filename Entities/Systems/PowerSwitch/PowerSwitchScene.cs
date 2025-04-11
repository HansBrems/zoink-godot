using Godot;
using Zoink.scenes.Environment;
using Zoink.Common.Interactions.InteractionArea;

public partial class PowerSwitchScene : StaticBody2D
{
	private InteractionAreaScene _interactionArea;

	[Export]
	public PowerScene Power { get; set; }

	public override void _Ready()
	{
		_interactionArea = GetNode<InteractionAreaScene>("InteractionArea");
		_interactionArea.Interact = Callable.From(Power.ToggleEnabled);
	}
}
