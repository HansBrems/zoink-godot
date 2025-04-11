using Godot;

namespace Zoink.Entities.Systems.Power;

public partial class PowerScene : Node2D
{
	private bool _isEnabled = false;
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
	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			_isEnabled = value;
			EmitSignal(SignalName.OnStatusChanged);
		}
	}

	[Signal]
	public delegate void OnChangeEventHandler(int value);

	[Signal]
	public delegate void OnStatusChangedEventHandler();

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
