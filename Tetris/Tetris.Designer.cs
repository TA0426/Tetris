namespace Tetris
{
    partial class Tetris
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBoxBoard;
        private PictureBox pictureBoxNext;
        private Label scoreLabel;
        private Label levelLabel;
        private Label nextLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBoxBoard = new PictureBox();
            pictureBoxNext = new PictureBox();
            scoreLabel = new Label();
            levelLabel = new Label();
            nextLabel = new Label();

            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNext).BeginInit();
            SuspendLayout();

            pictureBoxBoard.BackColor = Color.Black;
            pictureBoxBoard.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxBoard.Location = new Point(12, 12);
            pictureBoxBoard.Name = "pictureBoxBoard";
            pictureBoxBoard.Size = new Size(300, 600);
            pictureBoxBoard.TabStop = false;

            scoreLabel.AutoSize = true;
            scoreLabel.Font = new Font("Segoe UI", 12F);
            scoreLabel.Location = new Point(350, 20);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(90, 21);
            scoreLabel.Text = "Score: 0";

            levelLabel.AutoSize = true;
            levelLabel.Font = new Font("Segoe UI", 12F);
            levelLabel.Location = new Point(350, 52);
            levelLabel.Name = "levelLabel";
            levelLabel.Size = new Size(90, 21);
            levelLabel.Text = "Level: 0";

            nextLabel.AutoSize = true;
            nextLabel.Font = new Font("Segoe UI", 12F);
            nextLabel.Location = new Point(350, 100);
            nextLabel.Name = "nextLabel";
            nextLabel.Size = new Size(50, 21);
            nextLabel.Text = "Next";

            pictureBoxNext.BackColor = Color.Black;
            pictureBoxNext.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxNext.Location = new Point(350, 130);
            pictureBoxNext.Name = "pictureBoxNext";
            pictureBoxNext.Size = new Size(180, 180);
            pictureBoxNext.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxNext.TabStop = false;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(540, 650);
            Controls.Add(pictureBoxBoard);
            Controls.Add(pictureBoxNext);
            Controls.Add(scoreLabel);
            Controls.Add(levelLabel);
            Controls.Add(nextLabel);
            Name = "Tetris";
            Text = "Tetris";

            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNext).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
