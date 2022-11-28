using JovemProgramadorMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JovemProgramadorMVC.Data.Mapeamento
{
    public class AlunoMapping: IEntityTypeConfiguration<AlunoModel>
    {
        public void Configure(EntityTypeBuilder<AlunoModel> builder)
        {
            builder.ToTable("Aluno");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nome).HasColumnType("VARCHAR(50)");
            builder.Property(t => t.Idade).HasColumnType("INT");
            builder.Property(t => t.Contato).HasColumnType("VARCHAR(50)");
            builder.Property(t => t.Email).HasColumnType("VARCHAR(50)");
            builder.Property(t => t.Cep).HasColumnType("VARCHAR(50)");
        }
    }
}
