using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Common.Exceptions
{
    public class NotFoundException:BadRequestException
    {
        public NotFoundException(string name,object key)
            :base($"{name} with {key} is not found ")
        {
            
        }
    }
}
