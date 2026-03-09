using EBYS.Application.Common.Interface;
using EBYS.Domain.Entities;
using EBYS.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace EBYS.Persistence
{
    public class EBYSContext:DbContext
    {
        private readonly int _currentKurumId;
        private readonly int _currentUserId;

        public EBYSContext(DbContextOptions<EBYSContext> options, ICurrentUserService userService) : base(options)
        {
            _currentKurumId = userService.GetKurumId();
            _currentUserId = userService.GetUserId();

        }

        public DbSet<Evrak> Evraklar { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          

            // Tüm Entity tiplerini tara
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Eğer bu entity BaseEntity'den türetilmişse
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.BaseType == null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "BaseKurumId");
                    var condition = Expression.Equal(property, Expression.Constant(_currentKurumId));
                    var lambda = Expression.Lambda(condition, parameter);

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

            modelBuilder.Entity<EvrakMuhatap>()
                .HasOne(em => em.Muhatap)
                .WithMany(m => m.Evraklar)
                .HasForeignKey(em => em.MuhatapId);

            // 3. PostgreSQL İçin Veri Tipleri (Opsiyonel)
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

        private LambdaExpression CreateFilterExpression(Type type)
        {
            var parameter = Expression.Parameter(type, "e");

            var body = Expression.Equal(
                Expression.Property(parameter, "BaseKurumId"),
                Expression.Constant(_currentKurumId)
            );
            return Expression.Lambda(body, parameter);
        }

    }
}
