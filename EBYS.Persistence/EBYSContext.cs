using EBYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence
{
    public class EBYSContext:DbContext
    {
        public EBYSContext(DbContextOptions<EBYSContext> options) : base(options) { }

        public DbSet<Evrak> Evraklar { get; set; }
        public DbSet<EvrakEk> EvrakEkler { get; set; }
        public DbSet<EvrakIlgi> EvrakIlgiler { get; set; }
        public DbSet<Muhatap> Muhataplar { get; set; }
        public DbSet<KurumMuhatap> KurumMuhataplar { get; set; }
        public DbSet<TuzelKisiMuhatap> TuzelKisiMuhataplar { get; set; }
        public DbSet<BireyselMuhatap> BireyselMuhataplar { get; set; }
        public DbSet<EvrakMuhatap> EvrakMuhataplari { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. TPH Yapılandırması (Kalıtım Yönetimi)
            modelBuilder.Entity<Muhatap>()
                .HasDiscriminator<string>("MuhatapTipi") // Veritabanında gizli bir kolon açar
                .HasValue<KurumMuhatap>("Kurum")
                .HasValue<TuzelKisiMuhatap>("Tuzel")
                .HasValue<BireyselMuhatap>("Birey");


            modelBuilder.Entity<EvrakMuhatap>()
                .HasKey(em => new { em.EvrakId, em.MuhatapId });

            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(em => em.Evrak)
                .WithMany(e => e.Muhataplar)
                .HasForeignKey(em => em.EvrakId);

            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(em => em.Muhatap)
                .WithMany(m => m.Evraklar)
                .HasForeignKey(em => em.MuhatapId);

            // 3. PostgreSQL İçin Veri Tipleri (Opsiyonel)
            modelBuilder.Entity<Evrak>()
                .Property(e => e.Icerik)
                .HasColumnType("text");

            base.OnModelCreating(modelBuilder);

        }

        }
}
