using BoardGame.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Contracts.Responses
{
    [DataContract]
    public class StartGameResponse
    {
        [DataMember]
        public bool IsConfirmed { get; set; }

        [DataMember]
        public int GameId { get; set; }

        [DataMember]
        public bool YourTurn { get; set; }

        public StartGameResponse(bool isConfirmed, int gameId, bool yourTurn)
        {
            IsConfirmed = isConfirmed;
            GameId = gameId;
            YourTurn = yourTurn;
        }
    }
}
