using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	class DecorationSystem : Singleton<DecorationSystem>
	{
		#region public-field
		public event Action OnLoadDecoration;
		#endregion public-field

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
#if DEBUG
						Console.WriteLine("[DecorationSystem.Filt] Bigger");
#endif
						Combine(set, i, j);
						Combine(set, a, b);
					}
					else if (result < 0)
					{
#if DEBUG
						Console.WriteLine("[DecorationSystem.Filt] Smaller");
#endif
						Combine(set, j, i);
						Combine(set, b, a);
					}
					else
					{
#if DEBUG
						Console.WriteLine("[DecorationSystem.Filt] Equal");
#endif
					}
				}
			}

			for (var i = 0; i < size; i++) 
			{
				var index = set[i];
				if (index >= 0)
				{
					_decorations[i].ParentId = _decorations[index].Id;
				}
				else
				{
					_decorations[i].ParentId = -1;
				}
				_decorations[i].Refresh();
			}
		}

		public void Serialize(TextWriter textWriter)
		{
			var jWriter = new JsonTextWriter(textWriter);
			jWriter.WriteStartArray();
			foreach (var d in _decorations)
			{
				d.Serialize(jWriter);
			}
			jWriter.WriteEndArray();
		}

		public void Deerialize(TextReader textReader)
		{
			_decorations.Clear();
			var jReader = new JsonTextReader(textReader);

			var decorations = (JArray)JToken.ReadFrom(jReader);
			foreach (var d in decorations)
			{
				_decorations.Add(Decoration.Deserialize((JObject)d));
			}

			OnLoadDecoration?.Invoke();
		}

		public void RefreshView(ListView listView)
		{
			foreach (var d in _decorations)
			{
				listView.Items.Add(d);
				d.Item.SetView(listView);
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
