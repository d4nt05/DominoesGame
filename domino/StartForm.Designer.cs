namespace domino
{
    partial class StartForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb4Players = new System.Windows.Forms.RadioButton();
            this.rb3Players = new System.Windows.Forms.RadioButton();
            this.rb2Players = new System.Windows.Forms.RadioButton();
            this.cbDifficulty = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb4Players);
            this.groupBox1.Controls.Add(this.rb3Players);
            this.groupBox1.Controls.Add(this.rb2Players);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите количество игроков";
            // 
            // rb4Players
            // 
            this.rb4Players.AutoSize = true;
            this.rb4Players.Location = new System.Drawing.Point(18, 77);
            this.rb4Players.Name = "rb4Players";
            this.rb4Players.Size = new System.Drawing.Size(69, 17);
            this.rb4Players.TabIndex = 2;
            this.rb4Players.TabStop = true;
            this.rb4Players.Text = "4 игрока";
            this.rb4Players.UseVisualStyleBackColor = true;
            // 
            // rb3Players
            // 
            this.rb3Players.AutoSize = true;
            this.rb3Players.Location = new System.Drawing.Point(18, 54);
            this.rb3Players.Name = "rb3Players";
            this.rb3Players.Size = new System.Drawing.Size(69, 17);
            this.rb3Players.TabIndex = 1;
            this.rb3Players.TabStop = true;
            this.rb3Players.Text = "3 игрока";
            this.rb3Players.UseVisualStyleBackColor = true;
            // 
            // rb2Players
            // 
            this.rb2Players.AutoSize = true;
            this.rb2Players.Location = new System.Drawing.Point(18, 31);
            this.rb2Players.Name = "rb2Players";
            this.rb2Players.Size = new System.Drawing.Size(69, 17);
            this.rb2Players.TabIndex = 0;
            this.rb2Players.TabStop = true;
            this.rb2Players.Text = "2 игрока";
            this.rb2Players.UseVisualStyleBackColor = true;
            // 
            // cbDifficulty
            // 
            this.cbDifficulty.FormattingEnabled = true;
            this.cbDifficulty.Items.AddRange(new object[] {
            "Легкий",
            "Средний",
            "Сложный"});
            this.cbDifficulty.Location = new System.Drawing.Point(47, 168);
            this.cbDifficulty.Name = "cbDifficulty";
            this.cbDifficulty.Size = new System.Drawing.Size(131, 21);
            this.cbDifficulty.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(47, 222);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(130, 42);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Начать игру";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 302);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cbDifficulty);
            this.Controls.Add(this.groupBox1);
            this.Name = "StartForm";
            this.Text = "Новая игра";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb4Players;
        private System.Windows.Forms.RadioButton rb3Players;
        private System.Windows.Forms.RadioButton rb2Players;
        private System.Windows.Forms.ComboBox cbDifficulty;
        private System.Windows.Forms.Button btnStart;
    }
}