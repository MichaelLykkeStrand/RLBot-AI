
namespace Bot.UI
{
    partial class BotTrainerForm
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
            this.submitButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.statePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.TrainAI = new System.Windows.Forms.Button();
            this.LoadAI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(340, 50);
            this.submitButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(100, 28);
            this.submitButton.TabIndex = 1;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(340, 15);
            this.playButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(100, 28);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "Play Scenario";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // statePanel
            // 
            this.statePanel.Location = new System.Drawing.Point(16, 15);
            this.statePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statePanel.Name = "statePanel";
            this.statePanel.Size = new System.Drawing.Size(316, 412);
            this.statePanel.TabIndex = 3;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(340, 86);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(100, 28);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // TrainAI
            // 
            this.TrainAI.Location = new System.Drawing.Point(340, 403);
            this.TrainAI.Name = "TrainAI";
            this.TrainAI.Size = new System.Drawing.Size(100, 23);
            this.TrainAI.TabIndex = 5;
            this.TrainAI.Text = "Train AI";
            this.TrainAI.UseVisualStyleBackColor = true;
            this.TrainAI.Click += new System.EventHandler(this.TrainAI_Click);
            // 
            // LoadAI
            // 
            this.LoadAI.Location = new System.Drawing.Point(340, 374);
            this.LoadAI.Name = "LoadAI";
            this.LoadAI.Size = new System.Drawing.Size(100, 23);
            this.LoadAI.TabIndex = 6;
            this.LoadAI.Text = "Load AI";
            this.LoadAI.UseVisualStyleBackColor = true;
            this.LoadAI.Click += new System.EventHandler(this.LoadAI_Click);
            // 
            // BotTrainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 442);
            this.Controls.Add(this.LoadAI);
            this.Controls.Add(this.TrainAI);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.statePanel);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.submitButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BotTrainerForm";
            this.Text = "BotTrainer";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.FlowLayoutPanel statePanel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button TrainAI;
        private System.Windows.Forms.Button LoadAI;
    }
}