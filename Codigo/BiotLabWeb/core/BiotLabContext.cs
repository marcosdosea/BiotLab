using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace core;

public partial class BiotLabContext : DbContext
{
    public BiotLabContext()
    {
    }

    public BiotLabContext(DbContextOptions<BiotLabContext> options)
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

    public virtual DbSet<Harem> Harens { get; set; }

    public virtual DbSet<Instituicao> Instituicaos { get; set; }

    public virtual DbSet<Obituario> Obituarios { get; set; }

    public virtual DbSet<Pesquisador> Pesquisadors { get; set; }

    public virtual DbSet<Povoargaiola> Povoargaiolas { get; set; }

    public virtual DbSet<Usoanestesico> Usoanestesicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anestesico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anestesico");

            entity.HasIndex(e => e.InstituicaoId, "fk_anestesico_instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Concentracao).HasColumnName("concentracao");
            entity.Property(e => e.DataVencimento)
                .HasColumnType("date")
                .HasColumnName("dataVencimento");
            entity.Property(e => e.InstituicaoId).HasColumnName("instituicao_id");
            entity.Property(e => e.Marca)
                .HasMaxLength(255)
                .HasColumnName("marca");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");

            entity.HasOne(d => d.Instituicao).WithMany(p => p.Anestesicos)
                .HasForeignKey(d => d.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_anestesico_instituicao1");
        });

        modelBuilder.Entity<Bioterio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bioterio");

            entity.HasIndex(e => e.InstituicaoId, "fk_bioterio_instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(255)
                .HasColumnName("cidade");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.InstituicaoId).HasColumnName("instituicao_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Numero).HasColumnName("numero");

            entity.HasOne(d => d.Instituicao).WithMany(p => p.Bioterios)
                .HasForeignKey(d => d.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bioterio_instituicao1");
        });

        modelBuilder.Entity<Entradaanestesico>(entity =>
        {
            entity.HasKey(e => new { e.AnestesicoId, e.EntradaId }).HasName("PRIMARY");

            entity.ToTable("entradaanestesico");

            entity.HasIndex(e => e.AnestesicoId, "fk_anestesico_has_entrada_anestesico1_idx");

            entity.HasIndex(e => e.EntradaId, "fk_anestesico_has_entrada_entrada1_idx");

            entity.Property(e => e.AnestesicoId).HasColumnName("anestesico_id");
            entity.Property(e => e.EntradaId).HasColumnName("entrada_id");
            entity.Property(e => e.Lote)
                .HasMaxLength(255)
                .HasColumnName("lote");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");
            entity.Property(e => e.SubTotal).HasColumnName("subTotal");
            entity.Property(e => e.ValorUnitario).HasColumnName("valorUnitario");

            entity.HasOne(d => d.Anestesico).WithMany(p => p.Entradaanestesicos)
                .HasForeignKey(d => d.AnestesicoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_anestesico_has_entrada_anestesico1");

            entity.HasOne(d => d.Entrada).WithMany(p => p.Entradaanestesicos)
                .HasForeignKey(d => d.EntradaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_anestesico_has_entrada_entrada1");
        });

        modelBuilder.Entity<Entradum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entrada");

            entity.HasIndex(e => e.BioterioId, "fk_entrada_bioterio1_idx");

            entity.HasIndex(e => e.FornecedorId, "fk_entrada_fornecedor1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BioterioId).HasColumnName("bioterio_id");
            entity.Property(e => e.DataEntrada)
                .HasColumnType("date")
                .HasColumnName("dataEntrada");
            entity.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            entity.Property(e => e.Marca)
                .HasMaxLength(255)
                .HasColumnName("marca");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.NumeroNotaFiscal).HasColumnName("numeroNotaFiscal");

            entity.HasOne(d => d.Bioterio).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.BioterioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_entrada_bioterio1");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_entrada_fornecedor1");
        });

        modelBuilder.Entity<Experimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("experimento");

            entity.HasIndex(e => e.PesquisadorId, "fk_experimento_pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cepa).HasColumnName("cepa");
            entity.Property(e => e.DataFim)
                .HasColumnType("date")
                .HasColumnName("dataFim");
            entity.Property(e => e.DataInicio)
                .HasColumnType("date")
                .HasColumnName("dataInicio");
            entity.Property(e => e.PesquisadorId).HasColumnName("pesquisador_id");

            entity.HasOne(d => d.Pesquisador).WithMany(p => p.Experimentos)
                .HasForeignKey(d => d.PesquisadorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_experimento_pesquisador1");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fornecedor");

            entity.HasIndex(e => e.InstituicaoId, "fk_fornecedor_instituicao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CapacidadeFornecimento).HasColumnName("capacidadeFornecimento");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(255)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .HasColumnName("estado");
            entity.Property(e => e.InstituicaoId).HasColumnName("instituicao_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .HasColumnName("telefone");
            entity.Property(e => e.TipoProduto)
                .HasMaxLength(255)
                .HasColumnName("tipoProduto");

            entity.HasOne(d => d.Instituicao).WithMany(p => p.Fornecedors)
                .HasForeignKey(d => d.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_fornecedor_instituicao1");
        });

        modelBuilder.Entity<Gaiola>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gaiola");

            entity.HasIndex(e => e.BioterioId, "fk_gaiola_bioterio1_idx");

            entity.HasIndex(e => e.ExperimentoId, "fk_gaiola_experimento1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BioterioId).HasColumnName("bioterio_id");
            entity.Property(e => e.CodigoGaiola).HasColumnName("codigoGaiola");
            entity.Property(e => e.ExperimentoId).HasColumnName("experimento_id");
            entity.Property(e => e.Localizacao)
                .HasMaxLength(255)
                .HasColumnName("localizacao");
            entity.Property(e => e.NumeroRatos).HasColumnName("numeroRatos");
            entity.Property(e => e.Status)
                .HasComment("N - NOVA\nE - EXPERIMENTO SENDO REALIZADO\nF - EXPERIMENTO FINALIZADO")
                .HasColumnType("enum('N','E','F')")
                .HasColumnName("status");

            entity.HasOne(d => d.Bioterio).WithMany(p => p.Gaiolas)
                .HasForeignKey(d => d.BioterioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_gaiola_bioterio1");

            entity.HasOne(d => d.Experimento).WithMany(p => p.Gaiolas)
                .HasForeignKey(d => d.ExperimentoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_gaiola_experimento1");
        });

        modelBuilder.Entity<Harem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("harem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NumeroFemea).HasColumnName("numeroFemea");
            entity.Property(e => e.NumeroMacho).HasColumnName("numeroMacho");
            entity.Property(e => e.Status)
                .HasComment("A - ATIVO\nI - INATIVO")
                .HasColumnType("enum('A','I')")
                .HasColumnName("status");

            entity.HasMany(d => d.Gaiolas).WithMany(p => p.Harens)
                .UsingEntity<Dictionary<string, object>>(
                    "Haremgaiola",
                    r => r.HasOne<Gaiola>().WithMany()
                        .HasForeignKey("GaiolaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_harem_has_gaiola_gaiola1"),
                    l => l.HasOne<Harem>().WithMany()
                        .HasForeignKey("HaremId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_harem_has_gaiola_harem1"),
                    j =>
                    {
                        j.HasKey("HaremId", "GaiolaId").HasName("PRIMARY");
                        j.ToTable("haremgaiola");
                        j.HasIndex(new[] { "GaiolaId" }, "fk_harem_has_gaiola_gaiola1_idx");
                        j.HasIndex(new[] { "HaremId" }, "fk_harem_has_gaiola_harem1_idx");
                        j.IndexerProperty<int>("HaremId").HasColumnName("harem_id");
                        j.IndexerProperty<int>("GaiolaId").HasColumnName("gaiola_id");
                    });
        });

        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("instituicao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .HasColumnName("estado");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Obituario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("obituario");

            entity.HasIndex(e => e.GaiolaId, "fk_obituario_gaiola1_idx");

            entity.HasIndex(e => e.PesquisadorId, "fk_obituario_pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataFalecimento)
                .HasMaxLength(45)
                .HasColumnName("dataFalecimento");
            entity.Property(e => e.GaiolaId).HasColumnName("gaiola_id");
            entity.Property(e => e.PesquisadorId).HasColumnName("pesquisador_id");

            entity.HasOne(d => d.Gaiola).WithMany(p => p.Obituarios)
                .HasForeignKey(d => d.GaiolaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_obituario_gaiola1");

            entity.HasOne(d => d.Pesquisador).WithMany(p => p.Obituarios)
                .HasForeignKey(d => d.PesquisadorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_obituario_pesquisador1");
        });

        modelBuilder.Entity<Pesquisador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pesquisador");

            entity.HasIndex(e => e.BioterioId, "fk_pesquisador_bioterio1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BioterioId).HasColumnName("bioterio_id");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(45)
                .HasColumnName("cidade");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Senha)
                .HasMaxLength(32)
                .HasColumnName("senha");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .HasColumnName("telefone");
            entity.Property(e => e.Usuario)
                .HasMaxLength(20)
                .HasColumnName("usuario");

            entity.HasOne(d => d.Bioterio).WithMany(p => p.Pesquisadors)
                .HasForeignKey(d => d.BioterioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pesquisador_bioterio1");
        });

        modelBuilder.Entity<Povoargaiola>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("povoargaiola");

            entity.HasIndex(e => e.GaiolaId, "fk_povoarGaiola_gaiola1_idx");

            entity.HasIndex(e => e.HaremId, "fk_povoarGaiola_harem1_idx");

            entity.HasIndex(e => e.PesquisadorId, "fk_povoarGaiola_pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.DataChegada)
                .HasColumnType("date")
                .HasColumnName("dataChegada");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.EstadoGaiola)
                .HasMaxLength(50)
                .HasColumnName("estadoGaiola");
            entity.Property(e => e.GaiolaId).HasColumnName("gaiola_id");
            entity.Property(e => e.HaremId).HasColumnName("harem_id");
            entity.Property(e => e.PesoMedio).HasColumnName("pesoMedio");
            entity.Property(e => e.PesquisadorId).HasColumnName("pesquisador_id");
            entity.Property(e => e.QuantdadeRatos).HasColumnName("quantdadeRatos");
            entity.Property(e => e.Sexo)
                .HasMaxLength(2)
                .HasColumnName("sexo");

            entity.HasOne(d => d.Gaiola).WithMany(p => p.Povoargaiolas)
                .HasForeignKey(d => d.GaiolaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_povoarGaiola_gaiola1");

            entity.HasOne(d => d.Harem).WithMany(p => p.Povoargaiolas)
                .HasForeignKey(d => d.HaremId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_povoarGaiola_harem1");

            entity.HasOne(d => d.Pesquisador).WithMany(p => p.Povoargaiolas)
                .HasForeignKey(d => d.PesquisadorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_povoarGaiola_pesquisador1");
        });

        modelBuilder.Entity<Usoanestesico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usoanestesico");

            entity.HasIndex(e => e.AnestesicoId, "fk_usoAnestesico_anestesico_idx");

            entity.HasIndex(e => new { e.EntradaAnestesicoAnestesicoId, e.EntradaAnestesicoEntradaId }, "fk_usoAnestesico_entradaAnestesico1_idx");

            entity.HasIndex(e => e.ExperimentoId, "fk_usoAnestesico_experimento1_idx");

            entity.HasIndex(e => e.PesquisadorId, "fk_usoAnestesico_pesquisador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnestesicoId).HasColumnName("anestesico_id");
            entity.Property(e => e.AnestesicoUsado)
                .HasMaxLength(255)
                .HasColumnName("anestesicoUsado");
            entity.Property(e => e.DataUtilização)
                .HasColumnType("date")
                .HasColumnName("dataUtilização");
            entity.Property(e => e.EntradaAnestesicoAnestesicoId).HasColumnName("entradaAnestesico_anestesico_id");
            entity.Property(e => e.EntradaAnestesicoEntradaId).HasColumnName("entradaAnestesico_entrada_id");
            entity.Property(e => e.ExperimentoId).HasColumnName("experimento_id");
            entity.Property(e => e.NumeroCepa).HasColumnName("numeroCepa");
            entity.Property(e => e.NumerosAnimalEnvolcido).HasColumnName("numerosAnimalEnvolcido");
            entity.Property(e => e.PesquisadorId).HasColumnName("pesquisador_id");
            entity.Property(e => e.ProcedimetoRealizado)
                .HasMaxLength(255)
                .HasColumnName("procedimetoRealizado");
            entity.Property(e => e.Volume)
                .HasPrecision(10)
                .HasColumnName("volume");

            entity.HasOne(d => d.Anestesico).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => d.AnestesicoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_anestesico");

            entity.HasOne(d => d.Experimento).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => d.ExperimentoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_experimento1");

            entity.HasOne(d => d.Pesquisador).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => d.PesquisadorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_pesquisador1");

            entity.HasOne(d => d.Entradaanestesico).WithMany(p => p.Usoanestesicos)
                .HasForeignKey(d => new { d.EntradaAnestesicoAnestesicoId, d.EntradaAnestesicoEntradaId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usoAnestesico_entradaAnestesico1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
