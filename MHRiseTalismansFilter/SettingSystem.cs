using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	class SettingSystem : Singleton<SettingSystem>
	{
		#region private-field
		private string _currentLanguage = string.Empty;
		private HashSet<string> _languages = new HashSet<string>();
		#endregion private-field

		#region public-method
		public string GetLanguageType() 
		{
			return _currentLanguage;
		}

		public void AddLanguageType(string language) 
		{
			if (_languages.Add(language))
			{
				_currentLanguage = string.IsNullOrEmpty(_currentLanguage) ? language : _currentLanguage;
			}
		}
		#endregion public-method
	}
}
