using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RaprockPlaylist.Models;
using Pomelo.EntityFrameworkCore.MySql;

namespace RaprockPlaylist.Context
{
    public partial class PlaylistContext : DbContext
    {
        public PlaylistContext(DbContextOptions<PlaylistContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Band> Band { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<SongRequest> SongRequest { get; set; }
        public virtual DbSet<Visitor> Visitor { get; set; }
        private IConfiguration Configuration {get;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Band>(entity =>
            {
                entity.HasKey(e => e.IdBand)
                    .HasName("PRIMARY");

                entity.ToTable("band");

                entity.Property(e => e.IdBand)
                    .HasColumnName("idBand")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BandLocation)
                    .HasColumnName("band_Location")
                    .HasColumnType("varchar(45)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.BandName)
                    .HasColumnName("band_Name")
                    .HasColumnType("varchar(45)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.InPlaylist)
                    .IsRequired()
                    .HasColumnName("inPlaylist")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'0x30'");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => new { e.IdErrorLog, e.TsErrorLog })
                    .HasName("PRIMARY");

                entity.ToTable("errorLog");

                entity.HasIndex(e => e.IdErrorLog)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdVisitor)
                    .HasName("FK_idVisitor_idx");

                entity.HasIndex(e => new { e.TsErrorLog, e.Source })
                    .HasName("tsErrorLog_Source_idx");

                entity.Property(e => e.IdErrorLog)
                    .HasColumnName("idErrorLog")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TsErrorLog)
                    .HasColumnName("tsErrorLog")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IdVisitor)
                    .HasColumnName("idVisitor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Message)
                    .HasColumnType("varchar(255)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.Source)
                    .HasColumnType("varchar(50)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.HasOne(d => d.IdVisitorNavigation)
                    .WithMany(p => p.ErrorLog)
                    .HasForeignKey(d => d.IdVisitor)
                    .HasConstraintName("FK_idVisitor");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PRIMARY");

                entity.ToTable("log");

                entity.HasIndex(e => e.IdVisitor)
                    .HasName("FK_log_idVisitor_idx");

                entity.HasIndex(e => e.TsLog)
                    .HasName("tsLog_INX");

                entity.Property(e => e.IdLog)
                    .HasColumnName("idLog")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdVisitor)
                    .HasColumnName("idVisitor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("varchar(255)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasColumnType("varchar(50)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.TsLog)
                    .HasColumnName("tsLog")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.IdVisitorNavigation)
                    .WithMany(p => p.Log)
                    .HasForeignKey(d => d.IdVisitor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_log_idVisitor");
            });

            modelBuilder.Entity<SongRequest>(entity =>
            {
                entity.HasKey(e => e.IdSongRequest)
                    .HasName("PRIMARY");

                entity.ToTable("songRequest");

                entity.HasIndex(e => e.IdVisitor)
                    .HasName("FK_songRequest_idVisitor_idx");

                entity.Property(e => e.IdSongRequest)
                    .HasColumnName("idSongRequest")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.Property(e => e.IdVisitor)
                    .HasColumnName("idVisitor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SongRequest1)
                    .HasColumnName("songRequest")
                    .HasColumnType("mediumtext")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);

                entity.HasOne(d => d.IdVisitorNavigation)
                    .WithMany(p => p.SongRequest)
                    .HasForeignKey(d => d.IdVisitor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_songRequest_idVisitor");
            });

            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasKey(e => e.IdVisitor)
                    .HasName("PRIMARY");

                entity.ToTable("visitor");

                entity.Property(e => e.IdVisitor)
                    .HasColumnName("idVisitor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IpAdress)
                    .HasColumnName("ipAdress")
                    .HasColumnType("varchar(20)")
                    .HasCharSet(Pomelo.EntityFrameworkCore.MySql.Storage.CharSet.Utf8);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
