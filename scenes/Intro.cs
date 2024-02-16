using Godot;

public partial class Intro : Node2D
{
	private RichTextLabel _label;
	private Timer _timer;

	public override void _Ready()
	{
		_label = GetNode<RichTextLabel>("CanvasLayer/Label");
		_label.VisibleCharacters = 0;

		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += UpdateText;
	}

	public void UpdateText()
	{
		_label.VisibleCharacters += 1;

		if (_label.VisibleCharacters == _label.GetParsedText().Length)
		{
			_timer.Stop();

			var startColor = _label.Modulate;
			var endColor = new Color(startColor.R, startColor.G, startColor.B, 0);

			var tween = GetTree().CreateTween();
			tween.TweenProperty(_label, "modulate", endColor, 5);
			tween.Finished += () => GetTree().ChangeSceneToFile(SceneUris.Get("Main"));
		}
	}
}
