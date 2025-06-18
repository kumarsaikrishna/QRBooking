using Microsoft.EntityFrameworkCore;
using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.Models.DB
{
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<ManualOrder>().ToTable("ManualOrder");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<OfficeSupplyOrder>().ToTable("OfficeSupplyOrder");
            

        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<ManualOrder> ManualOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OfficeSupplyOrder> OfficeSupplyOrderS { get; set; }
    }



}

