using BFCAI.Nesyan.Domain.Entities.Primary.Relative;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Primary.Relatives
{
    internal class RelativeConfigurations : UserConfigurations<Relative>
    {
        public override void Configure(EntityTypeBuilder<Relative> builder)
        {
            base.Configure(builder);

            
        }
    }
}
