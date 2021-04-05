using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	class SkillSystem : Singleton<SkillSystem>
	{
		#region private-field
		private List<Skill> _skillList = new List<Skill>();
		#endregion private-field

		#region public-method
		public void AddSkill(Skill skill) 
		{
			_skillList.Add(skill);
		}
		#endregion public-method
	}
}
