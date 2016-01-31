using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Entities;

namespace BoardGame.Domain.Factories
{
    public class FieldFactory : IFieldFactory
    {
        public IField Create(int row, int column)
        {
            return new Field(row, column);
        }
    }
}
