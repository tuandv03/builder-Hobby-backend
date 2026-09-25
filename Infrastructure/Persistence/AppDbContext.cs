using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public virtual DbSet<Card> Cards { get; set; }

	public virtual DbSet<Cardimage> Cardimages { get; set; }

	public virtual DbSet<Cardinventory> Cardinventories { get; set; }

	public virtual DbSet<Cardset> Cardsets { get; set; }

	public virtual DbSet<Order> Orders { get; set; }

	public virtual DbSet<Orderdetail> Orderdetails { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer("Server=localhost;Database=Yugioh;User Id=sa;Password=Lamcha@123;TrustServerCertificate=True;");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Card>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("cards_pkey");

			entity.ToTable("cards");

			entity.Property(e => e.Id)
				.HasConversion<int>()
				.ValueGeneratedNever()
				.HasColumnName("id");
			entity.Property(e => e.Archetype)
				.HasMaxLength(100)
				.HasColumnName("archetype");
			entity.Property(e => e.Atk).HasColumnName("atk");
			entity.Property(e => e.Attribute)
				.HasMaxLength(20)
				.HasColumnName("attribute");
			entity.Property(e => e.Def).HasColumnName("def");
			entity.Property(e => e.Description).HasColumnName("description");
			entity.Property(e => e.FrameType)
				.HasMaxLength(50)
				.HasColumnName("frame_type");
			entity.Property(e => e.Level).HasColumnName("level");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
			entity.Property(e => e.Race)
				.HasMaxLength(100)
				.HasColumnName("race");
			entity.Property(e => e.Type)
				.HasMaxLength(100)
				.HasColumnName("type");
			entity.Property(e => e.YgoprodeckUrl).HasColumnName("ygoprodeck_url");
			entity.Ignore(e => e.HumanReadableType);
			entity.Ignore(e => e.Rarity);
			entity.Ignore(e => e.SetCode);
			entity.Ignore(e => e.SetName);
			entity.Ignore(e => e.Price);
		});

		modelBuilder.Entity<Cardimage>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("cardimages_pkey");

			entity.ToTable("card_images");

			entity.HasIndex(e => e.CardId, "idx_cardimages_card_id");

			entity.Property(e => e.Id).HasConversion<int>().HasColumnName("id");
			entity.Property(e => e.CardId).HasConversion<int>().HasColumnName("card_id");
			entity.Property(e => e.ImageId)
				.HasConversion<int>()
				.HasColumnName("image_id");
			entity.Property(e => e.ImageUrl).HasColumnName("image_url");
			entity.Property(e => e.ImageUrlCropped).HasColumnName("image_url_cropped");
			entity.Property(e => e.ImageUrlSmall).HasColumnName("image_url_small");

			entity.HasOne(d => d.Card).WithMany(p => p.Cardimages)
				.HasForeignKey(d => d.CardId)
				.HasConstraintName("fk_cardimages_card");
		});

		modelBuilder.Entity<Cardinventory>(entity =>
		{
			entity.HasKey(e => e.InventoryId).HasName("cardinventory_pkey");

			entity.ToTable("cardinventory");

			entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
			entity.Property(e => e.BuyPrice)
				.HasPrecision(10, 2)
				.HasColumnName("buy_price");
			entity.Property(e => e.CardsetId).HasColumnName("cardset_id");
			entity.Property(e => e.Quantity)
				.HasDefaultValue(0)
				.HasColumnName("quantity");
			entity.Property(e => e.SellPrice)
				.HasPrecision(10, 2)
				.HasColumnName("sell_price");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("datetime2")
				.HasColumnName("updated_at");

			entity.HasOne(d => d.Cardset).WithMany(p => p.Cardinventories)
				.HasForeignKey(d => d.CardsetId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_inventory_set");
		});

		modelBuilder.Entity<Cardset>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("cardsets_pkey");

			entity.ToTable("card_sets");

			entity.Property(e => e.Id).HasConversion<int>().HasColumnName("id");
			entity.Property(e => e.CardId).HasConversion<int>().HasColumnName("card_id");
			entity.Property(e => e.SetCode)
				.HasMaxLength(50)
				.HasColumnName("set_code");
			entity.Property(e => e.SetName)
				.HasMaxLength(255)
				.HasColumnName("set_name");
			entity.Property(e => e.SetPrice)
				.HasPrecision(10, 2)
				.HasColumnName("set_price");
			entity.Property(e => e.SetRarity)
				.HasMaxLength(100)
				.HasColumnName("set_rarity");
			entity.Property(e => e.SetRarityCode)
				.HasMaxLength(20)
				.HasColumnName("set_rarity_code");
			entity.Ignore(e => e.CardName);
			entity.Ignore(e => e.CardCode);

			entity.HasOne(d => d.Card).WithMany(p => p.Cardsets)
				.HasForeignKey(d => d.CardId)
				.HasConstraintName("fk_cardsets_card");
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasKey(e => e.OrderId).HasName("orders_pkey");

			entity.ToTable("orders");

			entity.Property(e => e.OrderId).HasColumnName("order_id");
			entity.Property(e => e.CustomerAddress).HasColumnName("customer_address");
			entity.Property(e => e.CustomerName)
				.HasMaxLength(255)
				.HasColumnName("customer_name");
			entity.Property(e => e.CustomerPhone)
				.HasMaxLength(50)
				.HasColumnName("customer_phone");
			entity.Property(e => e.OrderDate)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("datetime2")
				.HasColumnName("order_date");
			entity.Property(e => e.Status)
				.HasMaxLength(50)
				.HasDefaultValueSql("'PENDING'")
				.HasColumnName("status");
		});

		modelBuilder.Entity<Orderdetail>(entity =>
		{
			entity.HasKey(e => e.OrderDetailId).HasName("orderdetail_pkey");

			entity.ToTable("orderdetail");

			entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
			entity.Property(e => e.CardsetId).HasColumnName("cardset_id");
			entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
			entity.Property(e => e.OrderId).HasColumnName("order_id");
			entity.Property(e => e.Quantity).HasColumnName("quantity");
			entity.Property(e => e.Subtotal)
				.HasPrecision(10, 2)
				.HasComputedColumnSql("[quantity] * [unit_price]", true)
				.HasColumnName("subtotal");
			entity.Property(e => e.UnitPrice)
				.HasPrecision(10, 2)
				.HasColumnName("unit_price");

			entity.HasOne(d => d.Cardset).WithMany(p => p.Orderdetails)
				.HasForeignKey(d => d.CardsetId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_orderdetail_set");

			entity.HasOne(d => d.Inventory).WithMany(p => p.Orderdetails)
				.HasForeignKey(d => d.InventoryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_orderdetail_inventory");

			entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
				.HasForeignKey(d => d.OrderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_orderdetail_order");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
