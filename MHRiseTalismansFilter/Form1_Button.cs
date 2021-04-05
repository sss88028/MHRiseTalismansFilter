using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	partial class Form1
	{
		#region private-field
		private List<Button> _removeButtons = new List<Button>();
		#endregion private-field

		#region private-method
		private void AddRemoveButton(ListViewItem item) 
		{
			var newButton = new Button();
			newButton.Text = "X";
			newButton.Click += (sender, e) =>
			{
				var result = MessageBox.Show($"Delete this decoration?", "Confirm Message", MessageBoxButtons.OKCancel);
				if (result == DialogResult.OK)
				{
					_removeButtons.Remove(newButton);
					_decorationView.Controls.Remove(newButton);
					_decorationView.Items.Remove(item);
				}
				else if (result == DialogResult.Cancel)
				{
				}
			};

			_decorationView.Controls.Add(newButton);

			var length = item.SubItems.Count - 1;
			var bounds = item.SubItems[length].Bounds;
			var point = new Point(bounds.Left, bounds.Top);
			var size = new Size(bounds.Width, bounds.Height);
			newButton.Size = size;
			newButton.Location = point;
			newButton.Visible = true;

			_removeButtons.Add(newButton);
		}
		#endregion private-method
	}
}
