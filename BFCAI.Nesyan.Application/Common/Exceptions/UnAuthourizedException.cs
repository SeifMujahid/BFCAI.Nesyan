using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Common.Exceptions
{
    public class UnAuthourizedException:ApplicationException
    {
        public UnAuthourizedException(string message)
            :base(message)
        {
            
        }
    }
}
