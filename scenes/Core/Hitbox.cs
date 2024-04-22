using Godot;

namespace Zoink.scenes.Core;

public partial class Hitbox : Area2D
{
	[Export]
	public int Damage { get; set; }

	public Hitbox()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not Hurtbox hurtBox) return;

		hurtBox.TakeHit(Damage);
	}
}
