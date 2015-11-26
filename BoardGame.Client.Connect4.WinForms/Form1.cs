using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;

namespace BoardGame.Client.Connect4.WinForms
{
    public partial class Form1 : Form
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public Form1(IGameFactory gameFactory = null, IPlayerFactory playerFactory = null)
        {
            InitializeComponent();

            GameFactory = gameFactory;
            PlayerFactory = playerFactory;

            InitializeBoard();
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            if (GameFactory == null || PlayerFactory == null) return;

            int clickedColumn = GetColumnIndex(
                tableLayoutPanel1,
                tableLayoutPanel1.PointToClient(Cursor.Position));
            
            if (CurrentGame.IsMoveValid(0, clickedColumn))
            {
                IMove result = CurrentGame.MakeMove(0, clickedColumn);
                MakeMove(result);
            }
        }

        private void bSinglePlayer_Click(object sender, EventArgs e)
        {
            NewGame(GameType.SinglePlayer);
        }

        private void bTwoPlayers_Click(object sender, EventArgs e)
        {
            NewGame(GameType.TwoPlayers);
        }

 
        #region UI helpers

        private void InitializeBoard()
        {
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 1; i <= this.tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            }
            for (int i = 1; i <= this.tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }
        }
        
        private int GetColumnIndex(TableLayoutPanel tlp, Point point)
        {
            if (point.X > tlp.Width || point.Y > tlp.Height)
                return -1;

            int w = tlp.Width;
            int h = tlp.Height;
            int[] widths = tlp.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
            {
                w -= widths[i];
            }

            int col = i + 1;
            return col;
        }

        private void MakeMove(IMove result)
        {
            if (result != null && result.Row >= 0)
            {
                Panel panel1 = new Panel();
                panel1.BackColor = (result.PlayerId == 1) ? Color.Red : Color.Black;
                panel1.Name = "panel" + result.Column + result.Row;
                panel1.Size = new Size(91, 91);
                panel1.TabIndex = 0;
                tableLayoutPanel1.Controls.Add(panel1, result.Column, result.Row);

                if (result.IsConnected)
                {
                    Color connectColor = (result.PlayerId == 1) ? Color.Purple : Color.Brown;

                    MessageBox.Show("Success! Player " + result.PlayerId.ToString() + " won!");
                }
            }

            if (result != null && result.IsTie)
            {
                MessageBox.Show("Game Over!");
                return;
            }
        }

        private void NewGame(GameType type)
        {
            if (GameFactory == null || PlayerFactory == null) return;

            var players = new List<IPlayer> {
                PlayerFactory.Create(PlayerType.Human, 1)
            };

            switch (type)
            {
                case GameType.SinglePlayer:
                    players.Add(PlayerFactory.Create(PlayerType.Bot, 2));
                    break;
                case GameType.TwoPlayers:
                    players.Add(PlayerFactory.Create(PlayerType.Human, 2));
                    break;
            }

            CurrentGame = GameFactory.Create(players);

            tableLayoutPanel1.Controls.Clear();
            lPlayer1Score.Visible = true;
            lPlayer2Score.Visible = true;
        }

        #endregion
    }
}
