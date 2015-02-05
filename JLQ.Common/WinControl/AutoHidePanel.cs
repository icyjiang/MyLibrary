namespace JLQ.Common
{
	public class AutoHidePanel : System.ComponentModel.Component
	{
		public AutoHidePanel()
		{
		}

		protected int OriginalSplitterDistance { get; set; }
		protected System.Windows.Forms.SplitContainer MySplitContainer { get; set; }
		protected System.Windows.Forms.SplitterPanel OutsideSplitterPanel { get; set; }

		private System.Windows.Forms.SplitterPanel _mySplitterPanel;
		[System.ComponentModel.DefaultValue(null)]
		public System.Windows.Forms.SplitterPanel MySplitterPanel
		{
			get
			{
				return (_mySplitterPanel);
			}
			set
			{
				_mySplitterPanel = value;

				if (DesignMode == false)
				{
					// **************************************************
					MySplitContainer = value.Parent as System.Windows.Forms.SplitContainer;
					// **************************************************

					// **************************************************
					OriginalSplitterDistance = MySplitContainer.SplitterDistance;
					// **************************************************

					// **************************************************
					value.MouseEnter += new System.EventHandler(InsidePanel_MouseEnter);
					RaiseMouseEnterOfInsidePanelControls(value.Controls);
					// **************************************************

					// **************************************************
					if (MySplitContainer.Panel1 == value)
					{
						OutsideSplitterPanel = MySplitContainer.Panel2;
					}
					else
					{
						OutsideSplitterPanel = MySplitContainer.Panel1;
					}
					// **************************************************

					// **************************************************
					OutsideSplitterPanel.MouseEnter += new System.EventHandler(OutsidePanel_MouseEnter);
					RaiseMouseEnterOfOutsidePanelControls(OutsideSplitterPanel.Controls);
					// **************************************************

					// **************************************************
					MySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(MySplitContainer_SplitterMoved);
					// **************************************************
				}
			}
		}

		protected void MySplitContainer_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			if (MySplitterPanel == MySplitContainer.Panel1)
			{
				if ((MySplitContainer.SplitterDistance != MySplitContainer.Panel1MinSize) &&
					(MySplitContainer.SplitterDistance != OriginalSplitterDistance))
				{
					MySplitContainer.SuspendLayout();
					OriginalSplitterDistance = MySplitContainer.SplitterDistance;
					MySplitContainer.ResumeLayout();
				}
			}
			else
			{
				if ((MySplitContainer.SplitterDistance != MySplitContainer.Width - MySplitContainer.Panel2MinSize - 4) &&
					(MySplitContainer.SplitterDistance != OriginalSplitterDistance))
				{
					MySplitContainer.SuspendLayout();
					OriginalSplitterDistance = MySplitContainer.SplitterDistance;
					MySplitContainer.ResumeLayout();
				}
			}
		}

		protected void RaiseMouseEnterOfInsidePanelControls(System.Windows.Forms.Control.ControlCollection controlCollection)
		{
			if (controlCollection != null)
			{
				foreach (System.Windows.Forms.Control ctlCurrent in controlCollection)
				{
					ctlCurrent.MouseEnter += new System.EventHandler(InsidePanel_MouseEnter);
					RaiseMouseEnterOfInsidePanelControls(ctlCurrent.Controls);
				}
			}
		}

		protected void InsidePanel_MouseEnter(object sender, System.EventArgs e)
		{
			Open();
		}

		private void Open()
		{
			if (MySplitContainer.SplitterDistance != OriginalSplitterDistance)
			{
				MySplitContainer.SuspendLayout();
				MySplitContainer.SplitterDistance = OriginalSplitterDistance;
				MySplitContainer.ResumeLayout();
			}
		}

		protected void RaiseMouseEnterOfOutsidePanelControls(System.Windows.Forms.Control.ControlCollection controlCollection)
		{
			if (controlCollection != null)
			{
				foreach (System.Windows.Forms.Control ctlCurrent in controlCollection)
				{
					ctlCurrent.MouseEnter += new System.EventHandler(OutsidePanel_MouseEnter);
					RaiseMouseEnterOfOutsidePanelControls(ctlCurrent.Controls);
				}
			}
		}

		protected void OutsidePanel_MouseEnter(object sender, System.EventArgs e)
		{
			Close();
		}

		private void Close()
		{
			if (MySplitterPanel == MySplitContainer.Panel1)
			{
				if (MySplitContainer.SplitterDistance != MySplitContainer.Panel1MinSize)
				{
					MySplitContainer.SuspendLayout();
					MySplitContainer.SplitterDistance = MySplitContainer.Panel1MinSize;
					MySplitContainer.ResumeLayout();
				}
			}
			else
			{
				if (MySplitContainer.SplitterDistance != MySplitContainer.Width - MySplitContainer.Panel2MinSize - 4)
				{
					MySplitContainer.SuspendLayout();
					MySplitContainer.SplitterDistance = MySplitContainer.Width - MySplitContainer.Panel2MinSize - 4;
					MySplitContainer.ResumeLayout();
				}
			}
		}
	}
}
