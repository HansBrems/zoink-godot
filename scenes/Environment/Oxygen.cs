using Godot;
using System;

public partial class Oxygen : Node2D
{
	private Timer _timer;

	private float _value;

	[Export]
	public float DecayAmount { get; set; } = 1f;

	[Export]
	public float Value
	{
		get => _value;
		set
		{
			_value = Mathf.Clamp(value, 0, MaxValue);
			EmitSignal(SignalName.OnChange, _value);
			if (_value == 0) EmitSignal(SignalName.OnDepleted);
		}
	}

	[Export]
	public float MaxValue { get; set; } = 1000;

	[Signal]
	public delegate void OnChangeEventHandler(float value);

	[Signal]
	public delegate void OnDepletedEventHandler();

	public override void _Ready()
	{
		_timer = GetNode<Timer>("DecayTimer");
		_timer.Timeout += () => Drain(DecayAmount);
	}

	public void Drain(float amount)
	{
		Value -= amount;
	}

	public void Replenish(float amount)
	{
		Value += amount;
	}
}
