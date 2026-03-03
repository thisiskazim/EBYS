using EBYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EBYS.Application.Interface;
namespace EBYS.Persistence
{
    public class EBYSContext:DbContext
    {
        private readonly int _currentKurumId;

        public EBYSContext(DbContextOptions<EBYSContext> options, ICurrentUserService userService) : base(options)
        {
            _currentKurumId = userService.GetKurumId();

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

            // 1. Örnek Bir Kurum Oluşturuyoruz
            modelBuilder.Entity<BaseKurum>().HasData(new BaseKurum
            {
                Id = 1,
                BaseKurumId = 1, // Kurumun kendisi de bir kurum id'sine sahip olmalı (BaseEntity gereği)
                KurumAdi = "EBYS Genel Müdürlüğü",
                KurumKodu = "EGM001",
                VergiNo = "1234567890",
                DetsisNo = "DTS123456",
                creat_time = new DateTime()
            });

            // 2. Örnek Bir Rol Oluşturuyoruz
            modelBuilder.Entity<Rol>().HasData(new Rol
            {
                Id = 1,
                BaseKurumId = 1,
                RolAdi = "Sistem Yöneticisi",
                creat_time = new DateTime()
            });

            // 3. Örnek Bir Kullanıcı Oluşturuyoruz
            modelBuilder.Entity<Kullanici>().HasData(new Kullanici
            {
                Id = 1,
                BaseKurumId = 1,
                RolId = 1,
                Ad = "Kazim",
                Soyad = "U",
                KimlikNo = "11111111111", // Login olurken bunu kullanacağız
                SifreHash = "123456", // ŞİMDİLİK düz metin, Login servisini yazınca hash'e çevireceğiz
                creat_time = new DateTime()
            });



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
        private LambdaExpression CreateFilterExpression(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            // e.KurumId == _currentKurumId sorgusunu hazırlar
            var body = Expression.Equal(
                Expression.Property(parameter, "BaseKurumId"),
                Expression.Constant(_currentKurumId)
            );
            return Expression.Lambda(body, parameter);
        }

    }
}
