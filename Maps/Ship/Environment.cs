using Godot;
using Zoink.Entities.Systems.Power;

namespace Zoink.Maps.Ship;

public partial class Environment : Node
{
	private Entities.Systems.OxygenGenerator.OxygenGeneratorScene _oxygenGenerator;
	private Entities.Systems.Oxygen.OxygenScene _oxygen;
	private PowerScene _power;
	private Timer _oxygenHurtTimer;

	[Export]
	public Label OxygenLabel { get; set; }

	[Export]
	public Label PowerLabel { get; set; }

	[Signal]
	public delegate void OnOxygenDepletedEventHandler();

	public override void _Ready()
	{
		_oxygenGenerator = GetNode<Entities.Systems.OxygenGenerator.OxygenGeneratorScene>("OxygenGenerator");
		_oxygenGenerator.OnOxygenGenerated += (amount) => _oxygen.Replenish(amount);

		_oxygenHurtTimer = GetNode<Timer>("OxygenHurtTimer");
		_oxygenHurtTimer.Timeout += () => EmitSignal(SignalName.OnOxygenDepleted);

		_oxygen = GetNode<Entities.Systems.Oxygen.OxygenScene>("Oxygen");
		_oxygen.OnChange += (val) =>
		{
			UpdateLabel(OxygenLabel, val);
			if (val == 0 && _oxygenHurtTimer.IsStopped())
			{
				_oxygenHurtTimer.Start();
			}
			else if (val != 0 && !_oxygenHurtTimer.IsStopped())
			{
				_oxygenHurtTimer.Stop();
			}
		};

		_power = GetNode<PowerScene>("Power");
		_power.OnChange += (val) => UpdateLabel(PowerLabel, val);
		_power.OnDepleted += () => _power.TurnOff();

		UpdateLabel(OxygenLabel, _oxygen.Value);
		UpdateLabel(PowerLabel, _power.Value);
	}

	private void UpdateLabel(Label label, float value)
	{
		if (label == null) return;
		label.Text = $"{value}";
	}
}
