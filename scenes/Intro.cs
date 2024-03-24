using Godot;

namespace Zoink.scenes;

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

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Cancel")) TransitionToNextScene();
    }

    private void UpdateText()
    {
        _label.VisibleCharacters += 1;

        if (_label.VisibleCharacters == _label.GetParsedText().Length)
        {
            TransitionToNextScene();
        }
    }

    private void TransitionToNextScene()
    {
        _timer.Stop();

        var startColor = _label.Modulate;
        var endColor = new Color(startColor.R, startColor.G, startColor.B, 0);
        var tween = GetTree().CreateTween();
        tween.TweenProperty(_label, "modulate", endColor, 1);
        tween.Finished += () => GetTree().ChangeSceneToFile(scripts.SceneUris.Get("Main"));
    }
}