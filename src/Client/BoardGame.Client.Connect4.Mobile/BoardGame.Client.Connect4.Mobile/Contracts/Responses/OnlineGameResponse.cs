using BoardGame.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Client.Connect4.Mobile.Contracts.Responses
{
    [DataContract]
    public class OnlineGameResponse
    {
        [DataMember]
        public bool IsReady { get; set; }

        [DataMember]
        public int PlayerId { get; set; }

        public OnlineGameResponse(int playerId = 0, bool ready = false)
        {
            IsReady = ready;
            PlayerId = playerId;
        }
    }
}
