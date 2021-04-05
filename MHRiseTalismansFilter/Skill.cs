using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	public class Skill
	{
		#region public-field
		public static Dictionary<int, Skill> SkillDict = new Dictionary<int, Skill>();

		public readonly int Id;
		public readonly int? Size;
		public readonly int MaxLevel;
		#endregion public-field

		#region private-field
		private static int _serialId = 0;

		private Dictionary<string, string> _nameDict = new Dictionary<string, string>();
		#endregion private-field

		#region public-property
		public static IEnumerable<Skill> SkillList 
		{
			get 
			{
				return SkillDict.Values;
			}
		}

		public string Name
		{
			get 
			{
				if (!_nameDict.TryGetValue(SettingSystem.Instance.GetLanguageType(), out var name))
				{
					return string.Empty;
				}
				return name;
			}
		}
		#endregion public-property

		#region public-method
		public static void AddSkill(JObject jObj) 
		{
			var newSkill = new Skill(jObj);
			SkillDict.Add(newSkill.Id, newSkill);
		}

		public Skill(JObject jObj) 
		{
			Id = _serialId++;

			if (jObj.TryGetValue("size", out var sizeValue))
			{
				Size = sizeValue.Value<int>();
			}
			else 
			{
				Size = null;
			}

			if (jObj.TryGetValue("maxlevel", out var levelValue))
			{
				MaxLevel = levelValue.Value<int>();
			}
			else
			{
				MaxLevel = 0;
			}

			if (jObj.TryGetValue("name", out var nameArray))
			{
				var array = (JArray)nameArray;
				foreach (JObject content in array.Children<JObject>())
				{
					foreach (JProperty prop in content.Properties())
					{
						var languageType = prop.Name;
						var name = prop.Value.ToString();
						SettingSystem.Instance.AddLanguageType(languageType);
						_nameDict.Add(languageType, name);
					}
				}
			}
		}
		#endregion public-method
	}
}
