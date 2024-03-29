﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MHRiseTalismansFilter
{
	public partial class Form1 : Form
	{
		#region private-field
		private const string _skillDataPath = @".\Skill.txt";
		private const string _nameDataPath = @".\Name.txt";
		private const string _saveDataFolder = @".\Save\";
		private const string _saveDataPath = _saveDataFolder + @"save.txt";

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

			_decorationView.FullRowSelect = true;

			ListViewExtender extender = new ListViewExtender(_decorationView);
			var buttonAction = new ListViewButtonColumn(8);
			buttonAction.Click += OnClickRemoveButton;

			extender.AddColumn(buttonAction);
		}
		#endregion public-method

		#region private-field
		private void Form1_Load(object sender, EventArgs e)
		{
			InitSkill();

			InitSkillComboBox();
			InitSlotComboBox();
			InitViewList();
			InitName();
			InitEvent();
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
						Skill.AddSkill((JObject)skill);
					}
				}
			}
			Console.WriteLine(Skill.SkillList.Count());
			Console.WriteLine(Skill.SkillList.Where(s => s.Size.HasValue).Count());
			Console.WriteLine(Skill.SkillList.Where(s => !s.Size.HasValue).Count());
			Console.WriteLine(Skill.SkillList.Where(s => s.Size == 1).Count());
			Console.WriteLine(Skill.SkillList.Where(s => s.Size == 2).Count());
			Console.WriteLine(Skill.SkillList.Where(s => s.Size == 3).Count());
		}

		private void InitSkillComboBox() 
		{
			_skill_1_LevelList.Clear();
			_skill_1_LevelList.Add(0);
			_skill_2_LevelList.Clear();
			_skill_2_LevelList.Add(0);

			_skill_1_LevelComboBox.DataSource = _skill_1_LevelList;
			_skill_2_LevelComboBox.DataSource = _skill_2_LevelList;

			_skill_2_ComboBox.Format += SkillComboBoxFormat;
			_skill_2_ComboBox.DataSource = new BindingSource(Skill.SkillDict, null);
			_skill_2_ComboBox.ValueMember = "Key";

			_skill_1_ComboBox.Format += SkillComboBoxFormat;
			_skill_1_ComboBox.DataSource = new BindingSource(Skill.SkillDict, null);
			_skill_1_ComboBox.ValueMember = "Key";

			OnSkill1SelectedHandler();
		}

		private void SkillComboBoxFormat(object sender, ListControlConvertEventArgs e)
		{
			var pair = (KeyValuePair<int, Skill>)e.ListItem;
			string Name = pair.Value.Name;
			e.Value = Name;
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
			_decorationView.Columns.Add("Id");
			_decorationView.Columns.Add("名稱");
			_decorationView.Columns.Add("技能");
			_decorationView.Columns.Add("LV");
			_decorationView.Columns.Add("技能");
			_decorationView.Columns.Add("LV");
			_decorationView.Columns.Add("Slot");
			_decorationView.Columns.Add("Better");
			_decorationView.Columns.Add("");
			_decorationView.DrawSubItem += ViewListDrawSubItem;

			//var d = new Decoration();
			//d.Name = "123";
			//_decorationView.Items.Add(d.Item);
		}

		private void InitName()
		{
			var nameList = new List<string>();
			using (var stream = new StreamReader(_nameDataPath))
			{
				var json = stream.ReadToEnd();
				using (var reader = new JsonTextReader(new StringReader(json)))
				{
					var names = (JArray)JToken.ReadFrom(reader);
					foreach (var name in names)
					{
						var rare = string.Empty;
						var n = string.Empty;
						var jObj = (JObject)name;
						if (jObj.TryGetValue("rare", out var rareValue))
						{
							rare = $"R{rareValue.Value<int>()}";
						}

						if (jObj.TryGetValue("name", out var nameArray))
						{
							var array = (JArray)nameArray;
							foreach (JObject content in array.Children<JObject>())
							{
								var prop = content.Properties().First();
								n = prop.Value.ToString();
							}
						}
						nameList.Add($"{rare}-{n}");
					}
				}
			}

			_nameComboBox.DataSource = new BindingSource(nameList, null);
		}

		private void InitEvent()
		{
			DecorationSystem.Instance.OnLoadDecoration += () =>
			{
				_decorationView.Items.Clear();
				_decorationView.Controls.Clear();
				DecorationSystem.Instance.RefreshView(_decorationView);
			};
			
			DecorationSystem.Instance.OnFiltedDecoration += (count) =>
			{
				label2.Text = $"{count}";
			};
		}

		private void ViewListDrawSubItem(object sender, DrawListViewSubItemEventArgs e) 
		{
			e.DrawDefault = true;
		}

		#region UIEvent-method
		private void OnClickAddButton(object sender, EventArgs e)
		{
			var decoration = new Decoration();

			var name = (string)_nameComboBox.SelectedItem;
			decoration.Name = name;

			AddSkill(decoration, _skill_1_ComboBox, _skill_1_LevelComboBox);
			AddSkill(decoration, _skill_2_ComboBox, _skill_2_LevelComboBox);

			_decorationView.Items.Add(decoration);
			decoration.Item.SetView(_decorationView);

			DecorationSystem.Instance.AddDecoration(decoration);
		}

		private void AddSkill(Decoration decoration, ComboBox skillComboBox, ComboBox skillLevelComboBox) 
		{ 
			var skillId = ((KeyValuePair<int, Skill>)skillComboBox.SelectedItem).Key;
			var levelId = (int)skillLevelComboBox.SelectedItem;
			decoration.AddSkill(skillId, levelId);
			if (Skill.SkillDict[skillId].NewId.HasValue)
			{
				decoration.AddNewSkill(Skill.SkillDict[skillId].NewId.Value, levelId);
			}
			decoration.SetSlot(new int[] 
			{
				(int)_slot1.SelectedItem,
				(int)_slot2.SelectedItem,
				(int)_slot3.SelectedItem,
			});
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

			var pair = (KeyValuePair<int, Skill>)_skill_1_ComboBox.SelectedItem;
			var skill = pair.Value;
			var oldLevel = (int)_skill_1_LevelComboBox.SelectedItem;

			_skill_1_LevelComboBox.DataSource = null;
			_skill_1_LevelList.Clear();
			for (var i = isSkill1Selected ? 1 : 0; i <= skill.MaxLevel; i++)
			{
				_skill_1_LevelList.Add(i);
			}
			_skill_1_LevelComboBox.DataSource = _skill_1_LevelList;

			_skill_1_LevelComboBox.SelectedItem = Math.Min(oldLevel, skill.MaxLevel);
		}

		private void Skill2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSkill2SelectedHandler();
		}

		private void OnSkill2SelectedHandler()
		{
			var isSkill2Selected = _skill_2_ComboBox.SelectedIndex > 0;
			_skill_2_LevelComboBox.Enabled = isSkill2Selected;

			var pair = (KeyValuePair<int, Skill>)_skill_2_ComboBox.SelectedItem;
			var skill = pair.Value;

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

		private void OnClickFiltButton(object sender, EventArgs e)
		{
			DecorationSystem.Instance.Filt();
		}

		private void OnClickSaveButton(object sender, EventArgs e)
		{

			Directory.CreateDirectory(_saveDataFolder);

			var isSave = true;
			if (File.Exists(_saveDataPath))
			{
				var result = MessageBox.Show($"Save will overwrite current saved data!!!", "Confirm Message", MessageBoxButtons.OKCancel);
				isSave = result == DialogResult.OK;
			}
			if (isSave)
			{
				using (StreamWriter sw = new StreamWriter(_saveDataPath))
				{
					DecorationSystem.Instance.Serialize(sw);
					sw.Flush();
				}
			}
		}

		private void OnClickLoadButton(object sender, EventArgs e)
		{
			if (!File.Exists(_saveDataPath))
			{
				MessageBox.Show("File not exist");
				return;
			}

			var result = MessageBox.Show($"Load will overwrite current editing data!!!", "Confirm Message", MessageBoxButtons.OKCancel);
			if (result == DialogResult.OK)
			{
				using (var stream = new StreamReader(_saveDataPath))
				{
					DecorationSystem.Instance.Deerialize(stream);
				}
			}
			else if (result == DialogResult.Cancel)
			{
			}
		}

		private void OnClickRemoveButton(object sender, ListViewColumnMouseEventArgs e)
		{
			var item = (DecorationListViewItem)e.Item;
			if (item == null) 
			{
				return;
			}
			item.RemoveItem();
		}
		#endregion UIEvent-method

		#endregion private-field
	}
}
