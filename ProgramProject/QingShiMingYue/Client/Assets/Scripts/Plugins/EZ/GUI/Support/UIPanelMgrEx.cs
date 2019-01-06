using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Panel showing layer.
public enum _UILayer
{
	Invalid = -1,
	BottomMost = 0,
	Bottom = 1,
	Normal = 2,
	Top = 3,
	TopMost = 4,
}

/// <summary>
/// Panel manager extension. To manage panel by its local z order.
/// A panel can be shown at top or bottom.
/// </summary>
public class UIPanelMgrEx
{
	private class PanelLayer
	{
		public PanelLayer(float baseOffset, float layerSpace, float panelSpace)
		{
			this.baseOffset = baseOffset;
			this.layerSpace = layerSpace;
			this.panelSpace = panelSpace;
			currentOffsetZ = baseOffset;
		}

		private float baseOffset; // Start offset 
		private float layerSpace; // Offset range for this layer
		private float panelSpace; // Space for each panel, use to layout panel layer
		private float currentOffsetZ; // Current offset.
		private float MaxOffset { get { return baseOffset + layerSpace; } } // Max offset
		private List<UIPanelEx> shownPanelList = new List<UIPanelEx>(); // Panels is shown in this layer

		/// <summary>
		/// Show panel and layout panel layer
		/// </summary>
		public void ShowPanel(UIPanelEx panel)
		{
			// Get next offset
			IncreaseOffset();

			// Show this panel
			panel.Show(currentOffsetZ);

			// Add to shown list
			AddPanel(panel);
		}

		/// <summary>
		/// Hide a panel, shown in this layer before.
		/// </summary>
		public void HidePanel(UIPanelEx panel)
		{
			// Hide this panel
			panel.Hide();

			// Remove from shown list
			RemovePanel(panel);
		}

		private void AddPanel(UIPanelEx panel)
		{
			// If the panel is shown in this layer, re-add this panel to sort layer.
			RemovePanel(panel);
			shownPanelList.Add(panel);
		}

		public void RemovePanel(UIPanelEx panel)
		{
			if (shownPanelList.Contains(panel))
			{
				shownPanelList.Remove(panel);
			}
		}

		private void IncreaseOffset()
		{
			currentOffsetZ += panelSpace;
			if ((layerSpace >= 0 && currentOffsetZ > MaxOffset) || (layerSpace < 0 && currentOffsetZ < MaxOffset))
			{
				// Reach the range of this layer, reset the offset of all shown panel.
				ResetOffset();
			}
		}

		private void DecreaesOffset()
		{
			currentOffsetZ -= panelSpace;
			if ((layerSpace >= 0 && currentOffsetZ < baseOffset) || (layerSpace < 0 && currentOffsetZ > baseOffset))
			{
				currentOffsetZ = baseOffset;
			}
		}

		private void ResetOffset()
		{
			// Reset offset to base.
			currentOffsetZ = baseOffset;

			// Go through all shown panel, and reset panel layer.
			shownPanelList.Sort(Compare);
			foreach (var shownPanel in shownPanelList)
			{
				currentOffsetZ += panelSpace;

				if ((layerSpace >= 0 && currentOffsetZ > MaxOffset) || (layerSpace < 0 && currentOffsetZ < MaxOffset))
				{
					Debug.LogWarning("Layer space is not enough " + baseOffset);
					currentOffsetZ = MaxOffset;
				}

				// Reset this panel
				shownPanel.ResetOffsetZ(currentOffsetZ);
			}

			currentOffsetZ += panelSpace;
		}

		private static int Compare(UIPanelEx p1, UIPanelEx p2)
		{
			return (int)(p2.OffsetZ - p1.OffsetZ);
		}
	}

	private const float layerBaseOffsetZ = 400.0f;
	public float LayerBaseOffsetZ
	{
		get { return layerBaseOffsetZ; }
	}

	private const float layerSpaceZ = -20.0f;
	public float LayerSpaceZ
	{
		get { return layerSpaceZ; }
	}

	// Space z between panels.
	private const float panelSpaceZ = -1.0f;
	public float PanelSpaceZ
	{
		get { return panelSpaceZ; }
	}

	// Maximum z of panels, relative to the ui-root.
	public float TopMostPanelZ
	{
		get { return layerBaseOffsetZ + ((int)(_UILayer.TopMost) + 1) * LayerSpaceZ; }
	}

	// Minimum z of panels, relative to the ui-root.
	public float BottomMostPanelZ
	{
		get { return LayerBaseOffsetZ; }
	}	

	private Dictionary<_UILayer, PanelLayer> panelLayerDict = new Dictionary<_UILayer, PanelLayer>();
	private List<UIPanelEx> pnlList = new List<UIPanelEx>(); // UIPanelEx list.

	public bool Initialize()
	{
		// Initialze layer set.
		panelLayerDict.Clear();
		float layerOffset = LayerBaseOffsetZ;
		for (_UILayer i = _UILayer.BottomMost; i <= _UILayer.TopMost; ++i)
		{
			panelLayerDict.Add(i, new PanelLayer(layerOffset, LayerSpaceZ, PanelSpaceZ));
			layerOffset += LayerSpaceZ;
		}

		return true;
	}

	public void Dispose()
	{
		panelLayerDict.Clear();
		pnlList.Clear();
	}

	// Add panels to panel list.
	public void AddPanel(UIPanelEx pnl)
	{
		if (Contains(pnl))
			return;

		pnlList.Add(pnl);
	}

	// Delete panels from panel list.
	public void DelPanel(UIPanelEx panel)
	{
		if (!Contains(panel))
			return;

		foreach (var kvp in panelLayerDict)
		{
			kvp.Value.RemovePanel(panel);
		}

		pnlList.Remove(panel);
	}

	// Show panel.
	public void Show(UIPanelEx pnl, _UILayer layer)
	{
		if (!Contains(pnl))
			return;

		PanelLayer panelLayer = panelLayerDict[layer];
		panelLayer.ShowPanel(pnl);
	}

	// Hide panel.
	public void Hide(UIPanelEx pnl, _UILayer layer)
	{
		if (!Contains(pnl))
			return;

		PanelLayer panelLayer = panelLayerDict[layer];
		panelLayer.HidePanel(pnl);
	}

	private bool Contains(UIPanelEx pnl)
	{
		return pnlList.Contains(pnl);
	}


}
