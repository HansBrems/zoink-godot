using Godot;
using Zoink.Entities.Systems.Power;

namespace Zoink.Entities.Systems.OxygenGenerator;

public partial class OxygenGeneratorScene : StaticBody2D
{
	private Timer _generateTimer;
	private PointLight2D _pointLight2D;

	[Export]
	public int EnergyCost { get; set; } = 20;

	[Export]
	public int GenerationAmount { get; set; } = 20;

	[Export]
	public PowerScene PowerSource { get; set; }

	[Signal]
	public delegate void OnOxygenGeneratedEventHandler(int amount);

	public override void _Ready()
	{
		_generateTimer = GetNode<Timer>("GenerateTimer");
		_generateTimer.Timeout += GenerateOxygen;
		_pointLight2D = GetNode<PointLight2D>("PointLight2D");
	}

	public override void _Process(double delta)
	{
		_pointLight2D.Enabled = PowerSource.IsEnabled && PowerSource.Value != 0;
	}

	public void GenerateOxygen()
	{
		if (PowerSource.Drain(EnergyCost))
			EmitSignal(SignalName.OnOxygenGenerated, GenerationAmount);
	}
}
