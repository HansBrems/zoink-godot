using Godot;

namespace Zoink.Entities.Systems.Oxygen;

public partial class OxygenScene : Node2D
{
	private Timer _timer;

	private int _value;

	[Export]
	public int DecayAmount { get; set; } = 1;

	[Export]
	public int Value
	{
		get => _value;
		set
		{
			_value = Mathf.Clamp(value, 0, MaxValue);
			EmitSignal(SignalName.OnChange, _value);
		}
	}

	[Export]
	public int MaxValue { get; set; } = 1000;

	[Signal]
	public delegate void OnChangeEventHandler(int value);

	public override void _Ready()
	{
		_timer = GetNode<Timer>("DecayTimer");
		_timer.Timeout += () => Drain(DecayAmount);
	}

	public void Drain(int amount)
	{
		Value -= amount;
	}

	public void Replenish(int amount)
	{
		Value += amount;
	}
}
