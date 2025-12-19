/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using LoneEftDmaRadar.Tarkov.Unity;
using LoneEftDmaRadar.Tarkov.Unity.Structures;
using VmmSharpEx.Scatter;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Loot
{
    /// <summary>
    /// Handles scatter read operations for loot discovery.
    /// Flattens the nested callback structure into a more maintainable pattern.
    /// </summary>
    internal sealed class LootScatterReader
    {
        private readonly ConcurrentDictionary<ulong, LootItem> _loot;
        private readonly CancellationToken _ct;

        /// <summary>
        /// Intermediate data collected during scatter reads.
        /// </summary>
        private readonly ConcurrentDictionary<ulong, LootReadContext> _contexts = new();

        public LootScatterReader(ConcurrentDictionary<ulong, LootItem> loot, CancellationToken ct)
        {
            _loot = loot;
            _ct = ct;
        }

        /// <summary>
        /// Setup scatter reads for a loot item across all 4 rounds.
        /// </summary>
        public void SetupScatterReads(
            ulong lootBase,
            VmmScatter round1,
            VmmScatter round2,
            VmmScatter round3,
            VmmScatter round4,
            VmmScatterMap map)
        {
            if (_loot.ContainsKey(lootBase))
                return; // Already processed

            // Create context to track this loot item through rounds
            var context = new LootReadContext { LootBase = lootBase };
            _contexts[lootBase] = context;

            // Round 1: Read MonoBehaviour and C1
            SetupRound1(lootBase, context, round1, round2, round3, round4, map);
        }

        private void SetupRound1(
            ulong lootBase,
            LootReadContext context,
            VmmScatter round1,
            VmmScatter round2,
            VmmScatter round3,
            VmmScatter round4,
            VmmScatterMap map)
        {
            round1.PrepareReadPtr(lootBase + ObjectClass.MonoBehaviourOffset);
            round1.PrepareReadPtr(lootBase + ObjectClass.To_NamePtr[0]);

            round1.Completed += (sender, s1) =>
            {
                if (!s1.ReadPtr(lootBase + ObjectClass.MonoBehaviourOffset, out var monoBehaviour) ||
                    !s1.ReadPtr(lootBase + ObjectClass.To_NamePtr[0], out var c1))
                {
                    _contexts.TryRemove(lootBase, out _);
                    return;
                }

                context.MonoBehaviour = monoBehaviour;
                context.C1 = c1;

                SetupRound2(context, round2, round3, round4, map);
            };
        }

        private void SetupRound2(
            LootReadContext context,
            VmmScatter round2,
            VmmScatter round3,
            VmmScatter round4,
            VmmScatterMap map)
        {
            round2.PrepareReadPtr(context.MonoBehaviour + UnitySDK.UnityOffsets.Component_ObjectClassOffset);
            round2.PrepareReadPtr(context.MonoBehaviour + UnitySDK.UnityOffsets.Component_GameObjectOffset);
            round2.PrepareReadPtr(context.C1 + ObjectClass.To_NamePtr[1]);

            round2.Completed += (sender, s2) =>
            {
                if (!s2.ReadPtr(context.MonoBehaviour + UnitySDK.UnityOffsets.Component_ObjectClassOffset, out var interactiveClass) ||
                    !s2.ReadPtr(context.MonoBehaviour + UnitySDK.UnityOffsets.Component_GameObjectOffset, out var gameObject) ||
                    !s2.ReadPtr(context.C1 + ObjectClass.To_NamePtr[1], out var classNamePtr))
                {
                    _contexts.TryRemove(context.LootBase, out _);
                    return;
                }

                context.InteractiveClass = interactiveClass;
                context.GameObject = gameObject;
                context.ClassNamePtr = classNamePtr;

                SetupRound3(context, round3, round4, map);
            };
        }

        private void SetupRound3(
            LootReadContext context,
            VmmScatter round3,
            VmmScatter round4,
            VmmScatterMap map)
        {
            round3.PrepareRead(context.ClassNamePtr, LootConstants.MaxClassNameReadLength);
            round3.PrepareReadPtr(context.GameObject + UnitySDK.UnityOffsets.GameObject_ComponentsOffset);
            round3.PrepareReadPtr(context.GameObject + UnitySDK.UnityOffsets.GameObject_NameOffset);

            round3.Completed += (sender, s3) =>
            {
                var className = s3.ReadString(context.ClassNamePtr, LootConstants.MaxClassNameReadLength, Encoding.UTF8);
                if (className == null ||
                    !s3.ReadPtr(context.GameObject + UnitySDK.UnityOffsets.GameObject_ComponentsOffset, out var components) ||
                    !s3.ReadPtr(context.GameObject + UnitySDK.UnityOffsets.GameObject_NameOffset, out var pGameObjectName))
                {
                    _contexts.TryRemove(context.LootBase, out _);
                    return;
                }

                context.ClassName = className;
                context.Components = components;
                context.GameObjectNamePtr = pGameObjectName;

                SetupRound4(context, round4, map);
            };
        }

        private void SetupRound4(
            LootReadContext context,
            VmmScatter round4,
            VmmScatterMap map)
        {
            round4.PrepareRead(context.GameObjectNamePtr, LootConstants.MaxObjectNameReadLength);
            round4.PrepareReadPtr(context.Components + LootConstants.ComponentsTransformOffset);

            round4.Completed += (sender, s4) =>
            {
                var objectName = s4.ReadString(context.GameObjectNamePtr, LootConstants.MaxObjectNameReadLength, Encoding.UTF8);
                if (objectName == null ||
                    !s4.ReadPtr(context.Components + LootConstants.ComponentsTransformOffset, out var transformInternal))
                {
                    _contexts.TryRemove(context.LootBase, out _);
                    return;
                }

                context.ObjectName = objectName;
                context.TransformInternal = transformInternal;
                context.IsComplete = true;

                // Register final processing on map completion
                map.Completed += (sender, _) =>
                {
                    _ct.ThrowIfCancellationRequested();
                    ProcessCompletedContext(context);
                };
            };
        }

        private void ProcessCompletedContext(LootReadContext context)
        {
            if (!context.IsComplete)
                return;

            try
            {
                var processor = new LootItemProcessor(_loot);
                processor.Process(
                    context.LootBase,
                    context.InteractiveClass,
                    context.ClassName,
                    context.ObjectName,
                    context.TransformInternal);
            }
            catch
            {
                // Swallow processing errors for individual items
            }
            finally
            {
                _contexts.TryRemove(context.LootBase, out _);
            }
        }

        /// <summary>
        /// Context object to track a loot item through multiple scatter read rounds.
        /// </summary>
        private sealed class LootReadContext
        {
            public ulong LootBase { get; init; }

            // Round 1 results
            public ulong MonoBehaviour { get; set; }
            public ulong C1 { get; set; }

            // Round 2 results
            public ulong InteractiveClass { get; set; }
            public ulong GameObject { get; set; }
            public ulong ClassNamePtr { get; set; }

            // Round 3 results
            public string ClassName { get; set; }
            public ulong Components { get; set; }
            public ulong GameObjectNamePtr { get; set; }

            // Round 4 results
            public string ObjectName { get; set; }
            public ulong TransformInternal { get; set; }

            public bool IsComplete { get; set; }
        }
    }
}
