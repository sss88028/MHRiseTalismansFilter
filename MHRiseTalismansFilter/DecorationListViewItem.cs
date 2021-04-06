using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	class DecorationListViewItem : ListViewItem
	{
		#region public-field
		public Button RemoveButton = new Button();
		#endregion public-field

		#region private-field
		private ListView _listView;
		private Decoration _decoration;
		#endregion private-field

		#region public-method
		public DecorationListViewItem(Decoration decoration) : base()
		{
			RemoveButton.Click += OnClickRemove;
			RemoveButton.Visible = false;

			_decoration = decoration;
		}

		public void SetView(ListView listView)
		{
			var length = SubItems.Count - 1;
			var bounds = SubItems[length].Bounds;
			var point = new Point(bounds.Left, bounds.Top);
			var size = new Size(bounds.Width, bounds.Height);

			RemoveButton.Size = size;
			RemoveButton.Location = point;
			RemoveButton.Visible = true;
			RemoveButton.Text = "X";

			_listView = listView;
			_listView.Controls.Add(RemoveButton);
			_listView.ColumnWidthChanged += OnColumnWidthChanged;
		}
		#endregion public-method

		#region private-method
		private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			foreach (var item in _listView.Items)
			{
				((DecorationListViewItem)item).SetView(_listView);
			}
		}

		private void OnClickRemove(object sender, EventArgs e)
		{
			var result = MessageBox.Show($"Delete this decoration?", "Confirm Message", MessageBoxButtons.OKCancel);
			if (result == DialogResult.OK)
			{
				_listView.Controls.Remove(RemoveButton);
				_listView.Items.Remove(this);
				foreach (var item in _listView.Items) 
				{
					((DecorationListViewItem)item).SetView(_listView);
				}
				DecorationSystem.Instance.RemoveDecoration(_decoration);
			}
			else if (result == DialogResult.Cancel)
			{
			}
		}
		#endregion private-method
	}
}
