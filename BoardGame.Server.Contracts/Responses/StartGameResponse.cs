using BoardGame.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.Contracts.Responses
{
    [DataContract]
    public class StartGameResponse
    {
        [DataMember]
        public GameState State { get; set; }

        [DataMember]
        public int GameId { get; set; }

        [DataMember]
        public bool YourTurn { get; set; }

        public StartGameResponse(GameState state, int gameId, bool yourTurn)
        {
            State = state;
            GameId = gameId;
            YourTurn = yourTurn;
        }
    }
}
