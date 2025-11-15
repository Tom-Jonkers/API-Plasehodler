using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public const string ADMIN_ROLE = "admin";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Card>().HasData(Seed.SeedCards());
        builder.Entity<OwnedCard>().HasData(Seed.SeedOwnedCards());
        builder.Entity<StartingCard>().HasData(Seed.SeedStartingCards());
        builder.Entity<GameConfig>().HasData(Seed.SeedGameConfigs());
        builder.Entity<Paquet>().HasData(Seed.SeedPaquets());
        builder.Entity<Probability>().HasData(Seed.SeedProbabilities());
        builder.Entity<IdentityUser>().HasData(Seed.SeedUsers());
        builder.Entity<IdentityRole>().HasData(Seed.SeedRoles());
        builder.Entity<IdentityUserRole<string>>().HasData(Seed.SeedUserRoles());

        builder.Entity<IdentityUser>().HasData(Seed.SeedTestUsers());
        builder.Entity<Player>().HasData(Seed.SeedTestPlayers());
        builder.Entity<Power>().HasData(Seed.SeedPowers());
        builder.Entity<Status>().HasData(Seed.SeedStatuses());
        builder.Entity<CardPower>().HasData(Seed.CardPowers());
        // Lorsque le modèle de données se complexifient, il faut éventuellement utiliser Fluent API
        // https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
        // pour préciser certaines relations.
        // Nous allons couvrir ce sujet plus tard dans la session
        builder.Entity<Match>()
            .HasOne(m => m.PlayerDataA)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Match>()
            .HasOne(m => m.PlayerDataB)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Card>()
            .Property(i => i.Rarete)
            .HasConversion<string>();

        // Fin de Fluent API
    }

    public DbSet<Card> Cards { get; set; } = default!;
    
    public DbSet<Deck> Decks { get; set; } = default!;

    public DbSet<OwnedCard> OwnedCards { get; set; } = default!;

    public DbSet<Player> Players { get; set; } = default!;

    public DbSet<Match> Matches { get; set; } = default!;

    public DbSet<MatchPlayerData> MatchPlayersData { get; set; } = default!;

    public DbSet<StartingCard> StartingCards { get; set; } = default!;

    public DbSet<GameConfig> GameConfigs { get; set; } = default!;

    public DbSet<Power> Powers { get; set; } = default!;
    public DbSet<Paquet> Paquets { get; set; } = default!;

    public DbSet<CardPower> CardPowers { get; set; } = default!;
    public DbSet<Probability> Probabilities { get; set; } = default!;
}

