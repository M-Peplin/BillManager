using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillManager.Models.ModelConfiguration
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne<ApplicationUser>(u => u.User).WithMany(b => b.Bills).HasForeignKey(b => b.UserId);
        }
    }
}
