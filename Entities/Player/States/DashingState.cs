using Godot;
using Zoink.Common.Interactions.States.StateMachine;

namespace Zoink.Entities.Player.States;

public partial class DashingState : State
{
	[Export] public PlayerScene Player;

	private Vector2 _inputVector;
	private Timer _cooldownTimer;
	private Timer _dashTimer;

	public override void _Ready()
	{
		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_cooldownTimer.Timeout += () => Player.CanDash = true;
		_dashTimer = GetNode<Timer>("DashTimer");
		_dashTimer.Timeout += () => TransitionToState("FightingState");
	}

	public override void Enter()
	{
		_inputVector = Player.GetInputVector();
		Player.Dash();
		_dashTimer.Start();
	}

	public override void Exit()
	{
		Player.EndDash();
		_cooldownTimer.Start();
	}

	public override void Update(double delta)
	{
		Player.Move(_inputVector);
	}
}
