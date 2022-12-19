using Microsoft.EntityFrameworkCore;

namespace TryALittlerHarder.Models
{
	public partial class HardDB : DbContext
	{
		public HardDB()
		{
		}

		public HardDB(DbContextOptions<HardDB> options) : base(options) 
		{ 
		}

		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<OrderDetail> OrderDetails { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-HESUVIP\\TEW_SQLEXPRESS;Database=SamilDB;Trusted_Connection=True;Encrypt=False");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>(entity =>
			{
				entity.HasIndex(e => e.CategoryName, "CategoryName");

				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
				entity.Property(e => e.CategoryName).HasMaxLength(15);
			});

			modelBuilder.Entity<Customer>(entity =>
			{
				entity.Property(e => e.CustomerId)
					.HasColumnName("CustomerID");
			});

			modelBuilder.Entity<Order>(entity =>
			{
				entity.HasIndex(e => e.CustomerId, "CustomerID");

				entity.HasIndex(e => e.CustomerId, "CustomersOrders");

				entity.HasIndex(e => e.OrderName, "OrderName");

				entity.Property(e => e.OrderId).HasColumnName("OrderID");
				entity.Property(e => e.CustomerId)
					.HasMaxLength(5)
					.IsFixedLength()
					.HasColumnName("CustomerID");
				entity.Property(e => e.OrderName).HasMaxLength(30);

				entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
					.HasForeignKey(d => d.CustomerId)
					.HasConstraintName("FK_Orders_Customers");
			});

			modelBuilder.Entity<OrderDetail>(entity =>
			{
				entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_Order_Details");

				entity.ToTable("Order Details");

				entity.HasIndex(e => e.OrderId, "OrderID");

				entity.HasIndex(e => e.OrderId, "OrdersOrder_Details");

				entity.HasIndex(e => e.ProductId, "ProductID");

				entity.HasIndex(e => e.ProductId, "ProductsOrder_Details");

				entity.Property(e => e.OrderId).HasColumnName("OrderID");
				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.Quantity).HasDefaultValueSql("(0)");
				entity.Property(e => e.UnitPrice).HasColumnType("money");

				entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
					.HasForeignKey(d => d.OrderId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Order_Details_Orders");

				entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Order_Details_Products");
			});

			modelBuilder.Entity<Product>(entity =>
			{
				entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

				entity.HasIndex(e => e.CategoryId, "CategoryID");

				entity.HasIndex(e => e.ProductName, "ProductName");

				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
				entity.Property(e => e.ProductName).HasMaxLength(40);
				entity.Property(e => e.Image).HasMaxLength(50);

				entity.HasOne(d => d.Category).WithMany(p => p.Products)
					.HasForeignKey(d => d.CategoryId)
					.HasConstraintName("FK_Products_Categories");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
