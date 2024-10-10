using System;
using Godot;

namespace Zoink.scripts;

public partial class SceneUris : Node
{
	[Obsolete]
	public static string Get(string name)
		=> $"res://scenes/{name}.tscn";

	[Obsolete]
	public static string Get(string category, string name)
		=> $"res://scenes/{category}/{name}/{name}.tscn";

	public static string Get(string root, string category, string name)
		=> $"res://{root}/{category}/{name}/{name}.tscn";
}
