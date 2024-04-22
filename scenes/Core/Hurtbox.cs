using Godot;

namespace Zoink.scenes.Core;

public partial class Hurtbox : Area2D
{
	[Signal]
	public delegate void OnHurtEventHandler(int damage);

	public void TakeHit(int damage)
	{
		EmitSignal(SignalName.OnHurt, damage);
	}
}
