using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class PreguntaConfiguration : IEntityTypeConfiguration<Pregunta>
    {
        public void Configure(EntityTypeBuilder<Pregunta> builder)
        {
            builder.ToTable("Preguntas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Texto)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Orden)
                .IsRequired();

            builder.HasMany(p => p.Opciones)
                .WithOne()
                .HasForeignKey(o => o.PreguntaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
