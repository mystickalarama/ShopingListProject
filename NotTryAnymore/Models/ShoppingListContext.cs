using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NotTryAnymore.Models
{
	public partial class ShoppingListContext : DbContext
	{
		public ShoppingListContext()
		{
		}

		public ShoppingListContext(DbContextOptions<ShoppingListContext> options) : base(options)
		{
		}

		public virtual DbSet<CategoryModel> Categories { get; set; }
		public virtual DbSet<UserModel> Users { get; set; }
		public virtual DbSet<ProductModel> Products { get; set; }
		public virtual DbSet<ShopListModel> ShopLists { get; set; }
		public virtual DbSet<ShopListDetailModel> ShopListDeails { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-HESUVIP\\TEW_SQLEXPRESS;Database=ShoppingListDB;Trusted_Connection=True;Encrypt=False");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CategoryModel>(entity =>
			{
				entity.HasIndex(e => e.CategoryName, "CategoryName");

				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
				entity.Property(e => e.CategoryName).HasMaxLength(30);
			});

			modelBuilder.Entity<UserModel>(entity =>
			{
				entity.Property(e => e.UserId)
					.HasColumnName("UserID");

				entity.Property(e => e.UserName).HasMaxLength(25);
				entity.Property(e => e.UserSurname).HasMaxLength(20);
				entity.Property(e => e.Mail).HasMaxLength(50);
			});

			modelBuilder.Entity<ShopListModel>(entity =>
			{
				entity.HasIndex(e => e.UserId, "UserID");

				entity.HasIndex(e => e.UserId, "UsersShopList");

				entity.HasIndex(e => e.ShopListName, "ShopListName");

				entity.Property(e => e.ShopListId).HasColumnName("ShopListID");
				entity.Property(e => e.UserId)
					.HasColumnName("UserID");
				entity.Property(e => e.ShopListName).HasMaxLength(30);

				entity.HasOne(d => d.User).WithMany(p => p.ShopLists)
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("FK_ShopLists_Users");
			});

			modelBuilder.Entity<ShopListDetailModel>(entity =>
			{
				entity.HasKey(e => new { e.ShopListId, e.ProductId }).HasName("PK_ShopList_Details");

				entity.ToTable("Shoping List Details");

				entity.HasIndex(e => e.ShopListId, "ShopListID");

				entity.HasIndex(e => e.ShopListId, "ShopListShopList_Details");

				entity.HasIndex(e => e.ProductId, "ProductID");

				entity.HasIndex(e => e.ProductId, "ProductsShopList_Details");

				entity.Property(e => e.ShopListId).HasColumnName("ShopListID");
				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.Quantity).HasDefaultValueSql("(0)");
				entity.Property(e => e.Price).HasColumnType("money");
				entity.Property(e => e.Brand).HasMaxLength(20);
				entity.Property(e => e.Description).HasMaxLength(120);

				entity.HasOne(d => d.ShopList).WithMany(p => p.ShopListDetails)
					.HasForeignKey(d => d.ShopListId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_ShopList_Details_ShopLists");

				entity.HasOne(d => d.Product).WithMany(p => p.ShopListDetail)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_ShopList_Details_Products");
			});

			modelBuilder.Entity<ProductModel>(entity =>
			{
				entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

				entity.HasIndex(e => e.CategoryId, "CategoryID");

				entity.HasIndex(e => e.ProductName, "ProductName");

				entity.Property(e => e.ProductId).HasColumnName("ProductID");
				entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
				entity.Property(e => e.ProductName).HasMaxLength(40);
				entity.Property(e => e.ProductImage).HasMaxLength(50);

				entity.HasOne(d => d.Category).WithMany(p => p.Products)
					.HasForeignKey(d => d.CategoryId)
					.HasConstraintName("FK_Products_Categories");
			});
			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
