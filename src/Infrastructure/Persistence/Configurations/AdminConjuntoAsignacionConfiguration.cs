
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotacionesResidenciales.Domain.Entities;

namespace VotacionesResidenciales.Infrastructure.Persistence.Configurations
{
    public class AdminConjuntoAsignacionConfiguration
        : IEntityTypeConfiguration<AdminConjuntoAsignacion>
    {
        public void Configure(
            EntityTypeBuilder<AdminConjuntoAsignacion> builder)
        {
            builder.ToTable("AdminConjuntoAsignaciones");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AdminConjuntoId)
                .IsRequired();

            builder.Property(a => a.ConjuntoId)
                .IsRequired();

            builder.HasIndex(a => new
            {
                a.AdminConjuntoId,
                a.ConjuntoId
            })
                .IsUnique();

            builder.HasOne(a => a.AdminConjunto)
                .WithMany(admin => admin.Asignaciones)
                .HasForeignKey(a => a.AdminConjuntoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Conjunto)
                .WithMany()
                .HasForeignKey(a => a.ConjuntoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
