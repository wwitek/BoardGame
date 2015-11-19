using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities
{
    public class Game : IGame
    {
        public IBoard Board { get; }
        public GameType Type { get; }
        public CircularQueue<IPlayer> Players { get; }
        public GameState State { get; private set; }
        public IPlayer CurrentPlayer => Players.GetCurrentItem;

        public Game(GameType type, IBoard board, CircularQueue<IPlayer> players)
        {
            Type = type;
            Board = board;
            Players = players;
            State = GameState.New;
        }

        public IMove MakeMove(int row, int column, int playerId)
        {
            if (!Board.IsMoveValid(row, column, playerId) || State == GameState.Finished) return null;

            IMove result = Board.InsertChip(row, column, playerId);
            if (result.IsConnected)
            {
                State = GameState.Finished;
                return result;
            }

            Players.SetNextItem();
            //if (!result.IsTie && !result.IsConnected && CurrentPlayer.Type.Equals(PlayerType.Bot))
            //{
            //    Bot bot = new Bot(Players.GetCurrentItem.Id, playerId, board);
            //    result.FollowingBotResult = bot.Move();
            //    if (result.Row >= 0) Players.SetNextItem();
            //}

            return result;
        }
    }
}
