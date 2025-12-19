/*
 * Lone EFT DMA Radar
 * MIT License - Copyright (c) 2025 Lone DMA
 */

using LoneEftDmaRadar.Tarkov.Unity.Structures;
using LoneEftDmaRadar.UI.Misc;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Loot
{
    /// <summary>
    /// Processes loot items after scatter read data is collected.
    /// Handles creation of appropriate LootItem subclasses based on type.
    /// </summary>
    internal sealed class LootItemProcessor
    {
        private readonly ConcurrentDictionary<ulong, LootItem> _loot;

        public LootItemProcessor(ConcurrentDictionary<ulong, LootItem> loot)
        {
            _loot = loot;
        }

        /// <summary>
        /// Process a single loot item and add it to the collection.
        /// </summary>
        public void Process(
            ulong lootBase,
            ulong interactiveClass,
            string className,
            string objectName,
            ulong transformInternal)
        {
            // Skip certain objects
            if (objectName.Contains(LootConstants.SkipObjectNamePattern, StringComparison.OrdinalIgnoreCase))
                return;

            // Determine loot type
            var lootType = DetermineLootType(className);

            // Get position and transform
            var transform = new UnityTransform(transformInternal, true);
            var position = transform.UpdatePosition();

            // Create appropriate loot item
            switch (lootType)
            {
                case LootType.Corpse:
                    ProcessCorpse(lootBase, interactiveClass, position, transform);
                    break;

                case LootType.Container:
                    ProcessContainer(lootBase, interactiveClass, objectName, position);
                    break;

                case LootType.LooseLoot:
                    ProcessLooseLoot(lootBase, interactiveClass, position, transform);
                    break;
            }
        }

        private static LootType DetermineLootType(string className)
        {
            if (className.Contains(LootConstants.CorpseClassName, StringComparison.OrdinalIgnoreCase))
                return LootType.Corpse;

            if (className.Equals(LootConstants.ContainerClassName, StringComparison.OrdinalIgnoreCase))
                return LootType.Container;

            if (className.Equals(LootConstants.LooseLootClassName, StringComparison.OrdinalIgnoreCase))
                return LootType.LooseLoot;

            return LootType.Unknown;
        }

        private void ProcessCorpse(ulong lootBase, ulong interactiveClass, Vector3 position, UnityTransform transform)
        {
            var corpse = new LootCorpse(interactiveClass, position, transform);
            _ = _loot.TryAdd(lootBase, corpse);
        }

        private void ProcessContainer(ulong lootBase, ulong interactiveClass, string objectName, Vector3 position)
        {
            try
            {
                if (objectName.Equals(LootConstants.AirdropObjectName, StringComparison.OrdinalIgnoreCase))
                {
                    _ = _loot.TryAdd(lootBase, new LootAirdrop(position));
                }
                else
                {
                    var containerId = ReadContainerId(interactiveClass);
                    if (!string.IsNullOrEmpty(containerId))
                    {
                        _ = _loot.TryAdd(lootBase, new StaticLootContainer(containerId, position, interactiveClass));
                    }
                }
            }
            catch
            {
                // Skip invalid containers
            }
        }

        private static string ReadContainerId(ulong interactiveClass)
        {
            var itemOwner = Memory.ReadPtr(interactiveClass + Offsets.LootableContainer.ItemOwner);
            var ownerItemBase = Memory.ReadPtr(itemOwner + Offsets.LootableContainerItemOwner.RootItem);
            var ownerItemTemplate = Memory.ReadPtr(ownerItemBase + Offsets.LootItem.Template);
            var ownerItemMongoId = Memory.ReadValue<MongoID>(ownerItemTemplate + Offsets.ItemTemplate._id);
            return ownerItemMongoId.ReadString();
        }

        private void ProcessLooseLoot(ulong lootBase, ulong interactiveClass, Vector3 position, UnityTransform transform)
        {
            try
            {
                var item = Memory.ReadPtr(interactiveClass + Offsets.InteractiveLootItem.Item);
                var itemTemplate = Memory.ReadPtr(item + Offsets.LootItem.Template);
                var isQuestItem = Memory.ReadValue<bool>(itemTemplate + Offsets.ItemTemplate.QuestItem);

                var mongoId = Memory.ReadValue<MongoID>(itemTemplate + Offsets.ItemTemplate._id);
                var id = mongoId.ReadString();

                if (isQuestItem)
                {
                    ProcessQuestItem(lootBase, itemTemplate, id, position, transform);
                }
                else
                {
                    ProcessRegularItem(lootBase, id, position, transform);
                }
            }
            catch
            {
                // Skip invalid loose loot
            }
        }

        private void ProcessQuestItem(ulong lootBase, ulong itemTemplate, string id, Vector3 position, UnityTransform transform)
        {
            var shortNamePtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate.ShortName);
            var shortName = Memory.ReadUnicodeString(shortNamePtr, LootConstants.MaxShortNameLength);
            DebugLogger.LogDebug(shortName);
            _ = _loot.TryAdd(lootBase, new LootItem(id, $"Q_{shortName}", position, transform, isQuestItem: true));
        }

        private void ProcessRegularItem(ulong lootBase, string id, Vector3 position, UnityTransform transform)
        {
            if (TarkovDataManager.AllItems.TryGetValue(id, out var entry))
            {
                _ = _loot.TryAdd(lootBase, new LootItem(entry, position, transform));
            }
        }

        private enum LootType
        {
            Unknown,
            Corpse,
            Container,
            LooseLoot
        }
    }
}
