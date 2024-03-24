using Godot;

namespace Zoink.scenes.Components.Interaction.InteractionArea;

public partial class InteractionArea : Area2D
{
	private InteractionManager.InteractionManager _interactionManager;

	[Export]
	public string ActionName = "interact";

	public Callable Interact { get; set; } = Callable.From(() => { });

	public override void _Ready()
	{
		_interactionManager = GetNode<InteractionManager.InteractionManager>("/root/InteractionManager");
		BodyEntered += _ => _interactionManager.RegisterArea(this);
		BodyExited += _ => _interactionManager.UnregisterArea(this);
	}
}
