using Godot;

namespace Zoink.scenes.Environment;

public partial class Power : Node2D
{
	private int _value = 500;

	[Export]
	public int Value
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
	public int MaxValue { get; set; } = 1000;

	[Export]
	public bool IsEnabled { get; set; } = false;

	[Signal]
	public delegate void OnChangeEventHandler(int value);

	[Signal]
	public delegate void OnDepletedEventHandler();

	public bool Drain(int amount)
	{
		if (!IsEnabled || Value < amount) return false;
		Value -= amount;
		return true;
	}

	public void ToggleEnabled()
	{
		IsEnabled = !IsEnabled;
	}

	public void TurnOn()
	{
		IsEnabled = true;
	}

	public void TurnOff()
	{
		IsEnabled = false;
	}
}
