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
        public virtual DbSet<BandHasUser> BandHasUser { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Song> Song { get; set; }
        public virtual DbSet<SongRequest> SongRequest { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCredentials> UserCredentials { get; set; }
        public virtual DbSet<UserHasVisitor> UserHasVisitor { get; set; }
        public virtual DbSet<Visitor> Visitor { get; set; }
        private IConfiguration Configuration { get; }
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
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.BandName)
                    .HasColumnName("band_Name")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<BandHasUser>(entity =>
            {
                entity.HasKey(e => new { e.BandIdBand, e.UserIdUser })
                    .HasName("PRIMARY");

                entity.ToTable("band_has_user");

                entity.HasIndex(e => e.BandIdBand)
                    .HasName("fk_band_has_user_band1_idx");

                entity.HasIndex(e => e.UserIdUser)
                    .HasName("fk_band_has_user_user1_idx");

                entity.Property(e => e.BandIdBand)
                    .HasColumnName("band_idBand")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserIdUser)
                    .HasColumnName("user_idUser")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.BandIdBandNavigation)
                    .WithMany(p => p.BandHasUser)
                    .HasForeignKey(d => d.BandIdBand)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_band_has_user_band1");

                entity.HasOne(d => d.UserIdUserNavigation)
                    .WithMany(p => p.BandHasUser)
                    .HasForeignKey(d => d.UserIdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_band_has_user_user1");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.IdErrorLog)
                    .HasName("PRIMARY");

                entity.ToTable("errorLog");

                entity.HasIndex(e => e.IdVisitor)
                    .HasName("FK_errorLog_idVisitor_idx");

                entity.HasIndex(e => e.TsErrorLog)
                    .HasName("tsErrorLog_INX");

                entity.Property(e => e.IdErrorLog)
                    .HasColumnName("idErrorLog")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdVisitor)
                    .HasColumnName("idVisitor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Message)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TsErrorLog)
                    .HasColumnName("tsErrorLog")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.IdVisitorNavigation)
                    .WithMany(p => p.ErrorLog)
                    .HasForeignKey(d => d.IdVisitor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_errorLog_idVisitor");
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
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

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

            modelBuilder.Entity<Song>(entity =>
            {
                entity.HasKey(e => e.IdSong)
                    .HasName("PRIMARY");

                entity.ToTable("song");

                entity.HasIndex(e => e.IdBand)
                    .HasName("FK_song_idBand_idx");

                entity.HasIndex(e => e.IdSongRequest)
                    .HasName("FK_song_idSongRequest_idx");

                entity.HasIndex(e => e.SongUrl)
                    .HasName("songUrl_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdSong)
                    .HasColumnName("idSong")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdBand)
                    .HasColumnName("idBand")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdSongRequest)
                    .HasColumnName("idSongRequest")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SongUrl)
                    .IsRequired()
                    .HasColumnName("songUrl")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdBandNavigation)
                    .WithMany(p => p.Song)
                    .HasForeignKey(d => d.IdBand)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_song_idBand");

                entity.HasOne(d => d.IdSongRequestNavigation)
                    .WithMany(p => p.Song)
                    .HasForeignKey(d => d.IdSongRequest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_song_idSongRequest");
            });

            modelBuilder.Entity<SongRequest>(entity =>
            {
                entity.HasKey(e => e.IdSongRequest)
                    .HasName("PRIMARY");

                entity.ToTable("songRequest");

                entity.HasIndex(e => e.IdUser)
                    .HasName("FK_songRequest_idUser_idx");

                entity.Property(e => e.IdSongRequest)
                    .HasColumnName("idSongRequest")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SongRequest1)
                    .HasColumnName("songRequest")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.SongRequest)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_songRequest_idUser");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ConsentGdpr).HasColumnName("consentGDPR");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TsCreate)
                    .HasColumnName("tsCreate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<UserCredentials>(entity =>
            {
                entity.HasKey(e => e.IdUserCredentials)
                    .HasName("PRIMARY");

                entity.ToTable("userCredentials");

                entity.HasIndex(e => e.IdUser)
                    .HasName("FK_userCredentials_idUser_idx");

                entity.Property(e => e.IdUserCredentials)
                    .HasColumnName("idUserCredentials")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TsCreate)
                    .HasColumnName("tsCreate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserCredentials)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_userCredentials_idUser");
            });

            modelBuilder.Entity<UserHasVisitor>(entity =>
            {
                entity.HasKey(e => new { e.UserIdUser, e.VisitorIdVisitor })
                    .HasName("PRIMARY");

                entity.ToTable("user_has_visitor");

                entity.HasIndex(e => e.UserIdUser)
                    .HasName("fk_user_has_visitor_user1_idx");

                entity.HasIndex(e => e.VisitorIdVisitor)
                    .HasName("fk_user_has_visitor_visitor1_idx");

                entity.Property(e => e.UserIdUser)
                    .HasColumnName("user_idUser")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VisitorIdVisitor)
                    .HasColumnName("visitor_idVisitor")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.UserIdUserNavigation)
                    .WithMany(p => p.UserHasVisitor)
                    .HasForeignKey(d => d.UserIdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_has_visitor_user1");

                entity.HasOne(d => d.VisitorIdVisitorNavigation)
                    .WithMany(p => p.UserHasVisitor)
                    .HasForeignKey(d => d.VisitorIdVisitor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_has_visitor_visitor1");
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
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TsCreate)
                    .HasColumnName("tsCreate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
