using System.Collections.Generic;
using System.Linq;
using Godot;
using InteractionAreaScene = Zoink.Common.Interactions.InteractionArea.InteractionAreaScene;

namespace Zoink.Common.Interactions.InteractionManager;

public partial class InteractionManagerScene : Node2D
{
	private readonly List<InteractionAreaScene> _areas = new();
	private InteractionAreaScene _activeArea;
	private bool _canInteract = true;
	private Label _label;
	private PanelContainer _panelContainer;
	private scenes.Player.PlayerScene _player;

	public override void _Ready()
	{
		_label = GetNode<Label>("PanelContainer/MarginContainer/Label");
		_panelContainer = GetNode<PanelContainer>("PanelContainer");
	}

	public override void _Process(double delta)
	{
		if (_areas.Count > 0 && _canInteract)
		{
			_activeArea = GetClosestArea();
			ShowLabel();
		}
		else
		{
			_panelContainer.Hide();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!_canInteract || _activeArea == null || !@event.IsActionPressed("interact")) return;

		_canInteract = false;
		_activeArea.Interact.Call();
		_canInteract = true;
	}

	public void RegisterPlayer(scenes.Player.PlayerScene player)
	{
		_player = player;
	}

	public void RegisterArea(InteractionAreaScene area)
	{
		_areas.Add(area);
	}

	public void UnregisterArea(InteractionAreaScene area)
	{
		if (_activeArea == area) _activeArea = null;
		_areas.Remove(area);
	}

	private InteractionAreaScene GetClosestArea()
	{
		if (_player == null) return null;
		return _areas
			.OrderBy(area => area.GlobalPosition.DistanceTo(_player.GlobalPosition))
			.First();
	}

	private void ShowLabel()
	{
		if (string.IsNullOrEmpty(_activeArea.ActionName)) return;
		var labelPosX = _activeArea.GlobalPosition.X - _panelContainer.Size.X / 2;
		var labelPosY = _activeArea.GlobalPosition.Y - 12;
		_panelContainer.GlobalPosition = new Vector2(labelPosX, labelPosY);
		_label.Text = _activeArea.ActionName;
		_panelContainer.Size = _label.GetMinimumSize();
		_panelContainer.Show();
	}
}
