using Godot;

namespace Zoink.scenes.Environment;

public partial class Power : Node2D
{
	private Timer _decayTimer;

	[Export]
	public float CurrentValue { get; set; } = 500;

	[Export]
	public float MaxValue { get; set; } = 1000;

	[Export]
	public float DecayRate { get; set; } = 0f;

	[Export]
	public double DecayMilliseconds { get; set; } = 1000;

	[Signal]
	public delegate void OnChangeEventHandler(float currentValue);

	[Signal]
	public delegate void OnDepletedEventHandler();

	public override void _Ready()
	{
		_decayTimer = GetNode<Timer>("DecayTimer");
		_decayTimer.WaitTime = DecayMilliseconds / 1000;
		_decayTimer.Timeout += Decay;
		_decayTimer.Start();
	}

	private void Decay()
	{
		CurrentValue = Mathf.Clamp(CurrentValue - DecayRate, 0, MaxValue);
		EmitSignal(SignalName.OnChange, CurrentValue);
		if (CurrentValue <= 0) EmitSignal(SignalName.OnDepleted);
	}
}
