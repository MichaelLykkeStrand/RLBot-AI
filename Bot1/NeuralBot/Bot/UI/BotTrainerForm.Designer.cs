
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
            this.groupBoxStates = new System.Windows.Forms.GroupBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupBoxStates
            // 
            this.groupBoxStates.AutoSize = true;
            this.groupBoxStates.Location = new System.Drawing.Point(12, 12);
            this.groupBoxStates.Name = "groupBoxStates";
            this.groupBoxStates.Size = new System.Drawing.Size(206, 405);
            this.groupBoxStates.TabIndex = 0;
            this.groupBoxStates.TabStop = false;
            this.groupBoxStates.Text = "States";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(713, 415);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 1;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(255, 12);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(127, 23);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "Play Scenario";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // BotTrainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.groupBoxStates);
            this.Name = "BotTrainerForm";
            this.Text = "BotTrainer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStates;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button playButton;
    }
}