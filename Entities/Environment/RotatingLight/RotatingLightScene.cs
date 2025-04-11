using Godot;

namespace Zoink.Entities.Environment.RotatingLight;

public partial class RotatingLightScene : Node2D
{
	[Export]
	public Color LightColor { get; set; } = new Color(1, 1, 1, 1);

	[Export]
	public float RotationSpeed { get; set; } = 100;

	public override void _Ready()
	{
		GetNode<PointLight2D>("PointLight2D").Color = LightColor;
	}

	public override void _Process(double delta)
	{
		RotationDegrees += (float)(RotationSpeed * delta);
	}
}
