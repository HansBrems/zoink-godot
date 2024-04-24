using Godot;

namespace Zoink.scenes.Core;

public partial class PlayerCam : Camera2D
{
	private Timer _shakeTimer;

	private RandomNumberGenerator _random = new RandomNumberGenerator();
	private bool _isShaking = false;

	public override void _Ready()
	{
		_shakeTimer = GetNode<Timer>("ShakeTimer");
		_shakeTimer.Timeout += () => _isShaking = false;
	}

	public override void _Process(double delta)
	{
		if (!_isShaking) return;
		Offset = new Vector2(_random.Randf() * -4, _random.Randf() * 4);
	}

	public void Shake()
	{
		_isShaking = true;
		_shakeTimer.Start();
	}
}
