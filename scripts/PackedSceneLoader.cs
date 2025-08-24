using Godot;

namespace Zoink.scripts;

public partial class PackedSceneLoader : Node
{
	public static PackedScene Load(params string[] pathSegments)
	{
		var sceneUri = SceneUri.Build(pathSegments);
		return ResourceLoader.Load<PackedScene>(sceneUri);
	}
}
