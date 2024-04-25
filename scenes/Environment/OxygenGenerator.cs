using Godot;
using Zoink.scenes.Environment;

public partial class OxygenGenerator : StaticBody2D
{
	private Timer _generateTimer;

	[Export]
	public int EnergyCost { get; set; } = 20;

	[Export]
	public int GenerationAmount { get; set; } = 20;

	[Export]
	public Power PowerSource { get; set; }

	[Signal]
	public delegate void OnOxygenGeneratedEventHandler(int amount);

	public override void _Ready()
	{
		_generateTimer = GetNode<Timer>("GenerateTimer");
		_generateTimer.Timeout += GenerateOxygen;
	}

	public void GenerateOxygen()
	{
		if (PowerSource.Drain(EnergyCost))
			EmitSignal(SignalName.OnOxygenGenerated, GenerationAmount);
	}
}
