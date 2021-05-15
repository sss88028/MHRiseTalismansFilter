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
		#region public-field
		public static int SerialId = 1;
		public string Name;
		public int ParentId = -1;
		public readonly int Id;
		#endregion public-field

		#region private-field
		private DecorationListViewItem _item;
		private Dictionary<int, int> _skillDict = new Dictionary<int, int>();
		private Dictionary<int, int> _newSkillDict = new Dictionary<int, int>();
		private int[] _slots = new int[3];

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
					Refresh();
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
					var id = default(int);
					var level = skill.Value<int>("skillLevel");
					if (skill.TryGetValue("skillId_New", out var newId))
					{
						id = newId.Value<int>();
						decoration._newSkillDict.Add(id, level);
						decoration._skillDict.Add(Skill.NewSkillDict[id].Id, level);

					}
					else if (skill.TryGetValue("skillId", out var oldId))
					{
						id = oldId.Value<int>();
						decoration._skillDict.Add(id, level);
					}
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
			Id = SerialId++;
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

			var slotStr = $"{_slots[0]}-{_slots[1]}-{_slots[2]}";
			Item.SubItems.Add($"{slotStr}");
			if (ParentId != -1)
			{
				Item.SubItems.Add($"{ParentId}");
			}
			else
			{
				Item.SubItems.Add($"");
			}
			Item.SubItems.Add("Remove");
		}

		public void AddSkill(int skillId, int skillLevel)
		{
			_skillDict[skillId] = skillLevel;
		}

		public void AddNewSkill(int newSkillId, int skillLevel)
		{
			_newSkillDict[newSkillId] = skillLevel;
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
			var isSkillBigger = _skillCompareDict.All(k => k.Value >= 0);
			var isSkillSmall = _skillCompareDict.All(k => k.Value <= 0);

			BuildSlotCompareDict(_slotCompareDict, _slots, other._slots);

			var isSlotEqual = _slotCompareDict.All(k => k.Value == 0);
			if (isSlotEqual) 
			{
				if (isSkillBigger)
				{
					return 1;
				}
				else if(isSkillSmall)
				{
					return -1;
				}
			}

			var isCanSolve = !_skillCompareDict.Any(p => p.Value < 0 && !Skill.SkillDict[p.Key].Size.HasValue);
			var isCanBeSolved = !_skillCompareDict.Any(p => p.Value > 0 && !Skill.SkillDict[p.Key].Size.HasValue);
			if (!isCanSolve && !isCanBeSolved)
			{
				return 0;
			}

			if (isCanSolve)
			{
				CompareSlot(_slotCompareDict, _slots, other._slots);
				var remainSkills = from p in _skillCompareDict
								   where p.Value < 0
								   orderby Skill.SkillDict[p.Key].Size descending
								   select new 
								   {
									   Size = Skill.SkillDict[p.Key].Size.Value, 
									   Value = p.Value ,
								   };

				foreach (var skill in remainSkills)
				{
					for (var i = 1; i <= skill.Size; i++)
					{
						_slotCompareDict[i] += skill.Value;
					}
				}
				isCanSolve = _slotCompareDict.All(p => p.Value >= 0);
			}
			if (isCanBeSolved)
			{
				CompareSlot(_slotCompareDict, _slots, other._slots);
				var remainSkills = from p in _skillCompareDict
								   where p.Value > 0
								   orderby Skill.SkillDict[p.Key].Size descending
								   select new
								   {
									   Size = Skill.SkillDict[p.Key].Size.Value,
									   Value = p.Value,
								   };

				foreach (var skill in remainSkills)
				{
					for (var i = 1; i <= skill.Size; i++)
					{
						_slotCompareDict[i] += skill.Value;
					}
				}
				isCanBeSolved = _slotCompareDict.All(p => p.Value <= 0);
			}

			if (!isCanSolve && !isCanBeSolved)
			{
				return 0;
			}

			var result = 0;
			if (isCanSolve && !isCanBeSolved)
			{
				result = 1;
			}
			else if (!isCanSolve && isCanBeSolved)
			{
				result = -1;
			}

			return result;
		}

		public void Serialize(JsonTextWriter jsonTextWriter)
		{
			jsonTextWriter.WriteStartObject();
			jsonTextWriter.WritePropertyName("id");
			jsonTextWriter.WriteValue(Id);
			jsonTextWriter.WritePropertyName("name");
			jsonTextWriter.WriteValue(Name);

			jsonTextWriter.WritePropertyName("skills");
			jsonTextWriter.WriteStartArray();
			foreach (var skill in _skillDict)
			{
				jsonTextWriter.WriteStartObject();
				jsonTextWriter.WritePropertyName("skillId");
				jsonTextWriter.WriteValue(skill.Key);
				jsonTextWriter.WritePropertyName("skillId_New");
				jsonTextWriter.WriteValue(Skill.SkillDict[skill.Key].NewId);
				jsonTextWriter.WritePropertyName("skillName");
				jsonTextWriter.WriteValue(Skill.SkillDict[skill.Key].Name);
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
			if (selfSlots != null)
			{
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
			}
			if (otherSlots != null)
			{
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
