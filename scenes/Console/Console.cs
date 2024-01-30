using Godot;

public partial class Console : StaticBody2D
{
	private InteractionArea _interactionArea;
	private PointLight2D _light;

	public override void _Ready()
	{
		_interactionArea = GetNode<InteractionArea>("InteractionArea");
		_interactionArea.Interact = Callable.From(ToggleEnabled);
		_light = GetNode<PointLight2D>("PointLight2D");
	}

	private void ToggleEnabled()
	{
		_light.Visible = !_light.Visible;
	}
}
