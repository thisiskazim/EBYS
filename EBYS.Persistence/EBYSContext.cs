using EBYS.Application.Common.Interface;
using EBYS.Domain.Entities;
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

        public DbSet<Evrak> Evraklar { get; set; }
        public DbSet<EvrakKonuKodu> EvrakKonuKodlari { get; set; }
        public DbSet<EvrakEk> EvrakEkler { get; set; }
        public DbSet<EvrakIlgi> EvrakIlgiler { get; set; }
        public DbSet<Muhatap> Muhataplar { get; set; }
        public DbSet<BaseKurum> BaseKurums { get; set; }
        public DbSet<KurumMuhatap> KurumMuhataplar { get; set; }
        public DbSet<TuzelKisiMuhatap> TuzelKisiMuhataplar { get; set; }
        public DbSet<BireyselMuhatap> BireyselMuhataplar { get; set; }
        public DbSet<EvrakMuhatap> EvrakMuhataplari { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<ImzaRota> ImzaRotalar { get; set; }
        public DbSet<ImzaRotaAdimi> ImzaRotaAdimlari { get; set; }
        public DbSet<EvrakAkis> EvrakAkislari { get; set; }

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


            modelBuilder.Entity<EvrakMuhatap>()
                .HasKey(em => new { em.EvrakId, em.MuhatapId });

            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(em => em.Evrak)
                .WithMany(e => e.Muhataplar)
                .HasForeignKey(em => em.EvrakId);


            // 1. Muhataplar İlişkisi
            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.Muhataplar)
                .HasForeignKey(x => x.EvrakId)
                .OnDelete(DeleteBehavior.Cascade); // Evrak silinince Muhataplar silinir

            // 2. İlgiler İlişkisi
            modelBuilder.Entity<EvrakIlgi>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.İlgiler)
                .HasForeignKey(x => x.EvrakId)
                .OnDelete(DeleteBehavior.Cascade); // Evrak silinince İlgiler silinir

            modelBuilder.Entity<EvrakEk>()
                 .HasOne(x => x.Evrak)
                 .WithMany(x => x.Ekler)
                 .HasForeignKey(x => x.EvrakId)
                 .IsRequired() // 1. BU ÇOK ÖNEMLİ: EvrakId boş olamaz (Required)
                 .OnDelete(DeleteBehavior.Cascade); // 2. Evrak silinirse her şeyi sil

            // Aynı işlemi İlgiler için de yap
            modelBuilder.Entity<EvrakIlgi>()
                .HasOne(x => x.Evrak)
                .WithMany(x => x.İlgiler)
                .HasForeignKey(x => x.EvrakId)
                .IsRequired() // Null olamaz dedik
                .OnDelete(DeleteBehavior.Cascade);

            // Konu Kodu Koruması
            modelBuilder.Entity<Evrak>()
                .HasOne(e => e.EvrakKonuKodu)
                .WithMany()
                .HasForeignKey(e => e.KonuKoduId)
                .OnDelete(DeleteBehavior.Restrict);

            // İmza Rota Koruması
            modelBuilder.Entity<Evrak>()
                .HasOne(e => e.ImzaRota)
                .WithMany()
                .HasForeignKey(e => e.ImzaRotaId)
                .OnDelete(DeleteBehavior.Restrict); // Silinmesini engelle!

            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(em => em.Muhatap)
                .WithMany(m => m.Evraklar)
                .HasForeignKey(em => em.MuhatapId);

            modelBuilder.Entity<Evrak>()
                .HasOne(e => e.Olusturan)
                .WithMany()
                .HasForeignKey(e => e.OlusturanId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcıyı sildirme!


            modelBuilder.Entity<EvrakAkis>(entity =>
            {
                // Bir evrak silinirse, o evraka ait akış adımları da silinsin (Cascade)
                entity.HasOne(d => d.Evrak)
                    .WithMany(p => p.AkisAdimlari)
                    .HasForeignKey(d => d.EvrakId)
                    .OnDelete(DeleteBehavior.Cascade);


                // Bir kullanıcı silinirse, akış kayıtları silinmesin (Restrict) 
                // Çünkü o imza geçmişi bir belgedir, kullanıcı gitse de imza kalmalı.
                entity.HasOne(d => d.Kullanici)
                    .WithMany()
                    .HasForeignKey(d => d.KullaniciId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            modelBuilder.Entity<Evrak>()
                .Property(e => e.Icerik)
                .HasColumnType("text");

            modelBuilder.Entity<Evrak>()
                .Property(e => e.ImzaAltindaOlanIcerik)
                .HasColumnType("text");

            
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

                    if (entry.Entity is Evrak evrak)
                    {
                        evrak.SetOlusturanId(_currentUserId);
                    }


                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("BaseKurumId").IsModified = false;

                    if (entry.Entity is Evrak)
                    {
                        entry.Property("OlusturanId").IsModified = false;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
