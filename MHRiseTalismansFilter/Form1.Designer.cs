
namespace MHRiseTalismansFilter
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._skill_1_ComboBox = new System.Windows.Forms.ComboBox();
			this._skill_1_LevelComboBox = new System.Windows.Forms.ComboBox();
			this._skill_2_LevelComboBox = new System.Windows.Forms.ComboBox();
			this._skill_2_ComboBox = new System.Windows.Forms.ComboBox();
			this.AddButton = new System.Windows.Forms.Button();
			this._slot1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this._slot2 = new System.Windows.Forms.ComboBox();
			this._slot3 = new System.Windows.Forms.ComboBox();
			this._nameComboBox = new System.Windows.Forms.ComboBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this._decorationView = new System.Windows.Forms.ListView();
			this._filtButton = new System.Windows.Forms.Button();
			this._saveButton = new System.Windows.Forms.Button();
			this._loadButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _skill_1_ComboBox
			// 
			this._skill_1_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._skill_1_ComboBox.FormattingEnabled = true;
			this._skill_1_ComboBox.Location = new System.Drawing.Point(145, 12);
			this._skill_1_ComboBox.Name = "_skill_1_ComboBox";
			this._skill_1_ComboBox.Size = new System.Drawing.Size(121, 20);
			this._skill_1_ComboBox.TabIndex = 0;
			this._skill_1_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Skill1ComboBox_SelectedIndexChanged);
			// 
			// _skill_1_LevelComboBox
			// 
			this._skill_1_LevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._skill_1_LevelComboBox.FormattingEnabled = true;
			this._skill_1_LevelComboBox.Location = new System.Drawing.Point(272, 12);
			this._skill_1_LevelComboBox.Name = "_skill_1_LevelComboBox";
			this._skill_1_LevelComboBox.Size = new System.Drawing.Size(41, 20);
			this._skill_1_LevelComboBox.TabIndex = 1;
			// 
			// _skill_2_LevelComboBox
			// 
			this._skill_2_LevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._skill_2_LevelComboBox.FormattingEnabled = true;
			this._skill_2_LevelComboBox.Location = new System.Drawing.Point(446, 12);
			this._skill_2_LevelComboBox.Name = "_skill_2_LevelComboBox";
			this._skill_2_LevelComboBox.Size = new System.Drawing.Size(41, 20);
			this._skill_2_LevelComboBox.TabIndex = 3;
			// 
			// _skill_2_ComboBox
			// 
			this._skill_2_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._skill_2_ComboBox.FormattingEnabled = true;
			this._skill_2_ComboBox.Location = new System.Drawing.Point(319, 12);
			this._skill_2_ComboBox.Name = "_skill_2_ComboBox";
			this._skill_2_ComboBox.Size = new System.Drawing.Size(121, 20);
			this._skill_2_ComboBox.TabIndex = 2;
			this._skill_2_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Skill2ComboBox_SelectedIndexChanged);
			// 
			// AddButton
			// 
			this.AddButton.Location = new System.Drawing.Point(698, 10);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(75, 23);
			this.AddButton.TabIndex = 4;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// _slot1
			// 
			this._slot1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._slot1.FormattingEnabled = true;
			this._slot1.Location = new System.Drawing.Point(554, 12);
			this._slot1.Name = "_slot1";
			this._slot1.Size = new System.Drawing.Size(39, 20);
			this._slot1.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(515, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 12);
			this.label1.TabIndex = 8;
			this.label1.Text = "Slot";
			// 
			// _slot2
			// 
			this._slot2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._slot2.FormattingEnabled = true;
			this._slot2.Location = new System.Drawing.Point(599, 12);
			this._slot2.Name = "_slot2";
			this._slot2.Size = new System.Drawing.Size(39, 20);
			this._slot2.TabIndex = 9;
			// 
			// _slot3
			// 
			this._slot3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._slot3.FormattingEnabled = true;
			this._slot3.Location = new System.Drawing.Point(644, 12);
			this._slot3.Name = "_slot3";
			this._slot3.Size = new System.Drawing.Size(39, 20);
			this._slot3.TabIndex = 10;
			// 
			// _nameComboBox
			// 
			this._nameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._nameComboBox.FormattingEnabled = true;
			this._nameComboBox.Location = new System.Drawing.Point(12, 12);
			this._nameComboBox.Name = "_nameComboBox";
			this._nameComboBox.Size = new System.Drawing.Size(121, 20);
			this._nameComboBox.TabIndex = 11;
			// 
			// _decorationView
			// 
			this._decorationView.HideSelection = false;
			this._decorationView.Location = new System.Drawing.Point(12, 48);
			this._decorationView.Name = "_decorationView";
			this._decorationView.Size = new System.Drawing.Size(670, 373);
			this._decorationView.TabIndex = 12;
			this._decorationView.UseCompatibleStateImageBehavior = false;
			// 
			// _filtButton
			// 
			this._filtButton.Location = new System.Drawing.Point(698, 398);
			this._filtButton.Name = "_filtButton";
			this._filtButton.Size = new System.Drawing.Size(75, 23);
			this._filtButton.TabIndex = 13;
			this._filtButton.Text = "Filt";
			this._filtButton.UseVisualStyleBackColor = true;
			this._filtButton.Click += new System.EventHandler(this.OnClickFiltButton);
			// 
			// _saveButton
			// 
			this._saveButton.Location = new System.Drawing.Point(698, 48);
			this._saveButton.Name = "_saveButton";
			this._saveButton.Size = new System.Drawing.Size(75, 23);
			this._saveButton.TabIndex = 14;
			this._saveButton.Text = "Save";
			this._saveButton.UseVisualStyleBackColor = true;
			// 
			// _loadButton
			// 
			this._loadButton.Location = new System.Drawing.Point(698, 77);
			this._loadButton.Name = "_loadButton";
			this._loadButton.Size = new System.Drawing.Size(75, 23);
			this._loadButton.TabIndex = 15;
			this._loadButton.Text = "Load";
			this._loadButton.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this._loadButton);
			this.Controls.Add(this._saveButton);
			this.Controls.Add(this._filtButton);
			this.Controls.Add(this._decorationView);
			this.Controls.Add(this._nameComboBox);
			this.Controls.Add(this._slot3);
			this.Controls.Add(this._slot2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._slot1);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this._skill_2_LevelComboBox);
			this.Controls.Add(this._skill_2_ComboBox);
			this.Controls.Add(this._skill_1_LevelComboBox);
			this.Controls.Add(this._skill_1_ComboBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox _skill_1_ComboBox;
		private System.Windows.Forms.ComboBox _skill_1_LevelComboBox;
		private System.Windows.Forms.ComboBox _skill_2_ComboBox;
		private System.Windows.Forms.ComboBox _skill_2_LevelComboBox;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.ComboBox _slot1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox _slot2;
		private System.Windows.Forms.ComboBox _slot3;
		private System.Windows.Forms.ComboBox _nameComboBox;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.ListView _decorationView;
		private System.Windows.Forms.Button _filtButton;
		private System.Windows.Forms.Button _saveButton;
		private System.Windows.Forms.Button _loadButton;
	}
}

