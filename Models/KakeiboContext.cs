using Kakeibo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kakeibo.Models;

public partial class KakeiboContext : IdentityDbContext<KakeiboUser>
{
    public KakeiboContext()
    {
    }

    public KakeiboContext(DbContextOptions<KakeiboContext> options)
        : base(options)
    {
    }

    public static KakeiboContext Create()
    {
        return new KakeiboContext();
    }

    public virtual DbSet<Category> Categorys { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Monthly_report> Monthly_Reports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Categorys");
        });
        modelBuilder.Entity<Monthly_report>(entity =>
        {
            entity.HasKey(e => e.Year).HasName("PK_Monthly_Report");
            entity.HasKey(e => e.Month).HasName("PK_Monthly_Report");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
