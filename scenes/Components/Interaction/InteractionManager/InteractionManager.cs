using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InteractionManager : Node2D
{
	private const string Text = "E to ";

	private Label _label;
	private Player _player;
	private bool _canInteract = true;
	private List<InteractionArea> _activeAreas = new();
	private InteractionArea _activeArea;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
	}

	public override void _Process(double delta)
	{
		if (_player != null && _activeAreas.Count > 0 && _canInteract)
		{
			_activeArea = _activeAreas
				.OrderBy(area => area.GlobalPosition.DistanceTo(_player.GlobalPosition))
				.First();

			var labelPosX = _activeArea.GlobalPosition.X - _label.Size.X / 2;
			var labelPosY = _activeArea.GlobalPosition.Y - 36;
			//_label.GlobalPosition = new Vector2(labelPosX, labelPosY);
			//_label.Text = $"{Text} {_activeArea.ActionName}";
			//_label.Show();
		}
		else
		{
			_label.Hide();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!_canInteract || _activeArea == null || !@event.IsActionPressed("interact")) return;
		_canInteract = false;
		_activeArea.Interact.Call();
		_canInteract = true;
	}

	public void RegisterPlayer(Player player)
	{
		_player = player;
	}

	public void RegisterArea(InteractionArea area)
	{
		_activeAreas.Add(area);
	}

	public void UnregisterArea(InteractionArea area)
	{
		if (_activeArea == area) _activeArea = null;
		_activeAreas.Remove(area);
	}
}
