using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core;

public partial class BiotlabContext : DbContext
{
    public BiotlabContext()
    {
    }

    public BiotlabContext(DbContextOptions<BiotlabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anestesico> Anestesicos { get; set; }

    public virtual DbSet<Bioterio> Bioterios { get; set; }

    public virtual DbSet<Entradaanestesico> Entradaanestesicos { get; set; }

    public virtual DbSet<Entradum> Entrada { get; set; }

    public virtual DbSet<Experimento> Experimentos { get; set; }

    public virtual DbSet<Fornecedor> Fornecedors { get; set; }

    public virtual DbSet<Gaiola> Gaiolas { get; set; }

    public virtual DbSet<Gaiolaharem> Gaiolaharems { get; set; }

    public virtual DbSet<Harem> Harems { get; set; }

    public virtual DbSet<Instituicao> Instituicaos { get; set; }

    public virtual DbSet<Obituario> Obituarios { get; set; }

    public virtual DbSet<Pesquisador> Pesquisadors { get; set; }

    public virtual DbSet<Usoanestesico> Usoanestesicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anestesico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anestesico");

            entity.HasIndex(e => e.IdInstituicao, "fk_Anestesico_Instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Concentracao)
                .HasPrecision(10)
                .HasColumnName("concentracao");
            entity.Property(e => e.IdInstituicao).HasColumnName("idInstituicao");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .HasColumnName("marca");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdInstituicaoNavigation).WithMany(p => p.Anestesicos)
                .HasForeignKey(d => d.IdInstituicao)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Anestesico_Instituicao1");
        });

        modelBuilder.Entity<Bioterio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bioterio");

            entity.HasIndex(e => e.IdInstituicao, "fk_Bioterio_Instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.IdInstituicao).HasColumnName("idInstituicao");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(50)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone1)
                .HasMaxLength(15)
                .HasColumnName("telefone1");
            entity.Property(e => e.Telefone2)
                .HasMaxLength(15)
                .HasColumnName("telefone2");

            entity.HasOne(d => d.IdInstituicaoNavigation).WithMany(p => p.Bioterios)
                .HasForeignKey(d => d.IdInstituicao)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Bioterio_Instituicao1");
        });

        modelBuilder.Entity<Entradaanestesico>(entity =>
        {
            entity.HasKey(e => new { e.IdEntrada, e.IdAnestesico }).HasName("PRIMARY");

            entity.ToTable("entradaanestesico");

            entity.HasIndex(e => e.IdAnestesico, "fk_EntradaAnestesico_Anestesico1");

            entity.HasIndex(e => e.IdEntrada, "fk_EntradaAnestesico_Entrada1_idx");

            entity.Property(e => e.IdEntrada).HasColumnName("idEntrada");
            entity.Property(e => e.IdAnestesico).HasColumnName("idAnestesico");
            entity.Property(e => e.Lote)
                .HasMaxLength(50)
                .HasColumnName("lote");
            entity.Property(e => e.Quantidade)
                .HasPrecision(10)
                .HasColumnName("quantidade");
            entity.Property(e => e.SubTotal)
                .HasPrecision(10)
                .HasColumnName("subTotal");
            entity.Property(e => e.ValorUnitario)
                .HasPrecision(10)
                .HasColumnName("valorUnitario");

            entity.HasOne(d => d.IdAnestesicoNavigation).WithMany(p => p.Entradaanestesicos)
                .HasForeignKey(d => d.IdAnestesico)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_EntradaAnestesico_Anestesico1");

            entity.HasOne(d => d.IdEntradaNavigation).WithMany(p => p.Entradaanestesicos)
                .HasForeignKey(d => d.IdEntrada)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_EntradaAnestesico_Entrada1");
        });

        modelBuilder.Entity<Entradum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entrada");

            entity.HasIndex(e => e.IdFornecedor, "fk_Entrada_Fornecedor1_idx");

            entity.HasIndex(e => e.IdInstituicao, "fk_Entrada_Instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataEntrada)
                .HasColumnType("datetime")
                .HasColumnName("dataEntrada");
            entity.Property(e => e.IdFornecedor).HasColumnName("idFornecedor");
            entity.Property(e => e.IdInstituicao).HasColumnName("idInstituicao");
            entity.Property(e => e.NumeroNotaFiscal)
                .HasMaxLength(20)
                .HasColumnName("numeroNotaFiscal");

            entity.HasOne(d => d.IdFornecedorNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdFornecedor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Entrada_Fornecedor1");

            entity.HasOne(d => d.IdInstituicaoNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdInstituicao)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Entrada_Instituicao1");
        });

        modelBuilder.Entity<Experimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("experimento");

            entity.HasIndex(e => e.IdPesquisador, "fk_Experimento_Pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cepa)
                .HasMaxLength(50)
                .HasColumnName("cepa");
            entity.Property(e => e.DataFim)
                .HasColumnType("date")
                .HasColumnName("dataFim");
            entity.Property(e => e.DataInicio)
                .HasColumnType("date")
                .HasColumnName("dataInicio");
            entity.Property(e => e.IdPesquisador).HasColumnName("idPesquisador");

            entity.HasOne(d => d.IdPesquisadorNavigation).WithMany(p => p.Experimentos)
                .HasForeignKey(d => d.IdPesquisador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Experimento_Pesquisador1");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fornecedor");

            entity.HasIndex(e => e.IdInstituicao, "fk_Fornecedor_Instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.IdInstituicao).HasColumnName("idInstituicao");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(50)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone1)
                .HasMaxLength(15)
                .HasColumnName("telefone1");
            entity.Property(e => e.Telefone2)
                .HasMaxLength(15)
                .HasColumnName("telefone2");

            entity.HasOne(d => d.IdInstituicaoNavigation).WithMany(p => p.Fornecedors)
                .HasForeignKey(d => d.IdInstituicao)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Fornecedor_Instituicao1");
        });

        modelBuilder.Entity<Gaiola>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gaiola");

            entity.HasIndex(e => e.IdBioterio, "fk_Gaiola_Bioterio1_idx");

            entity.HasIndex(e => e.IdPesquisador, "fk_Gaiola_Pesquisador1_idx");

            entity.HasIndex(e => e.IdExperimento, "fk_Gaiola_experimento1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoInterno)
                .HasMaxLength(50)
                .HasColumnName("codigoInterno");
            entity.Property(e => e.Etiqueta)
                .HasMaxLength(50)
                .HasColumnName("etiqueta");
            entity.Property(e => e.IdBioterio).HasColumnName("idBioterio");
            entity.Property(e => e.IdExperimento).HasColumnName("idExperimento");
            entity.Property(e => e.IdPesquisador).HasColumnName("idPesquisador");
            entity.Property(e => e.Localizacao)
                .HasMaxLength(100)
                .HasColumnName("localizacao");
            entity.Property(e => e.NumeroFemeas).HasColumnName("numeroFemeas");
            entity.Property(e => e.NumeroMachos).HasColumnName("numeroMachos");
            entity.Property(e => e.Status)
                .HasComment("N - NOVA\nE - EXPERIMENTO SENDO REALIZADO\nF - EXPERIMENTO FINALIZADO")
                .HasColumnType("enum('N','E','F')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdBioterioNavigation).WithMany(p => p.Gaiolas)
                .HasForeignKey(d => d.IdBioterio)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Gaiola_Bioterio1");

            entity.HasOne(d => d.IdExperimentoNavigation).WithMany(p => p.Gaiolas)
                .HasForeignKey(d => d.IdExperimento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Gaiola_experimento1");

            entity.HasOne(d => d.IdPesquisadorNavigation).WithMany(p => p.Gaiolas)
                .HasForeignKey(d => d.IdPesquisador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Gaiola_Pesquisador1");
        });

        modelBuilder.Entity<Gaiolaharem>(entity =>
        {
            entity.HasKey(e => new { e.IdGaiola, e.IdHarem }).HasName("PRIMARY");

            entity.ToTable("gaiolaharem");

            entity.HasIndex(e => e.IdGaiola, "fk_GaiolaHarem_Gaiola1_idx");

            entity.HasIndex(e => e.IdHarem, "fk_GaiolaHarem_Harem1_idx");

            entity.HasIndex(e => e.IdPesquisador, "fk_GaiolaHarem_Pesquisador1_idx");

            entity.Property(e => e.IdGaiola).HasColumnName("idGaiola");
            entity.Property(e => e.IdHarem).HasColumnName("idHarem");
            entity.Property(e => e.DataPovoamento)
                .HasColumnType("datetime")
                .HasColumnName("dataPovoamento");
            entity.Property(e => e.IdPesquisador).HasColumnName("idPesquisador");

            entity.HasOne(d => d.IdGaiolaNavigation).WithMany(p => p.Gaiolaharems)
                .HasForeignKey(d => d.IdGaiola)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_GaiolaHarem_Gaiola1");

            entity.HasOne(d => d.IdHaremNavigation).WithMany(p => p.Gaiolaharems)
                .HasForeignKey(d => d.IdHarem)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_GaiolaHarem_Harem1");

            entity.HasOne(d => d.IdPesquisadorNavigation).WithMany(p => p.Gaiolaharems)
                .HasForeignKey(d => d.IdPesquisador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_GaiolaHarem_Pesquisador1");
        });

        modelBuilder.Entity<Harem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("harem");

            entity.HasIndex(e => e.CodigoInterno, "codigoInterno_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdBioterio, "fk_Harem_Bioterio1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoInterno)
                .HasMaxLength(20)
                .HasColumnName("codigoInterno");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("datetime")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.IdBioterio).HasColumnName("idBioterio");
            entity.Property(e => e.NumeroFemeas).HasColumnName("numeroFemeas");
            entity.Property(e => e.NumeroMachos).HasColumnName("numeroMachos");
            entity.Property(e => e.Status)
                .HasComment("A - ATIVO\nI - INATIVO")
                .HasColumnType("enum('A','I')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdBioterioNavigation).WithMany(p => p.Harems)
                .HasForeignKey(d => d.IdBioterio)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Harem_Bioterio1");
        });

        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("instituicao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(50)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone1)
                .HasMaxLength(15)
                .HasColumnName("telefone1");
            entity.Property(e => e.Telefone2)
                .HasMaxLength(15)
                .HasColumnName("telefone2");
        });

        modelBuilder.Entity<Obituario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("obituario");

            entity.HasIndex(e => e.IdGaiola, "fk_obituario_Gaiola1_idx");

            entity.HasIndex(e => e.IdPesquisador, "fk_obituario_Pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data)
                .HasColumnType("datetime")
                .HasColumnName("data");
            entity.Property(e => e.IdGaiola).HasColumnName("idGaiola");
            entity.Property(e => e.IdPesquisador).HasColumnName("idPesquisador");

            entity.HasOne(d => d.IdGaiolaNavigation).WithMany(p => p.Obituarios)
                .HasForeignKey(d => d.IdGaiola)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_obituario_Gaiola1");

            entity.HasOne(d => d.IdPesquisadorNavigation).WithMany(p => p.Obituarios)
                .HasForeignKey(d => d.IdPesquisador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_obituario_Pesquisador1");
        });

        modelBuilder.Entity<Pesquisador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pesquisador");

            entity.HasIndex(e => e.Cpf, "cpf_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .HasColumnName("estado");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(50)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone1)
                .HasMaxLength(15)
                .HasColumnName("telefone1");
            entity.Property(e => e.Telefone2)
                .HasMaxLength(15)
                .HasColumnName("telefone2");
        });

        modelBuilder.Entity<Usoanestesico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usoanestesico");

            entity.HasIndex(e => new { e.IdEntrada, e.IdAnestesico }, "fk_usoAnestesico_EntradaAnestesico1_idx");

            entity.HasIndex(e => e.IdExperimento, "fk_usoAnestesico_Experimento1_idx");

            entity.HasIndex(e => e.IdPesquisador, "fk_usoAnestesico_Pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cepa)
                .HasMaxLength(50)
                .HasColumnName("cepa");
            entity.Property(e => e.Data)
                .HasColumnType("datetime")
                .HasColumnName("data");
            entity.Property(e => e.IdAnestesico).HasColumnName("idAnestesico");
            entity.Property(e => e.IdEntrada).HasColumnName("idEntrada");
            entity.Property(e => e.IdExperimento).HasColumnName("idExperimento");
            entity.Property(e => e.IdPesquisador).HasColumnName("idPesquisador");
            entity.Property(e => e.NumeroAnimais).HasColumnName("numeroAnimais");
            entity.Property(e => e.Procedimento)
                .HasMaxLength(255)
                .HasColumnName("procedimento");
            entity.Property(e => e.Quantidade)
                .HasPrecision(10)
                .HasColumnName("quantidade");

            entity.HasOne(d => d.IdExperimentoNavigation).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => d.IdExperimento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_Experimento1");

            entity.HasOne(d => d.IdPesquisadorNavigation).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => d.IdPesquisador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_Pesquisador1");

            entity.HasOne(d => d.Entradaanestesico).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => new { d.IdEntrada, d.IdAnestesico })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_EntradaAnestesico1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
