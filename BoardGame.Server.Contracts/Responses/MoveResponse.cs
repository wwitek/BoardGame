using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.Contracts.Responses
{
    [DataContract]
    public class MoveResponse
    {
        [DataMember]
        IMove Move { get; set; }

        public MoveResponse(IMove move)
        {
            Move = move;
        }
    }
}
