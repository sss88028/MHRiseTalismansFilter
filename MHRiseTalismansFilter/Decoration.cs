﻿using System;
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
		public int ParentId = -1;
		public readonly int Id;
		#endregion public-field

		#region private-field
		private DecorationListViewItem _item;
		private Dictionary<int, int> _skillDict = new Dictionary<int, int>();
		private int[] _slots = new int[3];

		private static int _serialId = 0;
		private static Dictionary<int, int> _skillCompareDict = new Dictionary<int, int>();
		private static Dictionary<int, int> _slotCompareDict = new Dictionary<int, int>();
		#endregion private-field

		#region public-property
		public DecorationListViewItem Item 
		{
			get 
			{
				if (_item == null) 
				{
					_item = new DecorationListViewItem(this);
					_item.Text = Id.ToString();
					_item.SubItems.Add(Name);
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

					slotStr = $"{_slots[0]}-{_slots[1]}-{_slots[2]}";
					_item.SubItems.Add($"{slotStr}");
					_item.SubItems.Add($"{ParentId}");
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

		public Decoration() 
		{
			Id = _serialId++;
		}

		public void Refresh()
		{
			Item.Text = Id.ToString();
			Item.SubItems.Clear();
			Item.SubItems.Add(Name);
			var count = 2;
			foreach (var pair in _skillDict)
			{
				if (Skill.SkillDict.TryGetValue(pair.Key, out var skill))
				{
					Item.SubItems.Add($"{skill.Name}");
				}
				Item.SubItems.Add($"{pair.Value}");
				count--;
			}

			for (var i = 0; i < count; i++)
			{
				if (Skill.SkillDict.TryGetValue(0, out var skill))
				{
					_item.SubItems.Add($"{skill.Name}");
				}
				Item.SubItems.Add($"{0}");
			}

			var slotStr = string.Empty;

			slotStr = $"{_slots[0]}-{_slots[1]}-{_slots[2]}";
			Item.SubItems.Add($"{slotStr}");
			Item.SubItems.Add($"{ParentId}");
			Item.SubItems.Add(string.Empty);
		}

		public void AddSkill(int skillId, int skillLevel)
		{
			_skillDict[skillId] = skillLevel;
		}

		public void SetSlot(int[] array) 
		{
			Array.Sort(array);
			_slots = array.Reverse().ToArray();
		}

		public int CompareTo(Decoration other)
		{
			if (other == this) 
			{
				return 0;
			}

			_skillCompareDict.Clear();
			_slotCompareDict.Clear();

			foreach (var pair in _skillDict)
			{
				var key = pair.Key;
				if (_skillCompareDict.ContainsKey(pair.Key))
				{
					_skillCompareDict[key] += pair.Value;
				}
				else
				{
					_skillCompareDict[key] = pair.Value;
				}
			}

			foreach (var slot in _slots)
			{
				if (slot == 0) 
				{
					continue;
				}
				if (!_slotCompareDict.ContainsKey(slot)) 
				{
					_slotCompareDict[slot] = 0;
				}
				_slotCompareDict[slot]++;
			}

			foreach (var pair in other._skillDict) 
			{
				var key = pair.Key;
				if (_skillCompareDict.ContainsKey(pair.Key))
				{
					_skillCompareDict[key] -= pair.Value;
				}
				else 
				{
					_skillCompareDict[key] = -pair.Value;
				}
			}

			foreach (var slot in other._slots)
			{
				if (slot == 0)
				{
					continue;
				}
				if (!_slotCompareDict.ContainsKey(slot))
				{
					_slotCompareDict[slot] = 0;
				}
				_slotCompareDict[slot]--;
			}

			var isSlotUnbalance = _slotCompareDict.Any(p => p.Value > 0) && _slotCompareDict.Any(p => p.Value < 0);

			var remainSkills = from p in _skillCompareDict
							  where p.Value < 0
							  orderby Skill.SkillDict[p.Key].Size descending
							  select p;

			foreach (var remainSkill in remainSkills) 
			{
				var skill = Skill.SkillDict[remainSkill.Key];

				if (skill.Size == null) 
				{
					continue;
				}

				var skillSize = skill.Size.Value;
				for (var i = 3; i >= skillSize; i--)
				{
					if (!_slotCompareDict.ContainsKey(i)) 
					{
						continue;
					}

					if (_slotCompareDict[i] > 0)
					{
						var diff = _slotCompareDict[i] - remainSkill.Value;

						if (diff < 0)
						{
							_slotCompareDict[i] = 0;
							_skillCompareDict[remainSkill.Key] = diff;
						}
						else
						{
							_slotCompareDict[i] = diff;
							_skillCompareDict[remainSkill.Key] = 0;
						}
					}
				}
			}

			var result = 0;

			bool isBigger = _skillCompareDict.Any(p => p.Value > 0) || _slotCompareDict.Any(p => p.Value > 0);
			bool isSmaller = _skillCompareDict.Any(p => p.Value < 0) || _slotCompareDict.Any(p => p.Value < 0);

			if (!isBigger && !isSmaller)
			{
				result = CompareSlot(other, isSlotUnbalance);
			}
			else if (isBigger && !isSmaller)
			{
				result = 1;
			}
			else if (!isBigger && isSmaller)
			{
				result = -1;
			}
			else if (isBigger && isSmaller)
			{
				result = CompareSlot(other, isSlotUnbalance);
			}

			return result;
		}
		#endregion public-method

		#region private-method
		private int CompareSlot(Decoration other, bool isSlotUnbalance)
		{
			var result = 0;
			if (isSlotUnbalance)
			{
				_slotCompareDict.Clear();

				for (var i = 1; i <= 3; i++)
				{
					_slotCompareDict[i] = 0;
				}
				foreach (var slot in _slots)
				{
					if (slot == 0)
					{
						continue;
					}
					for (var i = 1; i <= slot; i++)
					{
						_slotCompareDict[i]++;
					}
				}
				foreach (var slot in other._slots)
				{
					if (slot == 0)
					{
						continue;
					}
					for (var i = 1; i <= slot; i++)
					{
						_slotCompareDict[i]--;
					}
				}
				isSlotUnbalance = _slotCompareDict.Any(p => p.Value > 0) && _slotCompareDict.Any(p => p.Value < 0);
				if (isSlotUnbalance)
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}
		#endregion private-method
	}
}
