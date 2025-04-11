using Godot;

namespace Zoink.scenes.Core;

public partial class HurtboxScene : Area2D
{
	[Signal]
	public delegate void OnHurtEventHandler(int damage);

	public void TakeHit(int damage)
	{
		EmitSignal(SignalName.OnHurt, damage);
	}
}
