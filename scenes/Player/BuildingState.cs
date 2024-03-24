using Godot;

namespace Zoink.scenes.Player;

public partial class BuildingState : Components.StateMachine.State
{
    [Export] public Player Player;

    public override void Enter()
    {
        Player.StartBuilding();
    }

    public override void Update(double delta)
    {
        if (Input.IsActionJustPressed("build"))
        {
            Player.CancelBuilding();
            TransitionToState("IdleState");
        }

        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            TransitionToState("BuildingTurretState");
        }
    }
}