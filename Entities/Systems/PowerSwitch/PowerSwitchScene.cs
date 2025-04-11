using Godot;
using Zoink.Common.Interactions.InteractionArea;
using Zoink.Entities.Systems.Power;

namespace Zoink.Entities.Systems.PowerSwitch;

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
