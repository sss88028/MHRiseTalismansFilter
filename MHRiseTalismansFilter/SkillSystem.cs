using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	class SkillSystem : Singleton<SkillSystem>
	{
		#region public-field
		public List<Skill> SkillList = new List<Skill>();
		#endregion public-field

		#region public-method
		public void AddSkill(Skill skill) 
		{
			SkillList.Add(skill);
		}
		#endregion public-method
	}
}
