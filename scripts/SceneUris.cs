using Godot;

public partial class SceneUris : Node
{
    public static string Get(string category, string name)
        => $"res://scenes/{category}/{name}/{name}.tscn";
}