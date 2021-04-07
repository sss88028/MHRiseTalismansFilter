using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
		private class Slots : IComparable<Slots>
		{
			public List<Slot1> SlotDatas = new List<Slot1>();

			public int CompareTo(Slots other)
			{
				if (other == this)
				{
					return 0;
				}
				var size = SlotDatas.Count;
				if (other.SlotDatas.Count != size)
				{
					return 0;
				}

				var isBigger = true;
				for (var i = 0; i < size; i++)
				{
					isBigger &= (SlotDatas[i].CompareTo(other.SlotDatas[i]) > 0) ? true : false;
				}

				return isBigger ? 1 : -1;
			}
		}

		private class Slot1 : IComparable<Slot1>
		{
			protected int _size = 1;

			public bool IsFilled
			{
				get;
				protected set;
			}

			public Slot1()
			{
				_size = 1;
			}

			public bool FillSkill(int skillId)
			{
				var result = Skill.SkillDict[skillId].Size.HasValue;
				result &= Skill.SkillDict[skillId].Size.Value <= _size;
				IsFilled = result;
				return result;
			}

			public void Clear()
			{
				IsFilled = false;
			}

			public int CompareTo(Slot1 other)
			{
				return _size.CompareTo(other._size);
			}
		}

		private class Slot2 : Slot1
		{
			public Slot2()
			{
				_size = 2;
			}
		}

		private class Slot3 : Slot2
		{
			public Slot3()
			{
				_size = 3;
			}
		}

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
		private static Dictionary<int, int> _remainSkillCompareDict = new Dictionary<int, int>();
		private static Dictionary<int, int> _remainSlotCompareDict = new Dictionary<int, int>();
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

		public static Decoration Deserialize(JObject jObj)
		{
			var decoration = new Decoration();

			if (jObj.TryGetValue("name", out var nameToken))
			{
				decoration.Name = nameToken.Value<string>();
			}

			if (jObj.TryGetValue("skills", out var skillsToken))
			{
				var skillArray = (JArray)skillsToken;
				foreach (var skillToken in skillArray)
				{
					var skill = (JObject)skillToken;
					var id = skill.Value<int>("skillId");
					var level = skill.Value<int>("skillLevel");
					decoration._skillDict.Add(id, level);
				}
			}
			if (jObj.TryGetValue("slots", out var slotsToken))
			{
				var slotArray = (JArray)slotsToken;
				var i = 0;
				foreach (var skillToken in slotArray)
				{
					decoration._slots[i++] = skillToken.Value<int>();
				}
			}
			return decoration;
		}

		public Decoration() 
		{
			Id = _serialId++;
		}

		public void Refresh()
		{
			Item.SubItems.Clear();
			Item.Text = Id.ToString();
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

			var isSkillEqual = _skillCompareDict.All(k => k.Value == 0);
			if (isSkillEqual)
			{
				return CompareSlot(_slotCompareDict, _slots, other._slots);
			}

			BuildSlotCompareDict(_slotCompareDict, _slots, other._slots);

			BuildRemainData();

			var isCanSolve = !_skillCompareDict.Any(p => p.Value < 0 && !Skill.SkillDict[p.Key].Size.HasValue);
			var isCanBeSolved = !_skillCompareDict.Any(p => p.Value > 0 && !Skill.SkillDict[p.Key].Size.HasValue);
			if (!isCanSolve && !isCanBeSolved)
			{
				return 0;
			}
			if (isCanSolve)
			{
				var remainSkills = from p in _skillCompareDict
								   where p.Value < 0
								   orderby Skill.SkillDict[p.Key].Size descending
								   select new { Skill.SkillDict[p.Key].Size, p.Value };

				foreach (var skill in remainSkills)
				{
					var diff = _remainSlotCompareDict[skill.Size.Value] + skill.Value;

				}
			}

			var result = 0;
			return result;
		}

		public void Serialize(JsonTextWriter jsonTextWriter)
		{
			jsonTextWriter.WriteStartObject();
			jsonTextWriter.WritePropertyName("name");
			jsonTextWriter.WriteValue(Name);

			jsonTextWriter.WritePropertyName("skills");
			jsonTextWriter.WriteStartArray();
			foreach (var skill in _skillDict)
			{
				jsonTextWriter.WriteStartObject();
				jsonTextWriter.WritePropertyName("skillId");
				jsonTextWriter.WriteValue(skill.Key);
				jsonTextWriter.WritePropertyName("skillLevel");
				jsonTextWriter.WriteValue(skill.Value);
				jsonTextWriter.WriteEndObject();
			}
			jsonTextWriter.WriteEndArray();


			jsonTextWriter.WritePropertyName("slots");
			jsonTextWriter.WriteStartArray();
			foreach (var slot in _slots)
			{
				jsonTextWriter.WriteValue(slot);
			}
			jsonTextWriter.WriteEndArray();

			jsonTextWriter.WriteEndObject();
		}
		#endregion public-method

		#region private-method
		private void BuildRemainData()
		{
			_remainSkillCompareDict.Clear();
			foreach (var pair in _skillCompareDict)
			{
				_remainSkillCompareDict.Add(pair.Key, pair.Value);
			}
			_remainSlotCompareDict.Clear();
			foreach (var pair in _slotCompareDict)
			{
				_remainSlotCompareDict.Add(pair.Key, pair.Value);
			}
		}

		private static void BuildSlotCompareDict(Dictionary<int, int> slotCompareDict, int[] selfSlots, int[] otherSlots)
		{
			slotCompareDict.Clear();

			foreach (var slot in selfSlots)
			{
				if (!slotCompareDict.ContainsKey(slot))
				{
					slotCompareDict[slot] = 0;
				}
				slotCompareDict[slot]++;
			}
			foreach (var slot in otherSlots)
			{
				if (!slotCompareDict.ContainsKey(slot))
				{
					slotCompareDict[slot] = 0;
				}
				slotCompareDict[slot]--;
			}
		}

		private static int CompareSlot(Dictionary<int, int> slotCompareDict, int[] selfSlots, int[] otherSlots)
		{
			var result = 0;
			slotCompareDict.Clear();

			for (var i = 1; i <= 3; i++)
			{
				slotCompareDict[i] = 0;
			}
			foreach (var slot in selfSlots)
			{
				if (slot == 0)
				{
					continue;
				}
				for (var i = 1; i <= slot; i++)
				{
					slotCompareDict[i]++;
				}
			}
			foreach (var slot in otherSlots)
			{
				if (slot == 0)
				{
					continue;
				}
				for (var i = 1; i <= slot; i++)
				{
					slotCompareDict[i]--;
				}
			}

			var isEqual = slotCompareDict.All(p => p.Value == 0);
			if (isEqual)
			{
				return 1;
			}

			var isBigger = slotCompareDict.All(p => p.Value >= 0);
			var isSmaller = slotCompareDict.All(p => p.Value <= 0);

			var isSlotUnbalance = !isBigger && !isSmaller;

			if (isSlotUnbalance)
			{
				result = 0;
			}
			else if(isBigger)
			{
				result = 1;
			}
			else if (isSmaller)
			{
				result = -1;
			}

			return result;
		}
		#endregion private-method
	}
}
