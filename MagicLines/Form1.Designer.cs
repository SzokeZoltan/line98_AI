namespace MagicLines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelJoc = new System.Windows.Forms.Panel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxScor = new System.Windows.Forms.TextBox();
            this.labelScor = new System.Windows.Forms.Label();
            this.buttonHighScores = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkLayer1 = new System.Windows.Forms.CheckBox();
            this.checkLayer2 = new System.Windows.Forms.CheckBox();
            this.checkLayer3 = new System.Windows.Forms.CheckBox();
            this.checkLayer4 = new System.Windows.Forms.CheckBox();
            this.checkLayer5 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_autotrigger = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panelJoc
            // 
            this.panelJoc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelJoc.BackgroundImage")));
            this.panelJoc.Location = new System.Drawing.Point(12, 10);
            this.panelJoc.Name = "panelJoc";
            this.panelJoc.Size = new System.Drawing.Size(368, 370);
            this.panelJoc.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(395, 10);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(61, 40);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "New game";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxScor
            // 
            this.textBoxScor.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.textBoxScor.Enabled = false;
            this.textBoxScor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxScor.Location = new System.Drawing.Point(395, 115);
            this.textBoxScor.Name = "textBoxScor";
            this.textBoxScor.Size = new System.Drawing.Size(61, 20);
            this.textBoxScor.TabIndex = 4;
            this.textBoxScor.Text = "0";
            // 
            // labelScor
            // 
            this.labelScor.AutoSize = true;
            this.labelScor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScor.Location = new System.Drawing.Point(405, 99);
            this.labelScor.Name = "labelScor";
            this.labelScor.Size = new System.Drawing.Size(40, 13);
            this.labelScor.TabIndex = 5;
            this.labelScor.Text = "Score";
            // 
            // buttonHighScores
            // 
            this.buttonHighScores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHighScores.Location = new System.Drawing.Point(395, 141);
            this.buttonHighScores.Name = "buttonHighScores";
            this.buttonHighScores.Size = new System.Drawing.Size(61, 40);
            this.buttonHighScores.TabIndex = 6;
            this.buttonHighScores.Text = "High Scores";
            this.buttonHighScores.UseVisualStyleBackColor = true;
            this.buttonHighScores.Click += new System.EventHandler(this.buttonHighScores_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(395, 223);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 40);
            this.button1.TabIndex = 8;
            this.button1.Text = "Auto Step";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkLayer1
            // 
            this.checkLayer1.AutoSize = true;
            this.checkLayer1.Checked = true;
            this.checkLayer1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLayer1.Location = new System.Drawing.Point(493, 223);
            this.checkLayer1.Name = "checkLayer1";
            this.checkLayer1.Size = new System.Drawing.Size(108, 17);
            this.checkLayer1.TabIndex = 9;
            this.checkLayer1.Text = "Layer 1 activated";
            this.checkLayer1.UseVisualStyleBackColor = true;
            this.checkLayer1.CheckedChanged += new System.EventHandler(this.checkLayer1_CheckedChanged);
            // 
            // checkLayer2
            // 
            this.checkLayer2.AutoSize = true;
            this.checkLayer2.Checked = true;
            this.checkLayer2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLayer2.Location = new System.Drawing.Point(493, 246);
            this.checkLayer2.Name = "checkLayer2";
            this.checkLayer2.Size = new System.Drawing.Size(108, 17);
            this.checkLayer2.TabIndex = 10;
            this.checkLayer2.Text = "Layer 2 activated";
            this.checkLayer2.UseVisualStyleBackColor = true;
            this.checkLayer2.CheckedChanged += new System.EventHandler(this.checkLayer2_CheckedChanged);
            // 
            // checkLayer3
            // 
            this.checkLayer3.AutoSize = true;
            this.checkLayer3.Checked = true;
            this.checkLayer3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLayer3.Location = new System.Drawing.Point(493, 269);
            this.checkLayer3.Name = "checkLayer3";
            this.checkLayer3.Size = new System.Drawing.Size(108, 17);
            this.checkLayer3.TabIndex = 11;
            this.checkLayer3.Text = "Layer 3 activated";
            this.checkLayer3.UseVisualStyleBackColor = true;
            this.checkLayer3.CheckedChanged += new System.EventHandler(this.checkLayer3_CheckedChanged);
            // 
            // checkLayer4
            // 
            this.checkLayer4.AutoSize = true;
            this.checkLayer4.Checked = true;
            this.checkLayer4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLayer4.Location = new System.Drawing.Point(493, 292);
            this.checkLayer4.Name = "checkLayer4";
            this.checkLayer4.Size = new System.Drawing.Size(108, 17);
            this.checkLayer4.TabIndex = 12;
            this.checkLayer4.Text = "Layer 4 activated";
            this.checkLayer4.UseVisualStyleBackColor = true;
            this.checkLayer4.CheckedChanged += new System.EventHandler(this.checkLayer4_CheckedChanged);
            // 
            // checkLayer5
            // 
            this.checkLayer5.AutoSize = true;
            this.checkLayer5.Checked = true;
            this.checkLayer5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLayer5.Location = new System.Drawing.Point(493, 315);
            this.checkLayer5.Name = "checkLayer5";
            this.checkLayer5.Size = new System.Drawing.Size(108, 17);
            this.checkLayer5.TabIndex = 13;
            this.checkLayer5.Text = "Layer 5 activated";
            this.checkLayer5.UseVisualStyleBackColor = true;
            this.checkLayer5.CheckedChanged += new System.EventHandler(this.checkLayer5_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2",
            "5",
            "10",
            "20",
            "50",
            "100",
            "200"});
            this.comboBox1.Location = new System.Drawing.Point(470, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(59, 21);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.Text = "100";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(467, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "step speed:";
            // 
            // checkBox_autotrigger
            // 
            this.checkBox_autotrigger.AutoSize = true;
            this.checkBox_autotrigger.Location = new System.Drawing.Point(395, 275);
            this.checkBox_autotrigger.Name = "checkBox_autotrigger";
            this.checkBox_autotrigger.Size = new System.Drawing.Size(84, 17);
            this.checkBox_autotrigger.TabIndex = 16;
            this.checkBox_autotrigger.Text = "Auto Trigger";
            this.checkBox_autotrigger.UseVisualStyleBackColor = true;
            this.checkBox_autotrigger.CheckedChanged += new System.EventHandler(this.checkBox_autotrigger_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(613, 391);
            this.Controls.Add(this.checkBox_autotrigger);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkLayer5);
            this.Controls.Add(this.checkLayer4);
            this.Controls.Add(this.checkLayer3);
            this.Controls.Add(this.checkLayer2);
            this.Controls.Add(this.checkLayer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonHighScores);
            this.Controls.Add(this.labelScor);
            this.Controls.Add(this.textBoxScor);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.panelJoc);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Magic Lines";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelJoc;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxScor;
        private System.Windows.Forms.Label labelScor;
        private System.Windows.Forms.Button buttonHighScores;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkLayer1;
        private System.Windows.Forms.CheckBox checkLayer2;
        private System.Windows.Forms.CheckBox checkLayer3;
        private System.Windows.Forms.CheckBox checkLayer4;
        private System.Windows.Forms.CheckBox checkLayer5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_autotrigger;
    }
}

