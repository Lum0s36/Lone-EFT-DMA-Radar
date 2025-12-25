using System.Linq;
using static LoneEftDmaRadar.Tarkov.Unity.UnitySDK;

namespace SDK
{
    public readonly partial struct Offsets
    {
        public readonly partial struct GameWorld
        {
            public const uint BtrController = 0x20; // EFT.Vehicle.BtrController
            public const uint LocationId = 0xC8; // string
            public const uint ExfiltrationController = 0x50; // EFT.Interactive.ExfiltrationController
            public const uint LootList = 0x190; // System.Collections.Generic.List<IKillable>
            public const uint RegisteredPlayers = 0x1B0; // System.Collections.Generic.List<IPlayer>
            public const uint MainPlayer = 0x208; // EFT.Player
            public const uint SynchronizableObjectLogicProcessor = 0x240; // EFT.SynchronizableObjects.SynchronizableObjectLogicProcessor
            public const uint Grenades = 0x280; // DictionaryListHydra<int, Throwable>
        }

        public readonly partial struct ExfiltrationController
        {
            public const uint SecretExfilitrationController = 0x18; // EFT.Interactive.SecretExfiltrations.SecretExfilitranionController
            public const uint ExfiltrationPoints = 0x20; // EFT.Interactive.ExfiltrationPoint[]
            public const uint ScavExfiltrationPoints = 0x28; // EFT.Interactive.ScavExfiltrationPoint[]
            public const uint SecretExfiltrationPoints = 0x30; // EFT.Interactive.SecretExfiltrations.SecretExfiltrationPoint[]
        }

        public readonly partial struct ExfiltrationPoint
        {
            public const uint _status = 0x58; // EFT.Interactive.EExfiltrationStatus
            public const uint Settings = 0x98; // EFT.Interactive.ExitTriggerSettings
            public const uint EligibleEntryPoints = 0xC0; // string[]
            public const uint EligibleIds = 0xF8; // System.Collections.Generic.List<string>
        }

        public readonly partial struct ExitTriggerSettings
        {
            public const uint Id = 0x10; // string
            public const uint Name = 0x18; // string
            public const uint EntryPoints = 0x40; // string
        }

        public readonly partial struct SynchronizableObject
        {
            public const uint Type = 0x68; // System.Int32
        }

        public readonly partial struct SynchronizableObjectLogicProcessor
        {
            public const uint _staticSynchronizableObjects = 0x18; // System.Collections.Generic.List<SynchronizableObject>
        }

        public readonly partial struct TripwireSynchronizableObject
        {
            public const uint _tripwireState = 0xE4; // System.Int32
            public const uint ToPosition = 0x158; // UnityEngine.Vector3
        }

        public readonly partial struct BtrController
        {
            public const uint BtrView = 0x50; // EFT.Vehicle.BTRView
        }

        public readonly partial struct BTRView
        {
            public const uint turret = 0x60; // EFT.Vehicle.BTRTurretView
            public const uint _targetPosition = 0xAC; // UnityEngine.Vector3
            public const uint _previousPosition = 0xB4; // UnityEngine.Vector3
        }

        public readonly partial struct BTRTurretView
        {
            public const uint _bot = 0x60; // System.ValueTuple<ObservedPlayerView, Boolean>
        }

        public readonly partial struct Throwable
        {
            public const uint _isDestroyed = 0x4D; // Boolean
        }

        public readonly partial struct Player
        {
            public const uint MovementContext = 0x60; // EFT.MovementContext
            public const uint _playerBody = 0x190; // EFT.PlayerBody
            public const uint Physical = 0x8F8; // -.\uE399 <Physical> Physical
            public const uint Corpse = 0x680; // EFT.Interactive.Corpse
            public const uint Location = 0x870; // String
            public const uint Profile = 0x900; // EFT.Profile
            public const uint ProceduralWeaponAnimation = 0x338; // EFT.Animations.ProceduralWeaponAnimation
            public const uint _inventoryController = 0x958; // EFT.PlayerInventoryController
            public const uint _handsController = 0x960; // EFT.PlayerHands
            public const uint _playerLookRaycastTransform = 0xA08; // UnityEngine.Transform
        }

        public readonly partial struct ObservedPlayerView
        {
            public const uint ObservedPlayerController = 0x28; // EFT.NextObservedPlayer.ObservedPlayerController
            public const uint Voice = 0x40; // string
            public const uint Id = 0x7C; // int32_t - unique in-memory player ID
            public const uint GroupID = 0x80; // string
            public const uint Side = 0x94; // EFT.EPlayerSide
            public const uint IsAI = 0xA0; // bool
            public const uint AccountId = 0xC0; // string
            public const uint PlayerBody = 0xD8; // EFT.PlayerBody
        }

        public readonly partial struct ObservedPlayerController
        {
            public const uint InventoryController = 0x10; // EFT.NextObservedPlayer.ObservedPlayerInventoryController
            public const uint PlayerView = 0x18; // EFT.NextObservedPlayer.ObservedPlayerView
            public const uint MovementController = 0xD8; // EFT.NextObservedPlayer.ObservedPlayerMovementController
            public const uint HealthController = 0xE8; // ObservedPlayerHealthController
        }

        public readonly partial struct InventoryController
        {
            public const uint Inventory = 0x100; // EFT.InventoryLogic.Inventory
        }

        public readonly partial struct Inventory
        {
            public const uint Equipment = 0x18; // EFT.InventoryLogic.InventoryEquipment
        }

        public readonly partial struct InventoryEquipment
        {
            public const uint _cachedSlots = 0x90; // EFT.InventoryLogic.Slot[]
        }

        public readonly partial struct Slot
        {
            public const uint ContainedItem = 0x48; // EFT.InventoryLogic.Item
            public const uint ID = 0x58; // String
            public const uint Required = 0x18; // Boolean
        }

        public readonly partial struct ObservedPlayerMovementController
        {
            public const uint ObservedPlayerStateContext = 0x98; // object
        }

        public readonly partial struct ObservedPlayerStateContext
        {
            public const uint Rotation = 0x20; // UnityEngine.Vector2
        }

        public readonly partial struct ObservedHealthController
        {
            public const uint HealthStatus = 0x10; // ETagStatus
            public const uint _player = 0x18; // EFT.NextObservedPlayer.ObservedPlayerView
            public const uint _playerCorpse = 0x20; // EFT.Interactive.ObservedCorpse
        }

        public readonly partial struct Profile
        {
            public const uint Id = 0x10; // String
            public const uint AccountId = 0x18; // String
            public const uint Info = 0x48; // object
            public const uint TaskConditionCounters = 0x90; // Dictionary<MongoID, TaskConditionCounter>
            public const uint QuestsData = 0x98; // System.Collections.Generic.List<QuestStatusData>
            public const uint WishlistManager = 0x108; // EFT.WishlistManager
        }

        public readonly partial struct WishlistManager
        {
            public const uint UserItems = 0x28; // GenericInst _userItems - Dictionary<MongoID, Int32>
            public const uint WishlistItems = 0x30; // GenericInst _wishlistItems
        }

        public readonly partial struct PlayerInfo
        {
            public const uint EntryPoint = 0x28; // String
            public const uint Side = 0x48; // Int32
            public const uint RegistrationDate = 0x4C; // Int32
            public const uint GroupId = 0x50; // String
        }

        public readonly partial struct QuestsData
        {
            public const uint Id = 0x10; // string
            public const uint Status = 0x1C; // object
            public const uint CompletedConditions = 0x28; // object
        }

        public readonly partial struct MovementContext
        {
            public const uint Player = 0x48; // EFT.Player
            public const uint _player = 0x48; // EFT.Player (alias)
            public const uint _rotation = 0xC8; // UnityEngine.Vector2
            public const uint PlantState = 0x78; // EFT.BaseMovementState
            public const uint CurrentState = 0x1F0; // EFT.BaseMovementState
            public const uint _states = 0x480; // Dictionary<Byte, BaseMovementState>
            public const uint _movementStates = 0x4b0; // IPlayerStateContainerBehaviour[]
            public const uint _tilt = 0xb4; // Single
            public const uint _physicalCondition = 0x198; // Int32
            public const uint _speedLimitIsDirty = 0x1b9; // Boolean
            public const uint StateSpeedLimit = 0x1bc; // Single
            public const uint StateSprintSpeedLimit = 0x1c0; // Single
            public const uint _lookDirection = 0x3b8; // Vector3
        }

        public readonly partial struct MovementState
        {
            public const uint StickToGround = 0x54; // Boolean
            public const uint PlantTime = 0x58; // Single
            public const uint Name = 0x11; // Byte
            public const uint AnimatorStateHash = 0x20; // Int32
        }

        public readonly partial struct PlayerStateContainer
        {
            public const uint Name = 0x19; // Byte
            public const uint StateFullNameHash = 0x40; // Int32
        }

        public readonly partial struct InteractiveLootItem
        {
            public const uint Item = 0xF0; // EFT.InventoryLogic.Item
            public const uint _item = 0xF0; // alias
        }

        public readonly partial struct DizSkinningSkeleton
        {
            public const uint _values = 0x30; // System.Collections.Generic.List<Transform>
        }

        public readonly partial struct LootableContainer
        {
            public const uint ItemOwner = 0x168; // object
            public const uint InteractingPlayer = 0x150; // Object
        }

        public readonly partial struct LootableContainerItemOwner
        {
            public const uint RootItem = 0xD0; // EFT.InventoryLogic.Item
        }

        public readonly partial struct ItemController
        {
            public const uint RootItem = 0xD0; // EFT.InventoryLogic.Item
        }

        public readonly partial struct LootItem
        {
            public const uint Template = 0x60; // EFT.InventoryLogic.ItemTemplate
            public const uint Version = 0x7C; // Int32
        }

        public readonly partial struct ItemTemplate
        {
            public const uint ShortName = 0x18; // String
            public const uint QuestItem = 0x34; // Boolean
            public const uint _id = 0xE0; // EFT.MongoID
        }

        public readonly partial struct PlayerBody
        {
            public const uint SkeletonRootJoint = 0x30; // object
        }

        public readonly partial struct FirearmController
        {
            public const uint WeaponAnimation = 0x198; // EFT.Animations.ProceduralWeaponAnimation
            public const uint Fireport = 0x150; // EFT.BifacialTransform
            public const uint TotalCenterOfImpact = 0x2a0; // Single
            public static readonly uint[] To_FirePortTransformInternal = new uint[] { Fireport, 0x10, 0x10 };
            public static readonly uint[] To_FirePortVertices = To_FirePortTransformInternal.Concat(new uint[] { UnityOffsets.TransformAccess_HierarchyOffset, UnityOffsets.Hierarchy_VerticesOffset }).ToArray();
        }

        public readonly partial struct ProceduralWeaponAnimation
        {
            public const uint HandsContainer = 0x20; // EFT.Animations.PlayerSpring
            public const uint Breath = 0x38; // EFT.Animations.BreathEffector
            public const uint MotionReact = 0x48; // MotionEffector
            public const uint Shootingg = 0x58; // ShotEffector
            public const uint _optics = 0x180; // List<SightNBone>
            public const uint Mask = 0x30; // Int32
            public const uint IsAiming = 0x145; // Boolean
            public const uint _isAiming = 0x145; // Boolean
            public const uint _fieldOfView = 0xA8; // Float
            public const uint _aimingSpeed = 0x164; // Single
            public const uint _fovCompensatoryDistance = 0x194; // Single
            public const uint _compensatoryScale = 0x1c4; // Single
            public const uint _shotDirection = 0x1c8; // Vector3
            public const uint CameraSmoothOut = 0x20c; // Single
            public const uint PositionZeroSum = 0x31c; // Vector3
            public const uint ShotNeedsFovAdjustments = 0x433; // Boolean
        }

        public readonly partial struct SightNBone
        {
            public const uint Mod = 0x10; // EFT.InventoryLogic.SightComponent
        }

        public readonly partial struct SightComponent
        {
            public const uint _template = 0x20; // EFT.InventoryLogic.ISightComponentTemplate
            public const uint ScopesSelectedModes = 0x30; // Int32[]
            public const uint SelectedScope = 0x38; // Int32
            public const uint ScopeZoomValue = 0x3C; // Single
        }

        public readonly partial struct SightInterface
        {
            public const uint Zooms = 0x1B8; // Single[]
        }

        public readonly partial struct Physical
        {
            public const uint Stamina = 0x68; // Stamina
            public const uint HandsStamina = 0x70; // Stamina
            public const uint Oxygen = 0x78; // Stamina
            public const uint Overweight = 0x1c; // Single
            public const uint WalkOverweight = 0x20; // Single
            public const uint WalkSpeedLimit = 0x24; // Single
            public const uint Inertia = 0x28; // Single
            public const uint WalkOverweightLimits = 0xa4; // Vector2
            public const uint BaseOverweightLimits = 0xac; // Vector2
            public const uint SprintOverweightLimits = 0xc0; // Vector2
            public const uint SprintWeightFactor = 0x104; // Single
            public const uint SprintAcceleration = 0x114; // Single
            public const uint PreSprintAcceleration = 0x118; // Single
            public const uint IsOverweightA = 0x11C; // Boolean
            public const uint IsOverweightB = 0x11D; // Boolean
        }

        public readonly partial struct PhysicalValue
        {
            public const uint Current = 0x10; // Single
        }

        public readonly partial struct BreathEffector
        {
            public const uint Intensity = 0x30; // Single
        }

        public readonly partial struct ShotEffector
        {
            public const uint NewShotRecoil = 0x20; // NewRecoilShotEffect
        }

        public readonly partial struct NewShotRecoil
        {
            public const uint IntensitySeparateFactors = 0x94; // Vector3
        }

        public readonly partial struct ItemHandsController
        {
            public const uint Item = 0x70; // EFT.InventoryLogic.Item
        }

        public readonly partial struct LootItemWeapon
        {
            public const uint FireMode = 0xa0; // FireModeComponent
            public const uint Chambers = 0xb0; // Slot[]
            public const uint _magSlotCache = 0xc8; // Slot
        }

        public readonly partial struct LootItemMagazine
        {
            public const uint Cartridges = 0x1a8; // StackSlot
            public const uint LoadUnloadModifier = 0x1b0; // Single
        }

        public readonly partial struct StackSlot
        {
            public const uint _items = 0x10; // List<Item>
            public const uint MaxCount = 0x38; // Int32
        }

        public readonly partial struct LootItemMod
        {
            public const uint Grids = 0x78; // object[]
            public const uint Slots = 0x80; // Slot[]
        }

        public readonly partial struct ModTemplate
        {
            public const uint Velocity = 0x188; // Single
        }

        public readonly partial struct AmmoTemplate
        {
            public const uint InitialSpeed = 0x1a4; // Single
            public const uint BallisticCoeficient = 0x1b8; // Single
            public const uint BulletMassGram = 0x25c; // Single
            public const uint BulletDiameterMilimeters = 0x260; // Single
        }

        public readonly partial struct WeaponTemplate
        {
            public const uint Velocity = 0x254; // Single
        }

        public readonly partial struct ObservedMovementController
        {
            public const uint ObservedPlayerStateContext = 0x98;
            public const uint Rotation = 0x1c; // Vector2
            public const uint Velocity = 0x30; // Vector3
        }

        public readonly partial struct QuestStatusData
        {
            public const uint Id = 0x10; // String
            public const uint StartTime = 0x18; // Int32
            public const uint Status = 0x1C; // Int32
            public const uint StatusStartTimestamps = 0x20; // object
            public const uint CompletedConditions = 0x28; // HashSet<MongoID>
            public const uint AvailableAfter = 0x30; // Int32
        }

        public readonly partial struct TaskConditionCounter
        {
            public const uint OnValueChanged = 0x10; // Action
            public const uint Id = 0x18; // MongoID
            public const uint Type = 0x30; // String
            public const uint SourceId = 0x38; // String
            public const uint Value = 0x40; // Int32
            public const uint Template = 0x48; // Condition
            public const uint Conditional = 0x50; // IConditional
        }

        public readonly partial struct Condition
        {
            public const uint Id = 0x10; // MongoID
            public const uint Value = 0x28; // Single
            public const uint CompareMethod = 0x2C; // Int32
            public const uint VisibilityConditions = 0x30; // Condition[]
            public const uint Index = 0x38; // Int32
            public const uint ParentId = 0x40; // Nullable<MongoID>
            public const uint DynamicLocale = 0x60; // Boolean
            public const uint IsNecessary = 0x61; // Boolean
            public const uint Identity = 0x64; // Int32
            public const uint ChildConditions = 0x68; // UpdatableBindableList<Condition>
            public const uint ResetOnSessionEnd = 0x70; // Boolean
            public const uint QuestNoteId = 0x78; // Nullable<MongoID>
        }

        public readonly partial struct TriggerWithId
        {
            public const uint Id = 0x18; // String
            public const uint Description = 0x20; // String
        }
    }

    public readonly partial struct Enums
    {
        public enum EPlayerSide
        {
            Usec = 1,
            Bear = 2,
            Savage = 4,
        }

        public enum EPlayerState : byte
        {
            None = 0,
            Idle = 1,
            ProneIdle = 2,
            ProneMove = 3,
            Run = 4,
            Sprint = 5,
            Jump = 6,
            FallDown = 7,
            Transition = 8,
            BreachDoor = 9,
            Loot = 10,
            Pickup = 11,
            Open = 12,
            Close = 13,
            Unlock = 14,
            Sidestep = 15,
            DoorInteraction = 16,
            Approach = 17,
            Prone2Stand = 18,
            Transit2Prone = 19,
            Plant = 20,
            Stationary = 21,
            Roll = 22,
            JumpLanding = 23,
            ClimbOver = 24,
            ClimbUp = 25,
            VaultingFallDown = 26,
            VaultingLanding = 27,
            BlindFire = 28,
            IdleWeaponMounting = 29,
            IdleZombieState = 30,
            MoveZombieState = 31,
            TurnZombieState = 32,
            StartMoveZombieState = 33,
            EndMoveZombieState = 34,
            DoorInteractionZombieState = 35,
        }

        [Flags]
        public enum ETagStatus
        {
            Unaware = 1,
            Aware = 2,
            Combat = 4,
            Solo = 8,
            Coop = 16,
            Bear = 32,
            Usec = 64,
            Scav = 128,
            TargetSolo = 256,
            TargetMultiple = 512,
            Healthy = 1024,
            Injured = 2048,
            BadlyInjured = 4096,
            Dying = 8192,
            Birdeye = 16384,
            Knight = 32768,
            BigPipe = 65536,
            BlackDivision = 131072,
            VSRF = 262144,
        }

        [Flags]
        public enum EMemberCategory
        {
            Default = 0,
            Developer = 1,
            UniqueId = 2,
            Trader = 4,
            Group = 8,
            System = 16,
            ChatModerator = 32,
            ChatModeratorWithPermanentBan = 64,
            UnitTest = 128,
            Sherpa = 256,
            Emissary = 512,
            Unheard = 1024,
        }

        public enum EExfiltrationStatus
        {
            NotPresent = 1,
            UncompleteRequirements = 2,
            Countdown = 3,
            RegularMode = 4,
            Pending = 5,
            AwaitsManualActivation = 6,
            Hidden = 7,
        }

        public enum SynchronizableObjectType
        {
            AirDrop = 0,
            AirPlane = 1,
            Tripwire = 2,
        }

        public enum ETripwireState
        {
            None = 0,
            Wait = 1,
            Active = 2,
            Exploding = 3,
            Exploded = 4,
            Inert = 5,
        }

        public enum EQuestStatus
        {
            Locked = 0,
            AvailableForStart = 1,
            Started = 2,
            AvailableForFinish = 3,
            Success = 4,
            Fail = 5,
            FailRestartable = 6,
            MarkedAsFailed = 7,
            Expired = 8,
            AvailableAfter = 9,
        }
    }
}
