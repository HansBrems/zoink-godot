using Godot;

namespace Zoink.scenes.Core;

public partial class Health : Node2D
{
	private int _currentHealth;

	[Export]
	public int MaxHealth { get; set; }

	[Export]
	public int CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = Mathf.Clamp(value, 0, MaxHealth);
			EmitSignal(SignalName.OnChange, _currentHealth);
			if (_currentHealth <= 0) EmitSignal(SignalName.OnNoHealth);
		}
	}

	[Signal]
	public delegate void OnChangeEventHandler(int health);

    [Signal]
	public delegate void OnNoHealthEventHandler();
}
