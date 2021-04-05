using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	class DecorationSystem
	{
		#region private-field
		private List<Decoration> _decorations = new List<Decoration>();
		private Dictionary<int, List<Decoration>> _skill_1_Dict = new Dictionary<int, List<Decoration>>();
		private Dictionary<int, List<Decoration>> _skill_2_Dict = new Dictionary<int, List<Decoration>>();
		#endregion private-field

		#region public-method
		public void AddDecoration(Decoration decoration) 
		{
			_decorations.Add(decoration);

			var skill_1_Id = decoration.Skills[0].Id;
			{
				if (!_skill_1_Dict.TryGetValue(skill_1_Id, out var list))
				{
					list = new List<Decoration>();
					_skill_1_Dict[skill_1_Id] = list;
				}
				list.Add(decoration);
			}

			var skill_2_Id = decoration.Skills[1].Id;
			{
				if (!_skill_2_Dict.TryGetValue(skill_2_Id, out var list))
				{
					list = new List<Decoration>();
					_skill_2_Dict[skill_2_Id] = list;
				}
				list.Add(decoration);
			}
		}
		#endregion public-method
	}
}
