using Godot;

namespace Zoink.scenes.Player;

public partial class IdleState : Components.StateMachine.State
{
    [Export] public Player Player;

    public override void Enter()
    {
        Player.PlayAnimation("Idle");
    }

    public override void Update(double delta)
    {
        if (Player.CanShoot && Input.IsActionPressed("shoot"))
            Player.Shoot();

        if (Player.CanBuild && Input.IsActionJustPressed("build"))
            TransitionToState("BuildingState");

        var inputVector = Player.GetInputVector();
        if (inputVector != Vector2.Zero)
            TransitionToState("FightingState");

        Player.LookAtMousePosition();
    }
}