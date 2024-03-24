using Godot;

namespace Zoink.scenes.Components.Follower;

public partial class Follower : Node2D
{
    private Timer _adjustTimer;
    private NavigationAgent2D _navigationAgent;

    [Signal]
    public delegate void OnDirectionChangedEventHandler(Vector2 direction);

    [Export]
    public Node2D Target { get; set; }

    public override void _Ready()
    {
        _adjustTimer = GetNode<Timer>("AdjustTimer");
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent");

        _adjustTimer.Timeout += AdjustNavigation;
    }

    public override void _Process(double delta)
    {
        var direction = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
        EmitSignal("OnDirectionChanged", direction);
    }

    private void AdjustNavigation()
    {
        GD.Print("Adjusting navigation");
        _navigationAgent.TargetPosition = Target.GlobalPosition;
    }
}