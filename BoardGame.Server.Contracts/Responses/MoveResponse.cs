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
        public int ClickedColumn { get; set; }

        public MoveResponse(int clickedColumn)
        {
            ClickedColumn = clickedColumn;
        }
    }
}
