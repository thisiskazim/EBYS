using EBYS.Application.Common.Interface;
using EBYS.Domain.Entities;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace EBYS.Persistence
{
    public class EBYSContext : DbContext
    {
        private readonly int _currentKurumId;
        public int CurrentUserId => _currentUserId;
        private readonly int _currentUserId;

        public EBYSContext(DbContextOptions<EBYSContext> options, ICurrentUserService userService) : base(options)
        {
            _currentKurumId = userService.GetKurumId();
            _currentUserId = userService.GetUserId();

        }

        public DbSet<GidenEvrak> Evraklar { get; set; }
        public DbSet<EvrakKonuKodu> EvrakKonuKodlari { get; set; }
        public DbSet<GidenEvrakEk> EvrakEkler { get; set; }
        public DbSet<GidenEvrakIlgi> EvrakIlgiler { get; set; }
        public DbSet<Muhatap> Muhataplar { get; set; }
        public DbSet<BaseKurum> BaseKurums { get; set; }
        public DbSet<KurumMuhatap> KurumMuhataplar { get; set; }
        public DbSet<TuzelKisiMuhatap> TuzelKisiMuhataplar { get; set; }
        public DbSet<BireyselMuhatap> BireyselMuhataplar { get; set; }
        public DbSet<GidenEvrakMuhatap> EvrakMuhataplari { get; set; }
        public DbSet<GidenEvrakAkis> EvrakAkislari { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<ImzaRota> ImzaRotalar { get; set; }
        public DbSet<ImzaRotaAdimi> ImzaRotaAdimlari { get; set; }
        public DbSet<GelenEvrak> GelenEvraklar { get; set; }
        public DbSet<GelenEvrakEk> GelenEvrakEkler { get; set; }
        public DbSet<GelenEvrakIlgi> GelenEvrakIlgileri { get; set; }
        public DbSet<GelenEvrakSevk> GelenEvrakSevkler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // 1. KONTROL: Entity, BaseEntity'den türemiş olmalı
                // 2. KONTROL: Kalıtım varsa (Muhatap örneğindeki gibi), filtre sadece en üst (Root) sınıfa yazılır
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.BaseType == null)
                {
                    // Lambda parametresi: (e => ...)
                    var parameter = Expression.Parameter(entityType.ClrType, "e");

                    // e.BaseKurumId mülküne erişim
                    var property = Expression.Property(parameter, "BaseKurumId");

                    // --- DİNAMİK BAĞLANTI ---
                    // DbContext içindeki private readonly int _currentKurumId alanına referans alıyoruz
                    var contextExpression = Expression.Constant(this);
                    var fieldInfo = typeof(EBYSContext).GetField("_currentKurumId",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (fieldInfo == null) continue; // Alan bulunamazsa atla

                    var currentKurumIdAccess = Expression.Field(contextExpression, fieldInfo);

                    // e.BaseKurumId == this._currentKurumId karşılaştırması
                    // property.Type kullanarak int veya int? uyumunu sağlar
                    var condition = Expression.Equal(property, Expression.Convert(currentKurumIdAccess, property.Type));

                    var lambda = Expression.Lambda(condition, parameter);

                    // Filtreyi Model'e uygula
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            // 1. TPH Yapılandırması (Kalıtım Yönetimi)
            modelBuilder.Entity<Muhatap>()
                .HasDiscriminator<string>("MuhatapTipi") // Veritabanında gizli bir kolon açar
                .HasValue<KurumMuhatap>("Kurum")
                .HasValue<TuzelKisiMuhatap>("Tuzel")
                .HasValue<BireyselMuhatap>("Birey");


            modelBuilder.Entity<GidenEvrakMuhatap>()
                .HasKey(em => new { em.EvrakId, em.MuhatapId });

            modelBuilder.Entity<GidenEvrakMuhatap>()
                .HasOne(em => em.Evrak)
                .WithMany(e => e.Muhataplar)
                .HasForeignKey(em => em.EvrakId);


            // 1. Muhataplar İlişkisi
            modelBuilder.Entity<GidenEvrakMuhatap>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.Muhataplar)
                .HasForeignKey(x => x.EvrakId)
                .OnDelete(DeleteBehavior.Cascade); // Evrak silinince Muhataplar silinir

            // 2. İlgiler İlişkisi
            modelBuilder.Entity<GidenEvrakIlgi>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.İlgiler)
                .HasForeignKey(x => x.EvrakId)
                .OnDelete(DeleteBehavior.Cascade); // Evrak silinince İlgiler silinir

            modelBuilder.Entity<GidenEvrakEk>()
                 .HasOne(x => x.Evrak)
                 .WithMany(x => x.Ekler)
                 .HasForeignKey(x => x.EvrakId)
                 .IsRequired() // 1. BU ÇOK ÖNEMLİ: EvrakId boş olamaz (Required)
                 .OnDelete(DeleteBehavior.Cascade); // 2. Evrak silinirse her şeyi sil

          
            modelBuilder.Entity<GidenEvrakIlgi>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.İlgiler)
                .HasForeignKey(x => x.EvrakId)
                .IsRequired() // Null olamaz dedik
                .OnDelete(DeleteBehavior.Cascade);

        
            modelBuilder.Entity<GidenEvrak>()
                .HasOne(e => e.EvrakKonuKodu)
                .WithMany()
                .HasForeignKey(e => e.KonuKoduId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<GidenEvrak>()
                .HasOne(e => e.ImzaRota)
                .WithMany()
                .HasForeignKey(e => e.ImzaRotaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GidenEvrakMuhatap>()
                .HasOne(em => em.Muhatap)
                .WithMany(m => m.Evraklar)
                .HasForeignKey(em => em.MuhatapId);

            modelBuilder.Entity<GidenEvrak>()
                .HasOne(e => e.Olusturan)
                .WithMany()
                .HasForeignKey(e => e.OlusturanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GidenEvrak>()
             .Property(e => e.Icerik)
             .HasColumnType("text");

            modelBuilder.Entity<GidenEvrak>()
                .Property(e => e.ImzaAltindaOlanIcerik)
                .HasColumnType("text");


            modelBuilder.Entity<GidenEvrakAkis>(entity =>
            {
               
                entity.HasOne(d => d.Evrak)
                    .WithMany(p => p.AkisAdimlari)
                    .HasForeignKey(d => d.EvrakId)
                    .OnDelete(DeleteBehavior.Cascade);


             
                entity.HasOne(d => d.Kullanici)
                    .WithMany()
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         

            //gelen evrak

            // --- Gelen Evrak ve Ekler İlişkisi ---
            modelBuilder.Entity<GelenEvrakEk>()
                .HasOne(x => x.GelenEvrak)
                .WithMany(x => x.Ekler)
                .HasForeignKey(x => x.GelenEvrakId)
                .OnDelete(DeleteBehavior.Cascade); 

            // --- Gelen Evrak ve İlgiler İlişkisi ---
            modelBuilder.Entity<GelenEvrakIlgi>()
                .HasOne(x => x.GelenEvrak)
                .WithMany(x => x.Ilgileri)
                .HasForeignKey(x => x.GelenEvrakId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Gelen Evrak ve Sevkler (Akış) İlişkisi ---
            modelBuilder.Entity<GelenEvrakSevk>()
                .HasOne(x => x.GelenEvrak)
                .WithMany(x => x.Sevkler)
                .HasForeignKey(x => x.GelenEvrakId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GelenEvrak>()
                .HasIndex(x => x.KayitNo)
                .IsUnique();

        }

        //KurumId'yi zorunlu olarak bas
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetBaseKurumId(_currentKurumId);

                    if (entry.Entity is GidenEvrak evrak)
                    {
                        evrak.SetOlusturanId(_currentUserId);
                    }
                    else if (entry.Entity is GelenEvrak gEvrak){
                        gEvrak.SetOlusturanId(_currentUserId);
                    }
                    

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("BaseKurumId").IsModified = false;

                    if (entry.Entity is GidenEvrak)
                    {
                        entry.Property("OlusturanId").IsModified = false;
                    }
                    else if (entry.Entity is GelenEvrak)
                    {
                        entry.Property("OlusturanId").IsModified = false;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
