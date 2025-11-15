using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Data
{
    public class Seed
    {
        public Seed() { }

        public static Card[] SeedCards()
        {
            return new Card[] {
                new Card
                {
                    Id = 1,
                    Name = "Jarvisse",
                    Attack = 5,
                    Health = 3,
                    Cost = 4,
                    Rarete = Raretes.Épique,
                    ImageUrl = "https://i.imgur.com/8iWUCdM.gif"
                }, new Card
                {
                    Id = 2,
                    Name = "Jermasouss",
                    Attack = 12,
                    Health = 1,
                    Cost = 3,
                    Rarete = Raretes.Épique,
                    ImageUrl = "https://media1.tenor.com/m/xCK-Co72KrIAAAAd/jerma-jerma-sus.gif"
                }, new Card
                {
                    Id = 3,
                    Name = "Evil Rizzled",
                    Attack = 2,
                    Health = 8,
                    Cost = 4,
                    Rarete = Raretes.Légendaire,
                    ImageUrl = "https://media.tenor.com/rPPN4J5vv2UAAAAM/rizzler-costco.gif"
                }, new Card
                {
                    Id = 4,
                    Name = "Jarvousse",
                    Attack = 8,
                    Health = 4,
                    Cost = 5,
                    Rarete = Raretes.Légendaire,
                    ImageUrl = "https://i.imgur.com/T7FZVSU.gif"
                }, new Card
                {
                    Id = 5,
                    Name = "Arf",
                    Attack = 2,
                    Health = 2,
                    Cost = 2,
                    Rarete = Raretes.Commune,
                    ImageUrl = "https://media1.tenor.com/m/DRBLpAd8zVIAAAAd/dog-whimsical.gif"
                }, new Card
                {
                    Id = 6,
                    Name = "Lemur",
                    Attack = 0,
                    Health = 9,
                    Cost = 4,
                    Rarete = Raretes.Commune,
                    ImageUrl = "https://i.pinimg.com/originals/87/ea/89/87ea8952891e51553d55d90dc649bbf3.gif"
                }, new Card
                {
                    Id = 7,
                    Attack = 2,
                    Cost = 4,
                    Health = 6,
                    Rarete = Raretes.Rare,
                    ImageUrl = "https://media1.tenor.com/m/ShtsLOsS2e4AAAAd/freaky-tree.gif",
                    Name = "Écorceur Farceur"
                }, new Card
                {
                    Id = 8,
                    Attack = 1,
                    Cost = 2,
                    Health = 4,
                    Rarete = Raretes.Commune,
                    ImageUrl = "https://media1.tenor.com/m/JWPDuXIDQukAAAAd/cat-osu.gif",
                    Name = "Evil Kitty"
                }, new Card
                {
                    Id = 9,
                    Attack = 0,
                    Cost = 3,
                    Health = 0,
                    Rarete = Raretes.Rare,
                    ImageUrl = "/assets/tom.gif",
                    Name = "Tom Farceur"
                }, new Card
                {
                    Id = 10,
                    Attack = 6,
                    Cost = 2,
                    Health = 1,
                    Rarete = Raretes.Rare,
                    ImageUrl = "https://media1.tenor.com/m/LmK9TOuC1msAAAAd/thats-class-lets-go.gif",
                    Name = "Bathieu"
                }, new Card
                {
                    Id = 11,
                    Attack = 10,
                    Cost = 5,
                    Health = 4,
                    Rarete = Raretes.Épique,
                    ImageUrl = "https://static.wikia.nocookie.net/beegyoshi/images/e/e6/HoCa.jpg",
                    Name = "Hogan's Castle."
                }, new Card
                {
                    Id = 12,
                    Attack = 2,
                    Cost = 3,
                    Health = 5,
                    Rarete = Raretes.Commune,
                    ImageUrl = "https://media.tenor.com/uPmcmIaUtvoAAAAM/john-pork-pork.gif",
                    Name = "John Pork"
                }, new Card
                {
                    Id = 13,
                    Attack = 0,
                    Cost = 3,
                    Health = 10,
                    Rarete = Raretes.Légendaire,
                    ImageUrl = "https://i.kym-cdn.com/entries/icons/original/000/043/174/cover12.jpg",
                    Name = "Hydrogen Bomb VS Coughing Baby"
                }, new Card
                {
                    Id = 14,
                    Attack = 0,
                    Cost = 5,
                    Health = 0,
                    Rarete = Raretes.Légendaire,
                    ImageUrl = "https://i.imgur.com/35y49Rq.png",
                    Name = "FIGUE BANANE NOIX!!!"
                }, new Card
                {
                    Id = 15,
                    Attack = 1,
                    Cost = 5,
                    Health = 10,
                    Rarete = Raretes.Rare,
                    ImageUrl = "/assets/cedric.jpg",
                    Name = "Cédrouc"
                },
                 new Card
                {
                    Id = 16,
                    Attack = 5,
                    Cost = 3,
                    Health = 2,
                    Rarete = Raretes.Rare,
                    ImageUrl = "https://cdn-images.dzcdn.net/images/cover/4937906affa0da23fabba7f104b84bf1/0x1900-000000-80-0-0.jpg",
                    Name = "Pessi"
                },
                  new Card
                {
                    Id = 17,
                    Attack = 8,
                    Cost = 6,
                    Health = 4,
                    Rarete = Raretes.Légendaire,
                    ImageUrl = "https://i.ytimg.com/vi/5DaQZPus0S4/sddefault.jpg",
                    Name = "Bombardino Crocodilo"
                },
                   new Card
                {
                    Id = 18,
                    Attack = 4,
                    Cost = 5,
                    Health = 4,
                    Rarete = Raretes.Épique,
                    ImageUrl = "https://platform.polygon.com/wp-content/uploads/sites/2/2025/04/CJ1.jpg?quality=90&strip=all&crop=11.73046566693,0,76.53906866614,100",
                    Name = "Chicken Jockey"
                },
                    new Card
                {
                    Id = 19,
                    Attack = 6,
                    Cost = 5,
                    Health = 2,
                    Rarete = Raretes.Épique,
                    ImageUrl = "https://i1.sndcdn.com/artworks-VPd3jo9czP2OCWDf-eGwrTA-t500x500.png",
                    Name = "Evil Chicken Jockey"
                },
                     new Card
                {
                    Id = 20,
                    Attack = 1,
                    Cost = 2,
                    Health = 3,
                    Rarete = Raretes.Commune,
                    ImageUrl = "https://static.wikia.nocookie.net/11868989-ce9d-4b44-9daf-3bb2e2393c60/scale-to-width/755",
                    Name = "Gros Sus"
                },
            };
        }

        public static Paquet[] SeedPaquets()
        {
            return new Paquet[]
            {
                new Paquet { Id = 1, Name = "Basic", Cost = 500, NbCartes = 3, RareteParDefaut = Raretes.Commune, ImageUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/4f7705ec-8c49-4eed-a56e-c21f3985254c/dah43cy-a8e121cb-934a-40f6-97c7-fa2d77130dd5.png/v1/fill/w_1024,h_1420/pokemon_card_backside_in_high_resolution_by_atomicmonkeytcg_dah43cy-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9MTQyMCIsInBhdGgiOiJcL2ZcLzRmNzcwNWVjLThjNDktNGVlZC1hNTZlLWMyMWYzOTg1MjU0Y1wvZGFoNDNjeS1hOGUxMjFjYi05MzRhLTQwZjYtOTdjNy1mYTJkNzcxMzBkZDUucG5nIiwid2lkdGgiOiI8PTEwMjQifV1dLCJhdWQiOlsidXJuOnNlcnZpY2U6aW1hZ2Uub3BlcmF0aW9ucyJdfQ.9GzaYS7sd8RPY5FlHca09J9ZQZ9D9zI69Ru-BsbkLDA"},
                new Paquet { Id = 2, Name = "Normal", Cost = 1000, NbCartes = 4, RareteParDefaut = Raretes.Commune, ImageUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/4f7705ec-8c49-4eed-a56e-c21f3985254c/dah43cy-a8e121cb-934a-40f6-97c7-fa2d77130dd5.png/v1/fill/w_1024,h_1420/pokemon_card_backside_in_high_resolution_by_atomicmonkeytcg_dah43cy-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9MTQyMCIsInBhdGgiOiJcL2ZcLzRmNzcwNWVjLThjNDktNGVlZC1hNTZlLWMyMWYzOTg1MjU0Y1wvZGFoNDNjeS1hOGUxMjFjYi05MzRhLTQwZjYtOTdjNy1mYTJkNzcxMzBkZDUucG5nIiwid2lkdGgiOiI8PTEwMjQifV1dLCJhdWQiOlsidXJuOnNlcnZpY2U6aW1hZ2Uub3BlcmF0aW9ucyJdfQ.9GzaYS7sd8RPY5FlHca09J9ZQZ9D9zI69Ru-BsbkLDA"},
                new Paquet { Id = 3, Name = "Super", Cost = 2000, NbCartes = 5, RareteParDefaut = Raretes.Rare, ImageUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/4f7705ec-8c49-4eed-a56e-c21f3985254c/dah43cy-a8e121cb-934a-40f6-97c7-fa2d77130dd5.png/v1/fill/w_1024,h_1420/pokemon_card_backside_in_high_resolution_by_atomicmonkeytcg_dah43cy-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9MTQyMCIsInBhdGgiOiJcL2ZcLzRmNzcwNWVjLThjNDktNGVlZC1hNTZlLWMyMWYzOTg1MjU0Y1wvZGFoNDNjeS1hOGUxMjFjYi05MzRhLTQwZjYtOTdjNy1mYTJkNzcxMzBkZDUucG5nIiwid2lkdGgiOiI8PTEwMjQifV1dLCJhdWQiOlsidXJuOnNlcnZpY2U6aW1hZ2Uub3BlcmF0aW9ucyJdfQ.9GzaYS7sd8RPY5FlHca09J9ZQZ9D9zI69Ru-BsbkLDA"}
            };
        }


        public static Probability[] SeedProbabilities()
        {
            return new Probability[]
            {
                new Probability { Id = 1, PaquetName = "Basic", Rarity = Raretes.Rare, Value = 0.3m, BaseQty = 0 },

                // Probabilités pour le pack "Normal"
                new Probability { Id = 2, PaquetName = "Normal", Rarity = Raretes.Rare, Value = 0.3m, BaseQty = 1 },
                new Probability { Id = 3, PaquetName = "Normal", Rarity = Raretes.Épique, Value = 0.1m, BaseQty = 0 },
                new Probability { Id = 4, PaquetName = "Normal", Rarity = Raretes.Légendaire, Value = 0.02m, BaseQty = 0 },

                // Probabilités pour le pack "Super"
                new Probability { Id = 5, PaquetName = "Super", Rarity = Raretes.Épique, Value = 0.25m, BaseQty = 1 },
                new Probability { Id = 6, PaquetName = "Super", Rarity = Raretes.Légendaire, Value = 0.1m, BaseQty = 0 },
                new Probability { Id = 7, PaquetName = "Super", Rarity = Raretes.Commune, Value = 0m, BaseQty = 0 }
            };
        }

        public static OwnedCard[] SeedOwnedCards()
        {

            return new OwnedCard[]
            {
               new OwnedCard { Id = 1, CardId = 1, PlayerId = "User1Id" },
               new OwnedCard { Id = 2, CardId = 2, PlayerId = "User1Id" },
               new OwnedCard { Id = 3, CardId = 3, PlayerId = "User1Id" },
               new OwnedCard { Id = 4, CardId = 4, PlayerId = "User1Id" },
               new OwnedCard { Id = 5, CardId = 5, PlayerId = "User1Id" },
               new OwnedCard { Id = 6, CardId = 6, PlayerId = "User1Id" },

               new OwnedCard { Id = 7, CardId = 7, PlayerId = "User2Id" },
               new OwnedCard { Id = 8, CardId = 8, PlayerId = "User2Id" },
               new OwnedCard { Id = 9, CardId = 9, PlayerId = "User2Id" },
               new OwnedCard { Id = 10, CardId = 10, PlayerId = "User2Id" },
               new OwnedCard { Id = 11, CardId = 1, PlayerId = "User2Id" },
               new OwnedCard { Id = 12, CardId = 2, PlayerId = "User2Id" },
               new OwnedCard { Id = 13, CardId = 3, PlayerId = "User2Id" },
               new OwnedCard { Id = 14, CardId = 4, PlayerId = "User2Id" }
            };
        }

        public static StartingCard[] SeedStartingCards()
        {
            return new StartingCard[] {
        new StartingCard
        {
            Id = 1,
            CardId = 1
        }, new StartingCard
        {
            Id = 2,
            CardId = 2
        }, new StartingCard
        {
            Id = 3,
            CardId = 3
        },
        new StartingCard
        {
            Id = 4,
            CardId = 1
        }, new StartingCard
        {
            Id = 5,
            CardId = 2
        }, new StartingCard
        {
            Id = 6,
            CardId = 3
        },new StartingCard
        {
            Id = 7,
            CardId = 4
        }, new StartingCard
        {
            Id = 8,
            CardId = 5
        }, new StartingCard
        {
            Id = 9,
            CardId = 6
        },new StartingCard
        {
            Id = 10,
            CardId = 4
        }, new StartingCard
        {
            Id = 11,
            CardId = 5
        }, new StartingCard
        {
            Id = 12,
            CardId = 6
        },new StartingCard
        {
            Id = 13,
            CardId = 4
        }, new StartingCard
        {
            Id = 14,
            CardId = 5
        }, new StartingCard
        {
            Id = 15,
            CardId = 6
        }, new StartingCard
        {
            Id = 16,
            CardId = 13
        }
            };
        }

        public static GameConfig[] SeedGameConfigs()
        {
            return new GameConfig[] {
                new GameConfig
                {
                    Id = 1,
                    nbCardToDraw = 4,
                    qtManaPerTurn = 3,
                    MonnaieRecueCreation = 150,
                    MonnaieRecueVictoire = 100,
                    MonnaieRecueDefaite = 15
                }
            };
        }



        public static IdentityUser[] SeedUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();
            IdentityUser admin = new IdentityUser
            {
                Id = "11111111-1111-1111-1111-111111111111",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                // La comparaison d'identity se fait avec les versions normalisés
                NormalizedEmail = "ADMIN@ADMIN.COM",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                // On encrypte le mot de passe
                PasswordHash = hasher.HashPassword(null, "Passw0rd!"),
                LockoutEnabled = true
            };

            return new IdentityUser[] { admin };
        }

        public static IdentityRole[] SeedRoles()
        {
            IdentityRole adminRole = new IdentityRole
            {
                Id = "11111111-1111-1111-1111-111111111112",
                Name = ApplicationDbContext.ADMIN_ROLE,
                NormalizedName = ApplicationDbContext.ADMIN_ROLE.ToUpper()
            };

            return new IdentityRole[] { adminRole };
        }

        public static IdentityUserRole<string>[] SeedUserRoles()
        {
            IdentityUserRole<string> userAdmin = new IdentityUserRole<string>
            {
                RoleId = "11111111-1111-1111-1111-111111111112",
                UserId = "11111111-1111-1111-1111-111111111111"
            };
            return new IdentityUserRole<string>[] { userAdmin };
        }

        public static IdentityUser[] SeedTestUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();
            return new IdentityUser[] {
                new IdentityUser()
                {
                Id = "User1Id",
                UserName = "Jean",
                Email = "jean@jean.jean",
                NormalizedEmail = "JEAN@JEAN.COM",
                NormalizedUserName = "JEAN",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Passw0rd!"),
                LockoutEnabled = true
                },
                new IdentityUser
                {
                Id = "User2Id",
                UserName = "Bob",
                Email = "bob@bob.bob",
                NormalizedEmail = "BOB@BOB.COM",
                NormalizedUserName = "BOB",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Passw0rd!"),
                LockoutEnabled = true
                }
            };
        }

        public static Player[] SeedTestPlayers()
        {
            return new Player[] {
                new Player
                {
                    Id = 1,
                    Name = "Test player 1",
                    UserId = "User1Id",
                    Money = 15000,
                    ELO = 1000
                },
                new Player
                {
                    Id = 2,
                    Name = "Test player 2",
                    UserId = "User2Id",
                    Money = 4000,
                    ELO = 750
                }
            };
        }

        public static Power[] SeedPowers()
        {
            return new Power[]
            {
                new Power
                {
                    Id = 1,
                    Name = "Assault",
                    Description = "Ça assault",
                    Icone = "🦍"
                },
                new Power
                {
                    Id = 2,
                    Name = "Piquotte",
                    Description = "Ça piquotte",
                    Icone = "💒"
                },
                new Power
                {
                    Id = 3,
                    Name = "Soin",
                    Description = "Ça soin",
                    Icone = "🙇‍♂️"
                },
                new Power
                {
                    Id = 4,
                    Name = "Nuke",
                    Description = "Ça nuke",
                    Icone = "☢"
                },
                new Power
                {
                    Id = 5,
                    Name = "Chaos",
                    Description = "Ça chaos pas mal",
                    Icone = "💥"
                },
                new Power
                {
                    Id = 6,
                    Name = "Earthquake",
                    Description = "Ça shake",
                    Icone = "🌎"
                },
                new Power
                {
                    Id = 7,
                    Name = "Random Pain",
                    Description = "T'as mal mais tu sais pas pourquoi",
                    Icone = "⁉"
                },
                new Power
                {
                    Id = 8,
                    Name = "Poison",
                    Description = "Ça em🐟e",
                    Icone = "🐟"
                },
                new Power
                {
                    Id = 9,
                    Name = "Stun",
                    Description = "Ça stun",
                    Icone = "💫"
                },
                new Power
                {
                    Id = 10,
                    Name = "Charme",
                    Description = "Offre un abonnement Tinder Premium",
                    Icone = "💕"
                }
            };
        }

        public static CardPower[] CardPowers()
        {
            return new CardPower[]
            {
                new CardPower
                {
                    Id = 1,
                    CardId = 6,
                    PowerId = 1,
                    Value = 3
                },
                new CardPower
                {
                    Id = 2,
                    CardId = 4,
                    PowerId = 1,
                    Value = 2
                },
                new CardPower
                {
                    Id = 3,
                    CardId = 5,
                    PowerId = 2,
                    Value = 3
                },
                new CardPower
                {
                    Id = 4,
                    CardId = 3,
                    PowerId = 3,
                    Value = 3
                },
                new CardPower
                {
                    Id = 5,
                    CardId = 6,
                    PowerId = 3,
                    Value = 3
                },
                new CardPower
                {
                    Id = 6,
                    CardId = 3,
                    PowerId = 8,
                    Value = 2
                },
                new CardPower
                {
                    Id = 7,
                    CardId = 13,
                    PowerId = 4,
                    Value = 1
                },
                new CardPower
                {
                    Id = 8,
                    CardId = 15,
                    PowerId = 10,
                    Value = 1
                },
                new CardPower
                {
                    Id = 9,
                    CardId = 5,
                    PowerId = 5,
                    Value = 1
                },
                new CardPower
                {
                    Id = 10,
                    CardId = 8,
                    PowerId = 9,
                    Value = 5
                },
                new CardPower
                {
                    Id = 11,
                    CardId = 14,
                    PowerId = 6,
                    Value = 4
                },
                new CardPower
                {
                    Id = 12,
                    CardId = 9,
                    PowerId = 7,
                    Value = 1
                }
            };
        }
        
        public static Status[] SeedStatuses()
        {
            return new Status[]
            {
                new Status
                {
                    Id = 1,
                    Name = "Poison",
                    Description = "Ça em🐟e",
                    Icone = "🦍"
                },
                new Status
                {
                    Id = 2,
                    Name = "Stunned",
                    Description = "Ça stun",
                    Icone = "💫"
                },
                new Status
                {
                    Id = 3,
                    Name = "Charmed",
                    Description = "Ça charmé",
                    Icone = "💕"
                }
            };
        }
    }
}

