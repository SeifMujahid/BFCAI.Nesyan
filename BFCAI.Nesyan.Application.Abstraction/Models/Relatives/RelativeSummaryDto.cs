using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Relatives
{
    public class RelativeSummaryDto
    {
        public int RelativeId { get; set; }

        public string FullName { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
