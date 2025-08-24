using Godot;

namespace Zoink.scripts;

public partial class SceneUri : Node
{
	public static string Build(params string[] pathSegments)
	{
		var lastSegment = pathSegments[^1];
		var path = pathSegments.Join("/") + $"/{lastSegment}Scene.tscn";
		return $"res://{path}";
	}
}
