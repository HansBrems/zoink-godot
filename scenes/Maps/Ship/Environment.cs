using Godot;
using Zoink.scenes.Environment;

public partial class Environment : Node
{
	private OxygenGenerator _oxygenGenerator;
	private Oxygen _oxygen;
	private Power _power;

	[Export]
	public Label OxygenLabel { get; set; }

	[Export]
	public Label PowerLabel { get; set; }

	public override void _Ready()
	{
		_oxygenGenerator = GetNode<OxygenGenerator>("OxygenGenerator");
		_oxygenGenerator.OnOxygenGenerated += (amount) => _oxygen.Replenish(amount);

		_oxygen = GetNode<Oxygen>("Oxygen");
		_oxygen.OnChange += (val) => UpdateLabel(OxygenLabel, val);

		_power = GetNode<Power>("Power");
		_power.OnChange += (val) => UpdateLabel(PowerLabel, val);

		UpdateLabel(OxygenLabel, _oxygen.Value);
		UpdateLabel(PowerLabel, _power.Value);
	}

	private void UpdateLabel(Label label, float value)
	{
		if (label == null) return;
		label.Text = $"{value}";
	}
}
