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

		private List<int> _skill_1_LevelList = new List<int>();
		private List<int> _skill_2_LevelList = new List<int>();
		private List<int> _slot_1_LevelList = new List<int>();
		private List<int> _slot_2_LevelList = new List<int>();
		private List<int> _slot_3_LevelList = new List<int>();
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

			InitSkillComboBox();
			InitSlotComboBox();
			InitViewList();
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

		private void InitSkillComboBox() 
		{
			_skill_1_LevelList.Clear();
			_skill_1_LevelList.Add(0);
			_skill_2_LevelList.Clear();
			_skill_2_LevelList.Add(0);

			_skill_1_LevelComboBox.DataSource = _skill_1_LevelList;
			_skill_2_LevelComboBox.DataSource = _skill_2_LevelList;

			var skill1List = new List<Skill>(SkillSystem.Instance.SkillList);
			var skill2List = new List<Skill>(SkillSystem.Instance.SkillList);

			_skill_2_ComboBox.DataSource = skill2List;
			_skill_2_ComboBox.DisplayMember = "Name";
			_skill_2_ComboBox.ValueMember = "Name";

			_skill_1_ComboBox.DataSource = skill1List;
			_skill_1_ComboBox.DisplayMember = "Name";
			_skill_1_ComboBox.ValueMember = "Name";

			OnSkill1SelectedHandler();
		}

		private void InitSlotComboBox()
		{
			for (var i = 0; i <= 3; i++) 
			{
				_slot_1_LevelList.Add(i);
				_slot_2_LevelList.Add(i);
				_slot_3_LevelList.Add(i);
			}
			_slot1.DataSource = _slot_1_LevelList;
			_slot2.DataSource = _slot_2_LevelList;
			_slot3.DataSource = _slot_3_LevelList;
		}

		private void InitViewList()
		{
			_decorationView.View = View.Details;
			_decorationView.GridLines = true;
			_decorationView.LabelEdit = false;
			_decorationView.FullRowSelect = true;
			_decorationView.Columns.Add("name");
			_decorationView.Columns.Add("test1");
			_decorationView.Columns.Add("test2");

			var d = new Decoration();
			d.Name = "123";
			_decorationView.Items.Add(d.Item);
		}

		#region UIEvent-method
		private void AddButton_Click(object sender, EventArgs e)
		{

		}

		private void Skill1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSkill1SelectedHandler();
		}

		private void OnSkill1SelectedHandler()
		{
			var isSkill1Selected = _skill_1_ComboBox.SelectedIndex > 0;
			if (!isSkill1Selected)
			{
				_skill_2_ComboBox.SelectedIndex = 0;
				_skill_2_LevelComboBox.Enabled = false;
			}
			_skill_2_ComboBox.Enabled = isSkill1Selected;
			_skill_1_LevelComboBox.Enabled = isSkill1Selected;

			var skill = (Skill)_skill_1_ComboBox.SelectedItem;

			var oldValue = (int)_skill_1_LevelComboBox.SelectedItem;

			_skill_1_LevelComboBox.DataSource = null;
			_skill_1_LevelList.Clear();
			for (var i = isSkill1Selected ? 1 : 0; i <= skill.MaxLevel; i++)
			{
				_skill_1_LevelList.Add(i);
			}
			_skill_1_LevelComboBox.DataSource = _skill_1_LevelList;

			_skill_1_LevelComboBox.SelectedItem = Math.Min(oldValue, skill.MaxLevel);
		}

		private void Skill2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSkill2SelectedHandler();
		}

		private void OnSkill2SelectedHandler()
		{
			var isSkill2Selected = _skill_2_ComboBox.SelectedIndex > 0;
			_skill_2_LevelComboBox.Enabled = isSkill2Selected;

			var skill = (Skill)_skill_2_ComboBox.SelectedItem;

			var oldValue = (int)_skill_2_LevelComboBox.SelectedItem;

			_skill_2_LevelComboBox.DataSource = null;
			_skill_2_LevelList.Clear();
			for (var i = isSkill2Selected ? 1 : 0; i <= skill.MaxLevel; i++)
			{
				_skill_2_LevelList.Add(i);
			}
			_skill_2_LevelComboBox.DataSource = _skill_2_LevelList;

			_skill_2_LevelComboBox.SelectedItem = Math.Min(oldValue, skill.MaxLevel);
		}
		#endregion UIEvent-method

		#endregion private-field
	}
}
