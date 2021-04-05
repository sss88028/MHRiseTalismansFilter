using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	class Decoration
	{
		#region public-field
		public string Name;
		public Skill[] Skills = new Skill[2];
		public int[] Slots = new int[3];
		#endregion public-field

		#region private-field
		private ListViewItem _item;
		#endregion private-field

		#region public-property
		public ListViewItem Item 
		{
			get 
			{
				if (_item == null) 
				{
					_item = new ListViewItem();
					_item.Text = Name;
					_item.SubItems.Add($"456");
					_item.SubItems.Add($"789");
				}
				return _item;
			}
		}
		#endregion public-property

		#region public-method
		#endregion public-method
	}
}
