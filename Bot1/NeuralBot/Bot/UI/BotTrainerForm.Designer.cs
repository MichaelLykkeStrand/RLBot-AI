
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
            this.SuspendLayout();
            // 
            // groupBoxStates
            // 
            this.groupBoxStates.Location = new System.Drawing.Point(12, 12);
            this.groupBoxStates.Name = "groupBoxStates";
            this.groupBoxStates.Size = new System.Drawing.Size(200, 100);
            this.groupBoxStates.TabIndex = 0;
            this.groupBoxStates.TabStop = false;
            this.groupBoxStates.Text = "States";
            // 
            // BotTrainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBoxStates);
            this.Name = "BotTrainerForm";
            this.Text = "BotTrainer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStates;
    }
}