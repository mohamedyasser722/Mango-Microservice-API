namespace Mango.Services.CouponAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Coupon>().HasData(seedCopouns());
    }

    public DbSet<Coupon> Coupons { get; set; }




    private static List<Coupon> seedCopouns()
    {
        List<Coupon> coupons = new List<Coupon>();

        coupons.Add(new Coupon
        {
            CouponId = 1,
            CouponCode = "10OFF",
            DiscountAmount = 10,
            MinAmount = 20
        });

        coupons.Add(new Coupon
        {
            CouponId = 2,
            CouponCode = "20OFF",
            DiscountAmount = 20,
            MinAmount = 40
        });

        return coupons;
    }

}
