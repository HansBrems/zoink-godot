using Godot;

public partial class InteractionArea : Area2D
{
	private InteractionManager _interactionManager;

	[Export]
	public string ActionName = "interact";

	public Callable Interact { get; set; } = Callable.From(() => { });

	public override void _Ready()
	{
		_interactionManager = GetNode<InteractionManager>("/root/InteractionManager");
		BodyEntered += _ => _interactionManager.RegisterArea(this);
		BodyExited += _ => _interactionManager.UnregisterArea(this);
	}
}
