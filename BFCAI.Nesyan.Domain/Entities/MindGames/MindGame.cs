using BFCAI.Nesyan.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.MindGames
{
    public class MindGame:BaseAuditableEntity<int>
    {
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Level { get; set; } = null!;
    }
}
