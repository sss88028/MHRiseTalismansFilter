using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRiseTalismansFilter
{
	public partial class Form1 : Form
	{
		#region private-field
		private const string _skillDataPath = @".\TextData\Skill.txt";
		#endregion private-field

		#region public-method
		public Form1()
		{
			InitializeComponent();
		}
		#endregion public-method

		#region private-field
		private void Form1_Load(object sender, EventArgs e)
		{
			InitSkill();
		}

		private void InitSkill() 
		{
			using (var stream = new StreamReader(_skillDataPath))
			{
				var json = stream.ReadToEnd();
				using (var reader = new JsonTextReader(new StringReader(json)))
				{
					var skills = (JArray)JToken.ReadFrom(reader);
					foreach (var skill in skills) 
					{
						SkillSystem.Instance.AddSkill(new Skill((JObject)skill));
					}
				}
			}
		}
		#endregion private-field
	}
}
