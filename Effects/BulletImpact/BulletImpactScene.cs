using Godot;

namespace Zoink.Effects.BulletImpact;

public partial class BulletImpactScene : Node2D
{
	private GpuParticles2D _fragments;

	public override void _Ready()
	{
		_fragments = GetNode<GpuParticles2D>("Fragments");
		_fragments.Emitting = true;
	}
}
