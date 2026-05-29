using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class OpcionConfiguration : IEntityTypeConfiguration<Opcion>
    {
        public void Configure(EntityTypeBuilder<Opcion> builder)
        {
            builder.ToTable("Opciones");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Texto)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(o => o.Orden)
                .IsRequired();
        }
    }
}
