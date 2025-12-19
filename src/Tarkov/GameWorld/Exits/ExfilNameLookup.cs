/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
MIT License

Copyright (c) 2025 Lone DMA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
*/

using System.Collections.Frozen;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Exits
{
    /// <summary>
    /// Provides friendly name lookups for exfil points across all maps.
    /// Maps internal game names to user-friendly display names.
    /// </summary>
    public static class ExfilNameLookup
    {
        /// <summary>
        /// Gets the friendly display name for an exfil point.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="internalName">The internal game name of the exfil.</param>
        /// <returns>The friendly name if found, otherwise the original internal name.</returns>
        public static string GetFriendlyName(string mapId, string internalName)
        {
            if (string.IsNullOrEmpty(mapId) || string.IsNullOrEmpty(internalName))
                return internalName ?? string.Empty;

            if (MapExfilNames.TryGetValue(mapId, out var mapNames) &&
                mapNames.TryGetValue(internalName, out var friendlyName))
            {
                return friendlyName;
            }

            return internalName;
        }

        /// <summary>
        /// Checks if a friendly name exists for the given exfil.
        /// </summary>
        public static bool TryGetFriendlyName(string mapId, string internalName, out string friendlyName)
        {
            friendlyName = internalName;

            if (string.IsNullOrEmpty(mapId) || string.IsNullOrEmpty(internalName))
                return false;

            if (MapExfilNames.TryGetValue(mapId, out var mapNames) &&
                mapNames.TryGetValue(internalName, out var name))
            {
                friendlyName = name;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lookup table mapping internal exfil names to friendly display names.
        /// Key: Map ID (case-insensitive)
        /// Value: Dictionary of internal name -> friendly name
        /// </summary>
        public static FrozenDictionary<string, FrozenDictionary<string, string>> MapExfilNames { get; } =
            new Dictionary<string, FrozenDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "woods", CreateWoodsExfils() },
                { "shoreline", CreateShorelineExfils() },
                { "bigmap", CreateCustomsExfils() },
                { "interchange", CreateInterchangeExfils() },
                { "rezervbase", CreateReserveExfils() },
                { "laboratory", CreateLabsExfils() },
                { "factory4_day", CreateFactoryExfils() },
                { "factory4_night", CreateFactoryExfils() },
                { "lighthouse", CreateLighthouseExfils() },
                { "tarkovstreets", CreateStreetsExfils() },
                { "Sandbox", CreateGroundZeroExfils() },
                { "Sandbox_high", CreateGroundZeroExfils() },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        #region Map-Specific Exfil Definitions

        private static FrozenDictionary<string, string> CreateWoodsExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Factory Gate", "Friendship Bridge (Co-Op)" },
                { "RUAF Gate", "RUAF Gate" },
                { "ZB-016", "ZB-016" },
                { "ZB-014", "ZB-014" },
                { "UN Roadblock", "UN Roadblock" },
                { "South V-Ex", "Bridge V-Ex" },
                { "Outskirts", "Outskirts" },
                { "un-sec", "Northern UN Roadblock" },
                { "wood_sniper_exit", "Power Line Passage (Flare)" },
                { "woods_secret_minefield", "Railway Bridge to Tarkov (Secret)" },
                { "Friendship Bridge (Co-Op)", "Friendship Bridge (Co-Op)" },
                { "Outskirts Water", "Scav Bridge" },
                { "Dead Man's Place", "Dead Man's Place" },
                { "The Boat", "Boat" },
                { "Scav House", "Scav House" },
                { "East Gate", "Scav Bunker" },
                { "Mountain Stash", "Mountain Stash" },
                { "West Border", "Eastern Rocks" },
                { "Old Station", "Old Railway Depot" },
                { "RUAF Roadblock", "RUAF Roadblock" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateShorelineExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Shorl_V-Ex", "Road to North V-Ex" },
                { "Road to Customs", "Road to Customs" },
                { "Road_at_railbridge", "Railway Bridge" },
                { "Tunnel", "Tunnel" },
                { "Lighthouse_pass", "Path to Lighthouse" },
                { "Smugglers_Trail_coop", "Smuggler's Path (Co-op)" },
                { "Pier Boat", "Pier Boat" },
                { "RedRebel_alp", "Climber's Trail" },
                { "shoreline_secret_heartbeat", "Mountain Bunker (Secret)" },
                { "Scav Road to Customs", "Road to Customs" },
                { "Lighthouse", "Lighthouse" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateCustomsExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "EXFIL_ZB013", "ZB-013" },
                { "EXFIL_FACTORYSHACKS", "Factory Shacks" },
                { "EXFIL_ZB012", "ZB-012" },
                { "EXFIL_TRAIN", "Railroad to Tarkov V-Ex" },
                { "EXFIL_SMUGGLERS", "Smuggler's Boat" },
                { "EXFIL_RUAFGATE", "RUAF Roadblock" },
                { "EXFIL_OLDROAD", "Old Road Gate" },
                { "EXFIL_CROSSROADS", "Crossroads" },
                { "EXFIL_DORMS", "Dorms V-Ex" },
                { "EXFIL_SCAVLANDS", "Scav Lands" },
                { "beyond_fuel_tank", "Beyond Fuel Tank" },
                { "EXFIL_MILBASE_COOP", "Factory Far Corner" },
                { "customs_secret_siren_base", "Scav CP Basement (Secret)" },
                { "Warehouse 17", "Warehouse 17" },
                { "Old Gas Station", "Old Gas Station" },
                { "Railroad to Tarkov", "Railroad to Tarkov" },
                { "SCAV Lands", "SCAV Lands" },
                { "Passage Between Rocks", "Passage Between Rocks" },
                { "Hole in the Fence", "Hole in the Fence" },
                { "Railroad To Port", "Railroad To Port" },
                { "Factory Far Corner", "Factory Far Corner" },
                { "Railroad to Military Base", "Railroad to Military Base" },
                { "Office Window", "Office Window" },
                { "Sniper Roadblock", "Sniper Roadblock" },
                { "Crossroads", "Crossroads" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateInterchangeExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "EXFIL_EMERCOM", "Emercom Checkpoint" },
                { "EXFIL_PP", "Hole in Fence" },
                { "EXFIL_SAFEHOUSE", "Safe Room (Co-Op)" },
                { "EXFIL_INTERCHANGE_VEXIT_COOP", "Power Station V-Ex" },
                { "interchange_secret_atrium", "Collapsed Atrium (Secret)" },
                { "NW Exfil", "Railway Exfil" },
                { "SE Exfil", "Emercom Checkpoint" },
                { "SafeRoom Exfil", "Safe Room (Co-Op)" },
                { "Scav Camp", "Scav Camp" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateReserveExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "EXFIL_Train", "Armored Train" },
                { "EXFIL_Bunker_D2", "D-2 Bunker" },
                { "EXFIL_Hermetic", "Sewer Manhole" },
                { "EXFIL_Mountain_Pass", "Cliff Descent" },
                { "EXFIL_PP_EXFIL", "Scav Lands (Co-Op)" },
                { "EXFIL_BUNKER_COOP", "Bunker Ventilation (Co-Op)" },
                { "rezervbase_secret_elevator_backroom", "RB-RS Elevator (Secret)" },
                { "Sewer Manhole", "Sewer Manhole" },
                { "Hole In Wall", "Hole In Wall" },
                { "D-2 Bunker", "D-2 Bunker" },
                { "Scav Lands", "Scav Lands (Co-Op)" },
                { "Depot Hermetic Door", "Depot Hermetic Door" },
                { "Heating Pipe", "Heating Pipe" },
                { "CP Fence", "Checkpoint Fence" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateLabsExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "lab_Hangar_Gate", "Cargo Elevator" },
                { "lab_Parking_Gate", "Parking Gate" },
                { "lab_Under_Storage_Collector", "Sewage Conduit" },
                { "lab_Elevator_Main_Parking", "Main Elevator" },
                { "lab_Elevator_Cargo", "Freight Elevator" },
                { "lab_Vent", "Ventilation Shaft" },
                { "lab_Elevator_Med", "Medical Elevator" },
                { "lab_Elevator_Tech", "Technical Elevator" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateFactoryExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "exit_factory_gate_0", "Gate 0" },
                { "exit_factory_gate_3", "Gate 3" },
                { "exit_factory_cameraRoom", "Security Room (Co-Op)" },
                { "exit_factory_doctor", "Emergency Exit (Behind Trucks)" },
                { "exit_factory_sniper", "Evacuation Point" },
                { "factory4_secret_power_line", "Tractor at Power Line (Secret)" },
                { "Gate 0", "Gate 0" },
                { "Gate 3", "Gate 3" },
                { "Gate m", "Cellars" },
                { "Office Window", "Office Window (Key)" },
                { "Camera Bunker Door", "Security Room (Co-Op)" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateLighthouseExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "V-Ex_light", "Northern Checkpoint (V-Ex)" },
                { "Tunnel", "Side Tunnel (Co-Op)" },
                { "Alpinist_light", "Mountain Pass" },
                { "Shorl_free", "Path to Shoreline" },
                { "Nothern_Checkpoint", "Northern Checkpoint" },
                { "Coastal_South_Road", "Southern Road" },
                { "EXFIL_Train", "Armored Train" },
                { "lighthouse_secret_minefield", "Passage by the Lake (Secret)" },
                { "Side Tunnel (Co-Op)", "Side Tunnel (Co-Op)" },
                { "Shorl_free_scav", "Path to Shoreline" },
                { "Scav_Coastal_South", "Southern Road" },
                { "Scav_Underboat_Hideout", "Hideout Under the Landing Stage" },
                { "Scav_Hideout_at_the_grotto", "Scav Hideout at the Grotto" },
                { "Scav_Industrial_zone", "Industrial Zone Gates" },
                { "Armored Train", "Armored Train" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateStreetsExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "E8_yard", "Courtyard" },
                { "E7_car", "Primorsky Ave Taxi V-Ex" },
                { "E1", "Stylobate Building Elevator" },
                { "E4", "Crash Site" },
                { "E2", "Sewer River" },
                { "E3", "Damaged House" },
                { "E5", "Collapsed Crane" },
                { "E6", "??" },
                { "E9_sniper", "Klimov Street" },
                { "Exit_E10_coop", "Pinewood Basement (Co-Op)" },
                { "E7", "Expo Checkpoint" },
                { "streets_secret_onyx", "Smugglers' Basement (Secret)" },
                { "scav_e1", "Basement Descent" },
                { "scav_e2", "Entrance to Catacombs" },
                { "scav_e3", "Ventilation Shaft" },
                { "scav_e4", "Sewer Manhole" },
                { "scav_e5", "Near Kamchatskaya Arch" },
                { "scav_e7", "Cardinal Apartment Complex Parking" },
                { "scav_e8", "Klimov Shopping Mall Exfil" },
                { "scav_e6", "Pinewood Basement (Co-Op)" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        private static FrozenDictionary<string, string> CreateGroundZeroExfils() =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Sandbox_VExit", "Police Cordon V-Ex" },
                { "Unity_free_exit", "Emercom Checkpoint" },
                { "Scav_coop_exit", "Scav Checkpoint (Co-Op)" },
                { "Nakatani_stairs_free_exit", "Nakatani Basement Stairs" },
                { "Sniper_exit", "Mira Ave" },
                { "groundzero_secret_adaptation", "Tartowers Sales Office (Secret)" },
                { "Scav Checkpoint (Co-Op)", "Scav Checkpoint (Co-Op)" },
                { "Emercom Checkpoint", "Emercom Checkpoint" },
                { "Nakatani Basement Stairs", "Nakatani Basement Stairs" },
            }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        #endregion
    }
}
