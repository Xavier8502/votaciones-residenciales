using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class ConjuntoConfiguration : IEntityTypeConfiguration<Conjunto>
    {
        public void Configure(EntityTypeBuilder<Conjunto> builder)
        {
            builder.ToTable("Conjuntos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Nit)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(c => c.Nit)
                .IsUnique();

            builder.Property(c => c.Direccion)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.Ciudad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Telefono)
                .HasMaxLength(20);

            builder.Property(c => c.Email)
                .HasMaxLength(150);

            builder.Property(c => c.CreadoEn)
                .IsRequired();

            builder.Property(c => c.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(c => c.Inmuebles)
                .WithOne(i => i.Conjunto)
                .HasForeignKey(i => i.ConjuntoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
