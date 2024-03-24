using Godot;

namespace Zoink.scenes.Effects.BulletImpact;

public partial class BulletImpact : Node2D
{
    private GpuParticles2D _fragments;

    public override void _Ready()
    {
        _fragments = GetNode<GpuParticles2D>("Fragments");
        _fragments.Emitting = true;
    }
}