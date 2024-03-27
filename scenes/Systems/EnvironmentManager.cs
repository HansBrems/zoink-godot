using Godot;

namespace Zoink.scenes.Systems;

public partial class EnvironmentManager : Node
{
	private Timer _oxygenTicker;

	private int _oxygen = 100;

	public bool IsOxygenOn { get; set; }

	[Signal] public delegate void OnOxygenChangedEventHandler(int oxygen);

	public override void _Ready()
	{
		_oxygenTicker = GetNode<Timer>("OxygenTicker");
		_oxygenTicker.Timeout += UpdateOxygen;
	}

	private void UpdateOxygen()
	{
		if (IsOxygenOn) _oxygen = _oxygen < 100 ? _oxygen + 1 : 100;
		else _oxygen = _oxygen > 0 ? _oxygen - 1 : 0;
		EmitSignal("OnOxygenChanged", _oxygen);
	}
}
