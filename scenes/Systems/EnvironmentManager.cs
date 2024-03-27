using System;
using Godot;

namespace Zoink.scenes.Systems;

public partial class EnvironmentManager : Node
{
	private Timer _oxygenTicker;
	private Timer _powerTicker;

	private int _oxygen = 100;
	private int _power = 100;

	public bool IsOxygenOn { get; set; }
	public bool IsPowerOn { get; set; }

	[Signal] public delegate void OnOxygenChangedEventHandler(int oxygen);
	[Signal] public delegate void OnPowerChangedEventHandler(int power);

	public override void _Ready()
	{
		_oxygenTicker = GetNode<Timer>("OxygenTicker");
		_oxygenTicker.Timeout += UpdateOxygen;
		_powerTicker = GetNode<Timer>("PowerTicker");
		_powerTicker.Timeout += UpdatePower;
	}

	private void UpdateOxygen()
    {
        var changeValue = 0;
        if (!IsOxygenOn) changeValue -= 1;
        if (IsOxygenOn) changeValue += 1;

        _oxygen = Math.Clamp(_oxygen + changeValue, 0, 100);
		EmitSignal("OnOxygenChanged", _oxygen);
	}

	private void UpdatePower()
	{
		var changeValue = 0;
		if (IsOxygenOn) changeValue -= 2;
		if (IsPowerOn) changeValue += 1;

		_power = Math.Clamp(_power + changeValue, 0, 100);
		EmitSignal("OnPowerChanged", _power);
	}
}
