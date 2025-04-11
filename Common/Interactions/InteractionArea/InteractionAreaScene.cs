using Godot;

namespace Zoink.Common.Interactions.InteractionArea;

public partial class InteractionAreaScene : Area2D
{
	private Common.Interactions.InteractionManager.InteractionManagerScene _interactionManager;

	[Export]
	public string ActionName = "interact";

	public Callable Interact { get; set; } = Callable.From(() => { });

	public override void _Ready()
	{
		_interactionManager = GetNode<Common.Interactions.InteractionManager.InteractionManagerScene>("/root/InteractionManager");
		BodyEntered += _ => _interactionManager.RegisterArea(this);
		BodyExited += _ => _interactionManager.UnregisterArea(this);
	}
}
