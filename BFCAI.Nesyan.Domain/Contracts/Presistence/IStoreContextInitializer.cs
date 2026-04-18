using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Contracts
{
    public interface IStoreContextInitializer
    {
        Task InitalizeAsync();
    }
}
