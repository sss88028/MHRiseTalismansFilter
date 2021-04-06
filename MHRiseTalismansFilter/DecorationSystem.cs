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
					var a = GetParent(set, i);
					var b = GetParent(set, j);

					var d1 = _decorations[a];
					var d2 = _decorations[b];

					var result = d1.CompareTo(d2);
					if (result > 0)
					{
						Console.WriteLine("[DecorationSystem.Filt] Bigger");
						Combine(set, i, j);
						Combine(set, a, b);
					}
					else if (result < 0)
					{
						Console.WriteLine("[DecorationSystem.Filt] Smaller");
						Combine(set, j, i);
						Combine(set, b, a);
					}
					else
					{
						Console.WriteLine("[DecorationSystem.Filt] Equal");
					}
				}
			}
		}
		#endregion public-method

		#region private-method
		private void Combine(int[] set, int parent, int child) 
		{
			var parentsParent = GetParent(set, parent);
			var childsParent = GetParent(set, child);
			if (parentsParent >= 0 && childsParent >= 0 && parentsParent == childsParent) 
			{
				return;
			}
			set[child] = parentsParent;
		}

		private int GetParent(int[] set, int child)
		{
			var parent = set[child];
			while (parent >= 0)
			{
				child = parent;
				parent = set[child];
			}
			return child;
		}
		#endregion private-method
	}
}
