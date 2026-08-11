using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseLayeredApi.Data;

public class AppDbContext : IdentityDbContext<User, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>> 
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }




    // -------------- BETTER OPTION FOR RELATIONSHIP--------------------------

    protected override void OnModelCreating(ModelBuilder modelBuilder)   // Dbcontext ke andar pahle se onmodelcreating() methad hota hai hum usi ko apne requirement ke according modify krte hai isliye override use kiye hai  
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)    // Every category has one user
            .WithMany()   // one user has many category
            .HasForeignKey(c => c.UserId)   // category table has column of user foreign key 
            .OnDelete(DeleteBehavior.Restrict);    // User cannot be deleted while related records exist

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Income>()
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}





