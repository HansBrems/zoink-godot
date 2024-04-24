using Godot;
using Zoink.scenes.Environment;

public partial class OxygenGenerator : StaticBody2D
{
	private Timer _generateTimer;

	[Export]
	public float EnergyCost { get; set; } = 20f;

	[Export]
	public float GenerationAmount { get; set; } = 20f;

	[Export]
	public Power PowerSource { get; set; }

	[Signal]
	public delegate void OnOxygenGeneratedEventHandler(float amount);

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
