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
		public readonly int? Size;
		public readonly int MaxLevel;
		#endregion public-field

		#region private-field
		private Dictionary<string, string> _nameDict = new Dictionary<string, string>();
		#endregion private-field

		#region public-property

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
		public Skill(JObject jObj) 
		{
			if (jObj.TryGetValue("size", out var value))
			{
				Size = value.Value<int>();
			}
			else 
			{
				Size = null;
			}

			if (jObj.TryGetValue("name", out var nameArray))
			{
				var array = (JArray)nameArray;
				foreach (JObject content in array.Children<JObject>())
				{
					foreach (JProperty prop in content.Properties())
					{
						_nameDict.Add(prop.Name, prop.Value.ToString());
					}
				}
			}
		}
		#endregion public-method
	}
}
