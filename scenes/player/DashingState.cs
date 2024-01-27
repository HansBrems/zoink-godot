using Godot;

public partial class DashingState : State
{
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
		GD.Print("Entering dashing state");
		_inputVector = Player.GetInputVector();
		Player.Dash();
		_dashTimer.Start();
	}

	public override void Exit()
	{
		GD.Print("Leaving dashing state");
		Player.EndDash();
		_cooldownTimer.Start();
	}

	public override void Update(double delta)
	{
		Player.Move(_inputVector);
	}
}
