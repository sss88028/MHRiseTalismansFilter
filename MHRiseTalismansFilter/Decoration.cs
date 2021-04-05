using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	class Decoration : IComparable<Decoration>
	{
		#region public-field
		public string Name;
		public int[] Slots = new int[3];
		#endregion public-field

		#region private-field
		private DecorationListViewItem _item;
		private Dictionary<int, int> _skillDict = new Dictionary<int, int>();
		#endregion private-field

		#region public-property
		public DecorationListViewItem Item 
		{
			get 
			{
				if (_item == null) 
				{
					_item = new DecorationListViewItem(this);
					_item.Text = Name;
					var count = 2;
					foreach (var pair in _skillDict)
					{
						if (Skill.SkillDict.TryGetValue(pair.Key, out var skill))
						{
							_item.SubItems.Add($"{skill.Name}");
						}
						_item.SubItems.Add($"{pair.Value}");
						count--;
					}

					for (var i = 0; i < count; i++)
					{
						if (Skill.SkillDict.TryGetValue(0, out var skill))
						{
							_item.SubItems.Add($"{skill.Name}");
						}
						_item.SubItems.Add($"{0}");
					}


					var slotStr = string.Empty;

					slotStr = $"{Slots[0]}-{Slots[1]}-{Slots[2]}";
					_item.SubItems.Add($"{slotStr}");
					_item.SubItems.Add(string.Empty);
				}
				return _item;
			}
		}
		#endregion public-property

		#region public-method

		public static implicit operator DecorationListViewItem(Decoration decoration)
		{
			return decoration.Item;
		}

		public void AddSkill(int skillId, int skillLevel)
		{
			_skillDict[skillId] = skillLevel;
		}

		public int CompareTo(Decoration other)
		{

			return 0;
		}
		#endregion public-method
	}
}
