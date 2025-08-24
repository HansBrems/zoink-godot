using Godot;
using HurtboxScene = Zoink.Core.Hurtbox.HurtboxScene;

namespace Zoink.Core.Hitbox;

public partial class HitboxScene : Area2D
{
	[Export]
	public int Damage { get; set; }

	public HitboxScene()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not HurtboxScene hurtBox) return;

		hurtBox.TakeHit(Damage);
	}
}
