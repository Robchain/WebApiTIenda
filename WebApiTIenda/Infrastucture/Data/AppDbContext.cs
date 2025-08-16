using Microsoft.EntityFrameworkCore;
using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Infrastucture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListDetail> WishListDetails { get; set; }
        public DbSet<OrderR> OrderRs { get; set; }
        public DbSet<OrderRDetail> OrderRDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========================
            // USUARIO
            // ========================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(l => l.IdUser);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
                entity.Property(u => u.Password).HasMaxLength(255).IsRequired();
                entity.Property(u => u.Rol)
                      .HasMaxLength(20)
                      .IsRequired()
                      .HasConversion<string>(); 

                entity.Property(u => u.DateCreated).HasDefaultValueSql("GETDATE()");
            });

            // ========================
            // PRODUCTO
            // ========================
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(l => l.IdProduct);
                entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
                entity.Property(p => p.Category).HasMaxLength(100).IsRequired();
                entity.Property(p => p.ImageUrl).HasMaxLength(255);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
            });

            // ========================
            // PROVEEDOR
            // ========================
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(l => l.IdSuPlier);
                entity.Property(pr => pr.Name).HasMaxLength(200).IsRequired();
                entity.Property(pr => pr.Contact).HasMaxLength(100);
                entity.Property(pr => pr.Phone).HasMaxLength(50);
                entity.Property(pr => pr.Address).HasMaxLength(255);
            });

            // ========================
            // LOTE
            // ========================
            modelBuilder.Entity<Lot>(entity =>
            {
                entity.HasKey(l => l.IdLot);
                entity.Property(l => l.CodeLote).HasMaxLength(50).IsRequired();
            });

            // ========================
            // PRODUCTO PROVEEDOR (llave compuesta)
            // ========================
            modelBuilder.Entity<ProductSupplier>(entity =>
            {
                entity.HasKey(pp => new { pp.IdProduct, pp.IdSupplier, pp.IdLot });

                entity.HasOne(pp => pp.Product)
                      .WithMany(p => p.ProductSuppliers)
                      .HasForeignKey(pp => pp.IdProduct);

                entity.HasOne(pp => pp.Supplier)
                      .WithMany(pr => pr.ProductSuppliers)
                      .HasForeignKey(pp => pp.IdSupplier);

                entity.HasOne(pp => pp.Lot)
                      .WithMany(l => l.ProductSuppliers)
                      .HasForeignKey(pp => pp.IdLot);

                entity.Property(pp => pp.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(pp => pp.Stock).IsRequired();
            });

            // ========================
            // LISTA DE DESEOS
            // ========================
            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(l => l.IdList);
                entity.HasOne(ld => ld.User)
                      .WithMany(u => u.WishLists)
                      .HasForeignKey(ld => ld.IdUser);
            });

            // ========================
            // LISTA DE DESEOS DETALLE (llave compuesta)
            // ========================
            modelBuilder.Entity<WishListDetail>(entity =>
            {
                entity.HasKey(ldd => new { ldd.IdList, ldd.IdProduct });

                entity.HasOne(ldd => ldd.WishList)
                      .WithMany(ld => ld.WishListDetails)
                      .HasForeignKey(ldd => ldd.IdList);

                entity.HasOne(ldd => ldd.Product)
                      .WithMany(p => p.WishListDetails)
                      .HasForeignKey(ldd => ldd.IdProduct);

                entity.Property(ldd => ldd.DateAdded)
                      .HasDefaultValueSql("GETDATE()");
            });

            // ========================
            // PEDIDO
            // ========================
            modelBuilder.Entity<OrderR>(entity =>
            {
                entity.HasKey(l => l.IdOrder);
                entity.HasOne(p => p.User)
                      .WithMany(u => u.OrderRs)
                      .HasForeignKey(p => p.IdUser);

                entity.Property(p => p.State).HasMaxLength(20).IsRequired();
                entity.Property(p => p.Total).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(p => p.OrderDate).HasDefaultValueSql("GETDATE()");
            });

            // ========================
            // PEDIDO DETALLE (llave compuesta)
            // ========================
            modelBuilder.Entity<OrderRDetail>(entity =>
            {
                entity.HasKey(pd => new { pd.IdOrder, pd.IdProduct });

                entity.HasOne(pd => pd.OrderR)
                      .WithMany(p => p.OrderRDetails)
                      .HasForeignKey(pd => pd.IdOrder);

                entity.HasOne(pd => pd.Product)
                      .WithMany(p => p.OrderRDetails)
                      .HasForeignKey(pd => pd.IdProduct);

                entity.Property(pd => pd.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
            });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ProductSupplier>()
                .HasKey(pp => new { pp.IdProduct, pp.IdSupplier, pp.IdLot });

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(pp => pp.IdProduct);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(pp => pp.Supplier)
                .WithMany(pr => pr.ProductSuppliers)
                .HasForeignKey(pp => pp.IdSupplier);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(pp => pp.Lot)
                .WithMany(l => l.ProductSuppliers)
                .HasForeignKey(pp => pp.IdLot);

          
            modelBuilder.Entity<WishList>()
                .HasOne(ld => ld.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(ld => ld.IdUser);

           
            modelBuilder.Entity<WishListDetail>()
                .HasKey(ldd => new { ldd.IdList, ldd.IdProduct });

            modelBuilder.Entity<WishListDetail>()
                .HasOne(ldd => ldd.WishList)
                .WithMany(ld => ld.WishListDetails)
                .HasForeignKey(ldd => ldd.IdList);

            modelBuilder.Entity<WishListDetail>()
                .HasOne(ldd => ldd.Product)
                .WithMany(p => p.WishListDetails)
                .HasForeignKey(ldd => ldd.IdProduct);

          
            modelBuilder.Entity<OrderR>()
                .HasOne(p => p.User)
                .WithMany(u => u.OrderRs)
                .HasForeignKey(p => p.IdUser);

            
            modelBuilder.Entity<OrderRDetail>()
                .HasKey(pd => new { pd.IdOrder, pd.IdProduct });

            modelBuilder.Entity<OrderRDetail>()
                .HasOne(pd => pd.OrderR)
                .WithMany(p => p.OrderRDetails)
                .HasForeignKey(pd => pd.IdOrder);

            modelBuilder.Entity<OrderRDetail>()
                .HasOne(pd => pd.Product)
                .WithMany(p => p.OrderRDetails)
                .HasForeignKey(pd => pd.IdProduct);
        }

    }

}
