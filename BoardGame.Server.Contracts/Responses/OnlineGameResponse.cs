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
    public class OnlineGameResponse
    {
        [DataMember]
        public GameState State { get; set; }

        [DataMember]
        public int PlayerId { get; set; }

        public OnlineGameResponse(GameState state, int playerId)
        {
            State = state;
            PlayerId = playerId;
        }
    }
}
