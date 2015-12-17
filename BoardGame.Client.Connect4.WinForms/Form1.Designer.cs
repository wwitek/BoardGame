namespace BoardGame.Client.Connect4.WinForms
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lPlayer1Score = new System.Windows.Forms.Label();
            this.lPlayer2Score = new System.Windows.Forms.Label();
            this.bSinglePlayer = new System.Windows.Forms.Button();
            this.bTwoPlayers = new System.Windows.Forms.Button();
            this.bOnline = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.3F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 79);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(700, 600);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.tableLayoutPanel1_Click);
            // 
            // lPlayer1Score
            // 
            this.lPlayer1Score.AutoSize = true;
            this.lPlayer1Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPlayer1Score.ForeColor = System.Drawing.Color.Blue;
            this.lPlayer1Score.Location = new System.Drawing.Point(12, 10);
            this.lPlayer1Score.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPlayer1Score.Name = "lPlayer1Score";
            this.lPlayer1Score.Size = new System.Drawing.Size(58, 63);
            this.lPlayer1Score.TabIndex = 2;
            this.lPlayer1Score.Text = "0";
            this.lPlayer1Score.Visible = false;
            // 
            // lPlayer2Score
            // 
            this.lPlayer2Score.AutoSize = true;
            this.lPlayer2Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPlayer2Score.ForeColor = System.Drawing.Color.Red;
            this.lPlayer2Score.Location = new System.Drawing.Point(630, 10);
            this.lPlayer2Score.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPlayer2Score.Name = "lPlayer2Score";
            this.lPlayer2Score.Size = new System.Drawing.Size(58, 63);
            this.lPlayer2Score.TabIndex = 3;
            this.lPlayer2Score.Text = "0";
            this.lPlayer2Score.Visible = false;
            // 
            // bSinglePlayer
            // 
            this.bSinglePlayer.Location = new System.Drawing.Point(110, 10);
            this.bSinglePlayer.Margin = new System.Windows.Forms.Padding(4);
            this.bSinglePlayer.Name = "bSinglePlayer";
            this.bSinglePlayer.Size = new System.Drawing.Size(150, 50);
            this.bSinglePlayer.TabIndex = 4;
            this.bSinglePlayer.Text = "Single player";
            this.bSinglePlayer.UseVisualStyleBackColor = true;
            this.bSinglePlayer.Click += new System.EventHandler(this.bSinglePlayer_Click);
            // 
            // bTwoPlayers
            // 
            this.bTwoPlayers.Location = new System.Drawing.Point(268, 10);
            this.bTwoPlayers.Margin = new System.Windows.Forms.Padding(4);
            this.bTwoPlayers.Name = "bTwoPlayers";
            this.bTwoPlayers.Size = new System.Drawing.Size(150, 50);
            this.bTwoPlayers.TabIndex = 5;
            this.bTwoPlayers.Text = "Two players";
            this.bTwoPlayers.UseVisualStyleBackColor = true;
            this.bTwoPlayers.Click += new System.EventHandler(this.bTwoPlayers_Click);
            // 
            // bOnline
            // 
            this.bOnline.Location = new System.Drawing.Point(426, 10);
            this.bOnline.Margin = new System.Windows.Forms.Padding(4);
            this.bOnline.Name = "bOnline";
            this.bOnline.Size = new System.Drawing.Size(150, 50);
            this.bOnline.TabIndex = 6;
            this.bOnline.Text = "Online";
            this.bOnline.UseVisualStyleBackColor = true;
            this.bOnline.Click += new System.EventHandler(this.bOnline_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 679);
            this.Controls.Add(this.bOnline);
            this.Controls.Add(this.bTwoPlayers);
            this.Controls.Add(this.bSinglePlayer);
            this.Controls.Add(this.lPlayer2Score);
            this.Controls.Add(this.lPlayer1Score);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lPlayer1Score;
        private System.Windows.Forms.Label lPlayer2Score;
        private System.Windows.Forms.Button bSinglePlayer;
        private System.Windows.Forms.Button bTwoPlayers;
        private System.Windows.Forms.Button bOnline;
    }
}

