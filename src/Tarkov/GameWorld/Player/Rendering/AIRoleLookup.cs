/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using LoneEftDmaRadar.Tarkov.GameWorld.Player.Helpers;
using LoneEftDmaRadar.UI.Misc;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Player.Rendering
{
    /// <summary>
    /// Provides AI role identification based on voice lines.
    /// Extracted from AbstractPlayer to separate AI classification logic.
    /// </summary>
    public static class AIRoleLookup
    {
        /// <summary>
        /// AI role information.
        /// </summary>
        public readonly struct AIRole
        {
            public string Name { get; init; }
            public PlayerType Type { get; init; }
        }

        /// <summary>
        /// Lookup AI Info based on Voice Line.
        /// </summary>
        public static AIRole GetRoleInfo(string voiceLine)
        {
            // Direct matches for known bosses and special units
            return voiceLine switch
            {
                "BossSanitar" => new AIRole { Name = "Sanitar", Type = PlayerType.AIBoss },
                "BossBully" => new AIRole { Name = "Reshala", Type = PlayerType.AIBoss },
                "BossGluhar" => new AIRole { Name = "Gluhar", Type = PlayerType.AIBoss },
                "SectantPriest" => new AIRole { Name = "Priest", Type = PlayerType.AIBoss },
                "SectantWarrior" => new AIRole { Name = "Cultist", Type = PlayerType.AIRaider },
                "BossKilla" => new AIRole { Name = "Killa", Type = PlayerType.AIBoss },
                "BossTagilla" => new AIRole { Name = "Tagilla", Type = PlayerType.AIBoss },
                "Boss_Partizan" => new AIRole { Name = "Partisan", Type = PlayerType.AIBoss },
                "BossBigPipe" => new AIRole { Name = "Big Pipe", Type = PlayerType.AIBoss },
                "BossBirdEye" => new AIRole { Name = "Birdeye", Type = PlayerType.AIBoss },
                "BossKnight" => new AIRole { Name = "Knight", Type = PlayerType.AIBoss },
                "Arena_Guard_1" => new AIRole { Name = "Arena Guard", Type = PlayerType.AIScav },
                "Arena_Guard_2" => new AIRole { Name = "Arena Guard", Type = PlayerType.AIScav },
                "Boss_Kaban" => new AIRole { Name = "Kaban", Type = PlayerType.AIBoss },
                "Boss_Kollontay" => new AIRole { Name = "Kollontay", Type = PlayerType.AIBoss },
                "Boss_Sturman" => new AIRole { Name = "Shturman", Type = PlayerType.AIBoss },
                "Zombie_Generic" => new AIRole { Name = "Zombie", Type = PlayerType.AIScav },
                "BossZombieTagilla" => new AIRole { Name = "Zombie Tagilla", Type = PlayerType.AIBoss },
                "Zombie_Fast" => new AIRole { Name = "Zombie", Type = PlayerType.AIScav },
                "Zombie_Medium" => new AIRole { Name = "Zombie", Type = PlayerType.AIScav },
                _ => GetRoleFromPattern(voiceLine)
            };
        }

        private static AIRole GetRoleFromPattern(string voiceLine)
        {
            if (voiceLine.Contains("scav", StringComparison.OrdinalIgnoreCase))
                return new AIRole { Name = "Scav", Type = PlayerType.AIScav };

            if (voiceLine.Contains("boss", StringComparison.OrdinalIgnoreCase))
                return new AIRole { Name = "Boss", Type = PlayerType.AIBoss };

            if (voiceLine.Contains("usec", StringComparison.OrdinalIgnoreCase))
                return new AIRole { Name = "Usec", Type = PlayerType.AIRaider };

            if (voiceLine.Contains("bear", StringComparison.OrdinalIgnoreCase))
                return new AIRole { Name = "Bear", Type = PlayerType.AIRaider };

            DebugLogger.LogDebug($"Unknown Voice Line: {voiceLine}");
            return new AIRole { Name = "AI", Type = PlayerType.AIScav };
        }
    }
}
