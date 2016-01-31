using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Infrastructure.Data.Interfaces;

namespace BoardGame.Infrastructure.Data.DTO
{
    public class PlayerDto : IDto
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
    }
}
