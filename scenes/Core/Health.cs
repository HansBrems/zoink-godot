using Godot;

namespace Zoink.scenes.Core;

public partial class Health : Node2D
{
	private int _currentHealth;

	[Signal]
	public delegate void OnNoHealthEventHandler();

	[Export]
	public int MaxHealth { get; set; }

	[Export]
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = value;
			if (_currentHealth <= 0) EmitSignal(SignalName.OnNoHealth);
		}
	}
}
