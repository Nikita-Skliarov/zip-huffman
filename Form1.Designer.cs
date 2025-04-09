namespace zip
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ZipButton = new Button();
            UnzipButton = new Button();
            SuspendLayout();
            // 
            // ZipButton
            // 
            ZipButton.Location = new Point(22, 49);
            ZipButton.Name = "ZipButton";
            ZipButton.Size = new Size(87, 37);
            ZipButton.TabIndex = 0;
            ZipButton.Text = "ZIP";
            ZipButton.UseVisualStyleBackColor = true;
            ZipButton.Click += ZipButton_Click;
            // 
            // UnzipButton
            // 
            UnzipButton.Location = new Point(204, 49);
            UnzipButton.Name = "UnzipButton";
            UnzipButton.Size = new Size(86, 37);
            UnzipButton.TabIndex = 1;
            UnzipButton.Text = "UNZIP";
            UnzipButton.UseVisualStyleBackColor = true;
            UnzipButton.Click += UnzipButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(321, 165);
            Controls.Add(UnzipButton);
            Controls.Add(ZipButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button ZipButton;
        private Button UnzipButton;
    }
}
