using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.MindGames
{
    internal class MindGameConfigurations: BaseEntityConfigurations<MindGame, int>
    {
        public override void Configure(EntityTypeBuilder<MindGame> builder)
        {
            base.Configure(builder);
        }
    }
}
