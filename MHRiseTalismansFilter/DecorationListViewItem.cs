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
		#endregion public-field

		#region private-field
		private ListView _listView;
		private Decoration _decoration;
		#endregion private-field

		#region public-method
		public DecorationListViewItem(Decoration decoration) : base()
		{
			_decoration = decoration;
		}

		public void SetView(ListView listView)
		{
			var length = SubItems.Count - 1;
			var bounds = SubItems[length].Bounds;
			var point = new Point(bounds.Left, bounds.Top);
			var size = new Size(bounds.Width, bounds.Height);

			_listView = listView;

			_listView.ColumnWidthChanged -= OnColumnWidthChanged;
			_listView.ColumnWidthChanged += OnColumnWidthChanged;
		}

		public void RemoveItem()
		{
			var result = MessageBox.Show($"Delete this decoration?", "Confirm Message", MessageBoxButtons.OKCancel);
			if (result == DialogResult.OK)
			{
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
		#endregion public-method

		#region private-method
		private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			foreach (var item in _listView.Items)
			{
				((DecorationListViewItem)item).SetView(_listView);
			}
		}
		#endregion private-method
	}
}
