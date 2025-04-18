﻿namespace domino
{
    partial class GameForm
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
            this.rulesButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rulesButton
            // 
            this.rulesButton.Location = new System.Drawing.Point(1149, 585);
            this.rulesButton.Name = "rulesButton";
            this.rulesButton.Size = new System.Drawing.Size(103, 32);
            this.rulesButton.TabIndex = 0;
            this.rulesButton.Text = "Правила (F1)";
            this.rulesButton.UseVisualStyleBackColor = true;
            this.rulesButton.Click += new System.EventHandler(this.rulesButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(1149, 547);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(103, 32);
            this.restartButton.TabIndex = 1;
            this.restartButton.Text = "Рестарт (R)";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(1149, 509);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(103, 32);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Загрузить (L)";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(1149, 471);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(103, 32);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Сохранить (S)";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.rulesButton);
            this.Name = "GameForm";
            this.Text = "Игра \"Домино\"";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button rulesButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
    }
}