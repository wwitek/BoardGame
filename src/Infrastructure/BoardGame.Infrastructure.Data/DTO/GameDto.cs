using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Infrastructure.Data.Interfaces;

namespace BoardGame.Infrastructure.Data.DTO
{
    public class GameDto : IDto
    {
        public int Id { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime FinishDate { get; set; }
    }
}
