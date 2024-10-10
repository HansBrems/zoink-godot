using System;
using Godot;

namespace Zoink.scripts;

public partial class PackedSceneLoader : Node
{
	[Obsolete]
	public static PackedScene Load(string name)
	{
		return ResourceLoader.Load<PackedScene>(SceneUris.Get(name));
	}

	[Obsolete]
	public static PackedScene Load(string category, string name)
	{
		return ResourceLoader.Load<PackedScene>(SceneUris.Get(category, name));
	}

	public static PackedScene Load(string root, string category, string name)
	{
		return ResourceLoader.Load<PackedScene>(SceneUris.Get(root, category, name));
	}
}
