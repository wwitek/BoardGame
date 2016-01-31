using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain
{
    internal static class Requires
    {
        internal static void IsNotNull(object instance, string paramName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
