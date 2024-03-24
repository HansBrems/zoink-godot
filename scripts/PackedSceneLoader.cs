using Godot;

namespace Zoink.scripts;

public partial class PackedSceneLoader : Node
{
	public static PackedScene Load(string name)
	{
		return ResourceLoader.Load<PackedScene>(SceneUris.Get(name));
	}

	public static PackedScene Load(string category, string name)
	{
		return ResourceLoader.Load<PackedScene>(SceneUris.Get(category, name));
	}
}
