using Analiza_Risc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Analiza_Risc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Logica constructorului
    }
    public DbSet<Companie> Companies { get; set; }
    public DbSet<Active_Imobilizate> ActiveImobilizate { get; set; }
    public DbSet<Active_Circulante> ActiveCirculante { get; set; }
    public DbSet<Datorii> Datorii { get; set; }
    public DbSet<CapitaluriiP> CapitaluriiP { get; set; }
    public DbSet<Ration_Financiar> RationFinanciar { get; set; }
    public DbSet<IndicatorR> IndicatorR { get; set; }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurați relația "un la unu" între Companie și Active_Imobilizate
        modelBuilder.Entity<Companie>()
            .HasOne(c => c.ActiveImobilizate)
            .WithOne(c=>c.Companie)
            .HasForeignKey<Active_Imobilizate>(ai => ai.Id_Companie)
            .OnDelete(DeleteBehavior.Cascade);//Aceasta este relatie unu la unu intre companie si active imobilizate DeleteBehavior.Cascade are rolul de a nu sterge compania daca stergem activele imobilizate
        modelBuilder.Entity<Companie>()
            .HasOne(c => c.ActiveCirculante)
            .WithOne(ai => ai.Companie)
            .HasForeignKey<Active_Circulante>(ai => ai.Id_Companie)
            .OnDelete(DeleteBehavior.Cascade);//Aceasta este relatie unu la unu intre companie si active circulante DeleteBehavior.Restrict are rolul de a nu sterge compania daca stergem activele circulante
        modelBuilder.Entity<Companie>()
            .HasOne(c => c.Datorii)
            .WithOne(d => d.Companie)
            .HasForeignKey<Datorii>(ai => ai.Id_Companie)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Companie>()
            .HasOne(c => c.CapitaluriiP)
            .WithOne(cp => cp.Companie)
            .HasForeignKey<CapitaluriiP>(ai => ai.Id_Companie)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Active_Imobilizate>()
            .HasOne(ai => ai.RationFinanciar)
            .WithOne(rf => rf.ActiveImobilizate)
            .HasForeignKey<Ration_Financiar>(rf => rf.Id_Activ_IMT)
            .OnDelete(DeleteBehavior.Cascade); // Modificare la Cascade
        modelBuilder.Entity<Active_Circulante>()
            .HasOne(ai => ai.RationFinanciar)
            .WithOne(rf => rf.ActiveCirculante)
            .HasForeignKey<Ration_Financiar>(rf => rf.Id_Active_circ)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Datorii>()
            .HasOne(ai => ai.RationFinanciar)
            .WithOne(rf => rf.Datorii)
            .HasForeignKey<Ration_Financiar>(rf => rf.Id_Datorii)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CapitaluriiP>()
            .HasOne(ai => ai.RationFinanciar)
            .WithOne(rf => rf.CapitaluriiP)
            .HasForeignKey<Ration_Financiar>(rf => rf.Id_CapitaluriP)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Ration_Financiar>()
            .HasOne(ai => ai.IndicatorR)
            .WithOne(rf => rf.RationFinanciar)
            .HasForeignKey<IndicatorR>(rf => rf.Id_RationF)
            .OnDelete(DeleteBehavior.Cascade); // Modificare la Cascade
        base.OnModelCreating(modelBuilder);
    }
}