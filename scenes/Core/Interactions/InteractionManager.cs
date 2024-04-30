using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Zoink.scenes.Core.Interactions;

public partial class InteractionManager : Node2D
{
	private readonly List<InteractionArea> _areas = new();
	private InteractionArea _activeArea;
	private bool _canInteract = true;
	private Label _label;
	private PanelContainer _panelContainer;
	private Player.Player _player;

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

	public void RegisterPlayer(Player.Player player)
	{
		_player = player;
	}

	public void RegisterArea(InteractionArea area)
	{
		_areas.Add(area);
	}

	public void UnregisterArea(InteractionArea area)
	{
		if (_activeArea == area) _activeArea = null;
		_areas.Remove(area);
	}

	private InteractionArea GetClosestArea()
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
