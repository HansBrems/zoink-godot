using Godot;
using Zoink.scenes.Environment;
using Zoink.scenes.Objects.Console;

public partial class Environment : Node
{
	private Oxygen _oxygen;
	private Console _oxygenConsole;
	private Power _power;
	private Console _powerConsole;

	[Export]
	public Label OxygenLabel { get; set; }

	[Export]
	public Label PowerLabel { get; set; }

	public override void _Ready()
	{
		_oxygen = GetNode<Oxygen>("Oxygen");
		_oxygen.OnChange += (val) => UpdateLabel(OxygenLabel, val);
		_oxygenConsole = GetNode<Console>("OxygenConsole");
		_oxygenConsole.OnStateChanged += HandleOxygenChanged;
		UpdateLabel(OxygenLabel, _oxygen.CurrentValue);

		_power = GetNode<Power>("Power");
		_power.OnChange += (val) => UpdateLabel(PowerLabel, val);
		_powerConsole = GetNode<Console>("PowerConsole");
		_powerConsole.OnStateChanged += HandlePowerChanged;
		UpdateLabel(PowerLabel, _power.CurrentValue);
	}

	private void HandleOxygenChanged(bool isEnabled)
	{
		if (isEnabled)
		{
			_oxygen.DecayRate -= 2;
			_power.DecayRate += 1;
		}
		else
		{
			_oxygen.DecayRate += 2;
			_power.DecayRate -= 1;
		}
	}

	private void HandlePowerChanged(bool isEnabled)
	{
		if (isEnabled)
		{
			_power.DecayRate -= 1;
		}
		else
		{
			_power.DecayRate += 1;
		}
	}

	private void UpdateLabel(Label label, float value)
	{
		if (label == null) return;
		label.Text = $"{value}";
	}
}
