using Godot;
using Zoink.scenes.Environment;

public partial class Environment : Node
{
	private OxygenGenerator _oxygenGenerator;
	private Oxygen _oxygen;
	private Power _power;
	private Timer _oxygenHurtTimer;

	[Export]
	public Label OxygenLabel { get; set; }

	[Export]
	public Label PowerLabel { get; set; }

	[Signal]
	public delegate void OnOxygenDepletedEventHandler();

	public override void _Ready()
	{
		_oxygenGenerator = GetNode<OxygenGenerator>("OxygenGenerator");
		_oxygenGenerator.OnOxygenGenerated += (amount) => _oxygen.Replenish(amount);

		_oxygenHurtTimer = GetNode<Timer>("OxygenHurtTimer");
		_oxygenHurtTimer.Timeout += () => EmitSignal(SignalName.OnOxygenDepleted);

		_oxygen = GetNode<Oxygen>("Oxygen");
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

		_power = GetNode<Power>("Power");
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
