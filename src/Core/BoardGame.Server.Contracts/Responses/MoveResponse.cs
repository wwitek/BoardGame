using BoardGame.Domain.Entities;
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
    [KnownType(typeof(Move))]
    [KnownType(typeof(Field))]
    public class MoveResponse
    {
        [DataMember]
        public IMove MoveMade { get; set; }

        [DataMember]
        public bool Timeout { get; set; }

        public MoveResponse(IMove moveMade, bool timeout = false)
        {
            MoveMade = moveMade;
            Timeout = timeout;

            if (!timeout) MoveMade = null;
        }
    }
}
