using Godot;

namespace Zoink.scenes.Components.ShootController;

public partial class OnShootEventArgs : GodotObject
{
	/// <summary>
	/// The direction in which the projectile is being shot.
	/// </summary>
	public Vector2 Direction { get; set; }

	/// <summary>
	/// The starting position of the projectile.
	/// </summary>
	public Vector2 Position { get; set; }

	/// <summary>
	/// The type of projectile.
	/// </summary>
	public ProjectileType ProjectileType { get; set; }
}
