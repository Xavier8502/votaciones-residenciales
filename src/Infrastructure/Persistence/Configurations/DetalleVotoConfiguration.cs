using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class DetalleVotoConfiguration : IEntityTypeConfiguration<DetalleVoto>
    {
        public void Configure(EntityTypeBuilder<DetalleVoto> builder)
        {
            builder.ToTable("DetallesVoto");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.PreguntaId)
                .IsRequired();

            builder.Property(d => d.OpcionId)
                .IsRequired();

            // Un voto no puede responder la misma pregunta dos veces
            builder.HasIndex(d => new { d.VotoId, d.PreguntaId })
                .IsUnique();
        }
    }
}
