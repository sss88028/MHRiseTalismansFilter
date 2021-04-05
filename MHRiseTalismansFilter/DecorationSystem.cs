using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRiseTalismansFilter
{
	class DecorationSystem : Singleton<DecorationSystem>
	{
		#region private-field
		private List<Decoration> _decorations = new List<Decoration>();
		#endregion private-field

		#region public-method
		public void AddDecoration(Decoration decoration) 
		{
			_decorations.Add(decoration);
		}

		public void RemoveDecoration(Decoration decoration)
		{
			_decorations.Remove(decoration);
		}

		public void Filt() 
		{
			var size = _decorations.Count;

			var set = new int[size];
			for (var i = 0; i < size; i++)
			{
				set[i] = -1;
			}

			for (var i = 0; i < size; i++)
			{
				for (var j = i + 1; j < size; j++)
				{
					var d1 = _decorations[i];
					var d2 = _decorations[j];

					if (d1.CompareTo(d2) > 0)
					{
						Combine(set, i, j);
					}
					else if (d1.CompareTo(d2) < 0)
					{
						Combine(set, j, i);
					}
				}
			}
		}
		#endregion public-method

		#region private-method
		private void Combine(int[] set, int a, int b) 
		{ 
		
		}
		#endregion private-method
	}
}
