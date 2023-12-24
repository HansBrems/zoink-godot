using Godot;
using System;

public partial class Candle : Node2D
{
	private PointLight2D _light;
	private bool _lightAnimating;
	private readonly Random _random = new Random();

	[Export]
	public Color LightColor { get; set; } = new Color(1, 1, 1, 1);

	public override void _Ready()
	{
		_light = GetNode<PointLight2D>("Light");
		_light.Color = LightColor;
	}

	public override void _Process(double delta)
	{
		Flicker();
	}

	private void Flicker()
	{
		if (!_lightAnimating)
		{
			// Calculate a new energy for the light.
			var newEnergy = _random.NextSingle() * 0.4 + 1.2;

			// Calculate how long the tween should last.
			var animationDuration = _random.NextSingle() * 0.1 + 0.3;

			// Create the tween.
			var tween = GetTree().CreateTween();
			tween.TweenProperty(_light, "energy", newEnergy, animationDuration);
			_lightAnimating = true;

			// Make it so the tween sets _lightAnimating to false when it's done animating.
			tween.TweenCallback(Callable.From(() => _lightAnimating = false));
		}
	}
}
