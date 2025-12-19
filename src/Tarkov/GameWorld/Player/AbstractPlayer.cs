using Collections.Pooled;
using LoneEftDmaRadar.DMA;
using LoneEftDmaRadar.Misc;
using LoneEftDmaRadar.Tarkov.GameWorld.Loot;
using LoneEftDmaRadar.Tarkov.GameWorld.Player.Helpers;
using LoneEftDmaRadar.Tarkov.GameWorld.Player.Rendering;
using LoneEftDmaRadar.Tarkov.Unity;
using LoneEftDmaRadar.Tarkov.Unity.Structures;
using LoneEftDmaRadar.UI.Misc;
using LoneEftDmaRadar.UI.Radar.Maps;
using LoneEftDmaRadar.UI.Radar.ViewModels;
using LoneEftDmaRadar.UI.Skia;
using LoneEftDmaRadar.Web.TarkovDev.Data;
using VmmSharpEx.Scatter;
using static LoneEftDmaRadar.Tarkov.Unity.Structures.UnityTransform;

namespace LoneEftDmaRadar.Tarkov.GameWorld.Player
{
    /// <summary>
    /// Base class for Tarkov Players.
    /// Tarkov implements several distinct classes that implement a similar player interface.
    /// </summary>
    public abstract class AbstractPlayer : IWorldEntity, IMapEntity, IMouseoverEntity
    {
        #region Static Interfaces

        public static implicit operator ulong(AbstractPlayer x) => x.Base;
        protected static readonly ConcurrentDictionary<string, int> _groups = new(StringComparer.OrdinalIgnoreCase);
        protected static int _lastGroupNumber;
        protected static int _lastPscavNumber;

        static AbstractPlayer()
        {
            MemDMA.RaidStopped += MemDMA_RaidStopped;
        }

        private static void MemDMA_RaidStopped(object sender, EventArgs e)
        {
            _groups.Clear();
            _lastGroupNumber = default;
            _lastPscavNumber = default;
        }

        #endregion

        #region Allocation

        /// <summary>
        /// Allocates a player.
        /// </summary>
        /// <param name="regPlayers">Player Dictionary collection to add the newly allocated player to.</param>
        /// <param name="playerBase">Player base memory address.</param>
        public static void Allocate(ConcurrentDictionary<ulong, AbstractPlayer> regPlayers, ulong playerBase)
        {
            try
            {
                _ = regPlayers.GetOrAdd(
                    playerBase,
                    addr => AllocateInternal(addr));
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"ERROR during Player Allocation for player @ 0x{playerBase.ToString("X")}: {ex}");
            }
        }

        private static AbstractPlayer AllocateInternal(ulong playerBase)
        {
            AbstractPlayer player;
            var className = ObjectClass.ReadName(playerBase, PlayerConstants.MaxClassNameLength);
            var isClientPlayer = className == PlayerConstants.ClientPlayerClassName || 
                                 className == PlayerConstants.LocalPlayerClassName;

            if (isClientPlayer)
                player = new ClientPlayer(playerBase);
            else
                player = new ObservedPlayer(playerBase);
            DebugLogger.LogDebug($"Player '{player.Name}' allocated.");
            return player;
        }

        /// <summary>
        /// Player Constructor.
        /// </summary>
        protected AbstractPlayer(ulong playerBase)
        {
            ArgumentOutOfRangeException.ThrowIfZero(playerBase, nameof(playerBase));
            Base = playerBase;
        }

        #endregion

        #region Fields / Properties
        /// <summary>
        /// Player Class Base Address
        /// </summary>
        public ulong Base { get; }

        /// <summary>
        /// True if the Player is Active (in the player list).
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Type of player unit.
        /// </summary>
        public virtual PlayerType Type { get; protected set; }

        private Vector2 _rotation;
        /// <summary>
        /// Player's Rotation in Local Game World.
        /// </summary>
        public Vector2 Rotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rotation;
            private set
            {
                _rotation = value;
                float mapRotation = value.X;
                mapRotation -= 90f;
                while (mapRotation < 0f)
                    mapRotation += 360f;
                MapRotation = mapRotation;
            }
        }

        /// <summary>
        /// Player's Map Rotation (with 90 degree correction applied).
        /// </summary>
        public float MapRotation { get; private set; }

        /// <summary>
        /// Corpse field value.
        /// </summary>
        public ulong? Corpse { get; private set; }

        /// <summary>
        /// Player's Skeleton Root.
        /// </summary>
        public UnityTransform SkeletonRoot { get; protected set; }

        /// <summary>
        /// Dictionary of Player Bones.
        /// </summary>
        public ConcurrentDictionary<Bones, UnityTransform> PlayerBones { get; } = new();
        
        /// <summary>
        /// Lightweight wrapper for skeleton access (used by DeviceAimbot/silent aim features).
        /// </summary>
        public PlayerSkeleton Skeleton { get; protected set; }
        protected int _verticesCount;
        private bool _skeletonErrorLogged;
        protected Vector3 _cachedPosition;

        /// <summary>
        /// TRUE if critical memory reads (position/rotation) have failed.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Timer to track how long player has been in error state.
        /// Used to trigger re-allocation if errors persist.
        /// </summary>
        public Stopwatch ErrorTimer { get; } = new Stopwatch();

        /// <summary>
        /// True if player is being focused via Right-Click (UI).
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Dead Player's associated loot container object.
        /// </summary>
        public LootCorpse LootObject { get; set; }
        
        /// <summary>
        /// Alerts for this Player Object.
        /// </summary>
        public virtual string Alerts { get; protected set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Player name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Account UUID for Human Controlled Players.
        /// </summary>
        public virtual string AccountID { get; }

        /// <summary>
        /// Group that the player belongs to.
        /// </summary>
        public virtual int GroupID { get; protected set; } = -1;

        /// <summary>
        /// Player's Faction.
        /// </summary>
        public virtual Enums.EPlayerSide PlayerSide { get; protected set; }

        /// <summary>
        /// Player is Human-Controlled.
        /// </summary>
        public virtual bool IsHuman { get; }

        /// <summary>
        /// MovementContext / StateContext
        /// </summary>
        public virtual ulong MovementContext { get; }

        /// <summary>
        /// Corpse field address.
        /// </summary>
        public virtual ulong CorpseAddr { get; }

        /// <summary>
        /// Player Rotation Field Address (view angles).
        /// </summary>
        public virtual ulong RotationAddress { get; }

        /// <summary>
        /// Hands Controller address.
        /// </summary>
        protected ulong HandsController { get; set; }
        
        #endregion

        #region Boolean Getters

        public bool IsAI => !IsHuman;
        public bool IsPmc => PlayerSide is Enums.EPlayerSide.Usec || PlayerSide is Enums.EPlayerSide.Bear;
        public bool IsScav => PlayerSide is Enums.EPlayerSide.Savage;
        public bool IsAlive => Corpse is null;
        public bool IsFriendly => this is LocalPlayer || Type is PlayerType.Teammate;
        public bool IsHostile => !IsFriendly;
        public bool IsNotLocalPlayerAlive => this is not LocalPlayer && IsActive && IsAlive;
        public bool IsHostilePmc => IsPmc && IsHostile;
        public bool IsHumanOther => IsHuman && this is not LocalPlayer;
        public bool IsAIActive => IsAI && IsActive && IsAlive;
        public bool IsDefaultAIActive => IsAI && Name == PlayerConstants.DefaultAIName && IsActive && IsAlive;
        public bool IsHumanActive => IsHuman && IsActive && IsAlive;
        public bool IsHostileActive => IsHostile && IsActive && IsAlive;
        public bool IsHumanHostile => IsHuman && IsHostile;
        public bool IsHumanHostileActive => IsHumanHostile && IsActive && IsAlive;
        public bool IsFriendlyActive => IsFriendly && IsActive && IsAlive;
        public bool HasExfild => !IsActive && IsAlive;

        #endregion

        #region Methods

        private readonly Lock _alertsLock = new();
        
        /// <summary>
        /// Update the Alerts for this Player Object.
        /// </summary>
        public void UpdateAlerts(string alert)
        {
            if (alert is null)
                return;
            lock (_alertsLock)
            {
                if (Alerts is null)
                    Alerts = alert;
                else
                    Alerts = $"{alert} | {Alerts}";
            }
        }

        /// <summary>
        /// Validates the Rotation Address.
        /// </summary>
        protected static ulong ValidateRotationAddr(ulong rotationAddr)
        {
            var rotation = Memory.ReadValue<Vector2>(rotationAddr, false);
            if (!rotation.IsNormalOrZero() ||
                Math.Abs(rotation.X) > PlayerConstants.MaxRotationX ||
                Math.Abs(rotation.Y) > PlayerConstants.MaxRotationY)
                throw new ArgumentOutOfRangeException(nameof(rotationAddr));

            return rotationAddr;
        }

        /// <summary>
        /// Refreshes non-realtime player information.
        /// </summary>
        public virtual void OnRegRefresh(VmmScatter scatter, ISet<ulong> registered, bool? isActiveParam = null)
        {
            if (isActiveParam is not bool isActive)
                isActive = registered.Contains(this);
            if (isActive)
            {
                SetAlive();
            }
            else if (IsAlive)
            {
                scatter.PrepareReadPtr(CorpseAddr);
                scatter.Completed += (sender, x1) =>
                {
                    if (x1.ReadPtr(CorpseAddr, out var corpsePtr))
                        SetDead(corpsePtr);
                    else
                        SetExfild();
                };
            }
        }

        public void SetDead(ulong corpse)
        {
            Corpse = corpse;
            IsActive = false;
        }

        private void SetExfild()
        {
            Corpse = null;
            IsActive = false;
        }

        private void SetAlive()
        {
            Corpse = null;
            LootObject = null;
            IsActive = true;
        }

        /// <summary>
        /// Executed on each Realtime Loop.
        /// </summary>
        public virtual void OnRealtimeLoop(VmmScatter scatter)
        {
            if (SkeletonRoot == null)
            {
                IsError = true;
                return;
            }

            int vertexCount = SkeletonRoot.Count;
            int maxBoneRequirement = 0;
            foreach (var bone in PlayerBones.Values)
            {
                if (bone.Count > maxBoneRequirement)
                    maxBoneRequirement = bone.Count;
            }
            int actualRequired = Math.Max(vertexCount, maxBoneRequirement);
            
            if (actualRequired <= 0 || actualRequired > PlayerConstants.MaxVertexCount)
            {
                if (!TryRecoverSkeleton(ref actualRequired))
                {
                    IsError = true;
                    _verticesCount = 0;
                    return;
                }
            }

            scatter.PrepareReadValue<Vector2>(RotationAddress);
            int requestedVertices = _verticesCount > 0 ? _verticesCount : actualRequired;
            scatter.PrepareReadArray<TrsX>(SkeletonRoot.VerticesAddr, requestedVertices);

            scatter.Completed += (sender, s) =>
            {
                bool successRot = s.ReadValue<Vector2>(RotationAddress, out var rotation) && SetRotation(rotation);
                bool successPos = false;

                if (s.ReadArray<TrsX>(SkeletonRoot.VerticesAddr, requestedVertices) is PooledMemory<TrsX> vertices)
                {
                    using (vertices)
                    {
                        try
                        {
                            if (vertices.Span.Length >= requestedVertices)
                            {
                                _ = SkeletonRoot.UpdatePosition(vertices.Span);
                                successPos = true;

                                foreach (var bonePair in PlayerBones)
                                {
                                    try
                                    {
                                        if (bonePair.Value.Count <= vertices.Span.Length)
                                            bonePair.Value.UpdatePosition(vertices.Span);
                                        else
                                        {
                                            DebugLogger.LogDebug($"Bone '{bonePair.Key}' needs {bonePair.Value.Count} vertices but only {vertices.Span.Length} available for '{Name}'");
                                            ResetBoneTransform(bonePair.Key);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugLogger.LogDebug($"ERROR updating bone '{bonePair.Key}' for '{Name}': {ex}");
                                        ResetBoneTransform(bonePair.Key);
                                    }
                                }

                                _cachedPosition = SkeletonRoot.Position;
                                if (_skeletonErrorLogged)
                                {
                                    DebugLogger.LogDebug($"Skeleton update successful for Player '{Name}'");
                                    _skeletonErrorLogged = false;
                                }
                            }
                            else
                            {
                                DebugLogger.LogDebug($"Insufficient vertices for '{Name}': got {vertices.Span.Length}, expected {requestedVertices}");
                                _verticesCount = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.LogDebug($"ERROR updating skeleton position for '{Name}': {ex}");
                        }
                    }
                }

                bool hasError = !successRot || !successPos;
                if (hasError && !IsError)
                    ErrorTimer.Restart();
                else if (!hasError && IsError)
                {
                    ErrorTimer.Stop();
                    ErrorTimer.Reset();
                }
                IsError = hasError;
            };
        }

        private bool TryRecoverSkeleton(ref int actualRequired)
        {
            try
            {
                DebugLogger.LogDebug($"Invalid vertex count detected for '{Name}': {actualRequired}");
                foreach (var bone in PlayerBones.Keys.ToList())
                    ResetBoneTransform(bone);
                Skeleton = new PlayerSkeleton(SkeletonRoot, PlayerBones);
                DebugLogger.LogDebug($"Fast skeleton recovery for Player '{Name}'");

                int vertexCount = SkeletonRoot.Count;
                int maxBoneRequirement = 0;
                foreach (var bone in PlayerBones.Values)
                    if (bone.Count > maxBoneRequirement) maxBoneRequirement = bone.Count;
                actualRequired = Math.Max(vertexCount, maxBoneRequirement);
                
                return actualRequired > 0 && actualRequired <= PlayerConstants.MaxVertexCount;
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"ERROR in fast skeleton recovery for '{Name}': {ex}");
                return false;
            }
        }

        /// <summary>
        /// Executed on each Transform Validation Loop.
        /// </summary>
        public void OnValidateTransforms(VmmScatter round1, VmmScatter round2)
        {
            round1.PrepareReadPtr(SkeletonRoot.TransformInternal + UnitySDK.UnityOffsets.TransformAccess_HierarchyOffset);
            round1.Completed += (sender, x1) =>
            {
                if (x1.ReadPtr(SkeletonRoot.TransformInternal + UnitySDK.UnityOffsets.TransformAccess_HierarchyOffset, out var tra))
                {
                    round2.PrepareReadPtr(tra + UnitySDK.UnityOffsets.Hierarchy_VerticesOffset);
                    round2.Completed += (sender, x2) =>
                    {
                        if (x2.ReadPtr(tra + UnitySDK.UnityOffsets.Hierarchy_VerticesOffset, out var verticesPtr))
                        {
                            if (SkeletonRoot.VerticesAddr != verticesPtr)
                            {
                                DebugLogger.LogDebug($"WARNING - SkeletonRoot Transform has changed for Player '{Name}'");
                                RebuildSkeleton();
                            }
                        }
                    };
                }
            };

            OnValidateTransforms();
        }

        private void RebuildSkeleton()
        {
            var transform = new UnityTransform(SkeletonRoot.TransformInternal);
            SkeletonRoot = transform;
            _verticesCount = 0;
            try
            {
                foreach (var bone in PlayerBones.Keys.ToList())
                    ResetBoneTransform(bone);
                Skeleton = new PlayerSkeleton(SkeletonRoot, PlayerBones);
                DebugLogger.LogDebug($"Skeleton rebuilt for Player '{Name}'");
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"ERROR rebuilding skeleton for '{Name}': {ex}");
            }
        }

        public virtual void OnValidateTransforms()
        {
        }

        protected virtual bool SetRotation(Vector2 rotation)
        {
            try
            {
                rotation.ThrowIfAbnormalAndNotZero(nameof(rotation));
                rotation.X = rotation.X.NormalizeAngle();
                ArgumentOutOfRangeException.ThrowIfLessThan(rotation.X, 0f);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(rotation.X, PlayerConstants.MaxRotationX);
                ArgumentOutOfRangeException.ThrowIfLessThan(rotation.Y, -PlayerConstants.MaxRotationY);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(rotation.Y, PlayerConstants.MaxRotationY);
                Rotation = rotation;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region AI Player Types (Delegated to AIRoleLookup)

        /// <summary>
        /// AI role information struct (for backwards compatibility).
        /// </summary>
        public readonly struct AIRole
        {
            public readonly string Name { get; init; }
            public readonly PlayerType Type { get; init; }
        }

        /// <summary>
        /// Lookup AI Info based on Voice Line.
        /// </summary>
        public static AIRole GetAIRoleInfo(string voiceLine)
        {
            var result = AIRoleLookup.GetRoleInfo(voiceLine);
            return new AIRole { Name = result.Name, Type = result.Type };
        }

        #endregion

        #region Interfaces (Delegated to PlayerRenderer)

        public virtual ref readonly Vector3 Position
        {
            get
            {
                var skeletonPos = SkeletonRoot.Position;
                if (skeletonPos != Vector3.Zero && !float.IsNaN(skeletonPos.X) && !float.IsInfinity(skeletonPos.X))
                {
                    _cachedPosition = skeletonPos;
                    return ref SkeletonRoot.Position;
                }
                return ref _cachedPosition;
            }
        }

        public Vector2 MouseoverPosition { get; set; }

        public void Draw(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            try
            {
                PlayerRenderer.Draw(this, canvas, mapParams, localPlayer);
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"WARNING! Player Draw Error: {ex}");
            }
        }

        public void DrawMouseover(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            PlayerRenderer.DrawMouseover(this, canvas, mapParams, localPlayer);
        }

        #endregion

        #region High Alert

        /// <summary>
        /// True if Current Player is facing target.
        /// </summary>
        public bool IsFacingTarget(AbstractPlayer target, float? maxDist = null)
        {
            Vector3 delta = target.Position - this.Position;

            if (maxDist is float m)
            {
                float maxDistSq = m * m;
                float distSq = Vector3.Dot(delta, delta);
                if (distSq > maxDistSq) return false;
            }

            float distance = delta.Length();
            if (distance <= 1e-6f)
                return true;

            Vector3 fwd = RotationToDirection(this.Rotation);
            float cosAngle = Vector3.Dot(fwd, delta) / distance;

            float x = MathF.Abs(PlayerConstants.FacingAngleCoeffC - PlayerConstants.FacingAngleCoeffD * distance);
            float angleDeg = PlayerConstants.FacingAngleCoeffA - PlayerConstants.FacingAngleCoeffB * MathF.Log(MathF.Max(x, 1e-6f));
            if (angleDeg < PlayerConstants.MinFacingAngle) angleDeg = PlayerConstants.MinFacingAngle;
            if (angleDeg > PlayerConstants.MaxFacingAngle) angleDeg = PlayerConstants.MaxFacingAngle;

            float cosThreshold = MathF.Cos(angleDeg * (MathF.PI / 180f));
            return cosAngle >= cosThreshold;

            static Vector3 RotationToDirection(Vector2 rotation)
            {
                float yaw = rotation.X * (MathF.PI / 180f);
                float pitch = rotation.Y * (MathF.PI / 180f);

                float cp = MathF.Cos(pitch);
                float sp = MathF.Sin(pitch);
                float sy = MathF.Sin(yaw);
                float cy = MathF.Cos(yaw);

                var dir = new Vector3(cp * sy, -sp, cp * cy);

                float lenSq = Vector3.Dot(dir, dir);
                if (lenSq > 0f && MathF.Abs(lenSq - 1f) > 1e-4f)
                {
                    float invLen = 1f / MathF.Sqrt(lenSq);
                    dir *= invLen;
                }
                return dir;
            }
        }

        /// <summary>
        /// Get Bone Position (if available).
        /// </summary>
        public Vector3 GetBonePos(Bones bone)
        {
            try
            {
                if (PlayerBones.TryGetValue(bone, out var boneTransform))
                {
                    var pos = boneTransform.Position;
                    if (pos != Vector3.Zero && !float.IsNaN(pos.X) && !float.IsInfinity(pos.X))
                        return pos;
                }
            }
            catch { }

            var rootPos = SkeletonRoot?.Position ?? Vector3.Zero;
            if (rootPos != Vector3.Zero && !float.IsNaN(rootPos.X) && !float.IsInfinity(rootPos.X))
                return rootPos;

            return Position;
        }

        private void ResetBoneTransform(Bones bone)
        {
            try
            {
                if (PlayerBones.TryGetValue(bone, out var boneTransform))
                {
                    DebugLogger.LogDebug($"Resetting transform for bone '{bone}' for Player '{Name ?? "Unknown"}'");
                    PlayerBones[bone] = new UnityTransform(boneTransform.TransformInternal);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.LogDebug($"Failed to reset bone '{bone}' transform for Player '{Name ?? "Unknown"}': {ex}");
            }
        }

        #endregion
    }

    /// <summary>
    /// Simple wrapper exposing skeleton root and bone transforms for aim helpers.
    /// </summary>
    public sealed class PlayerSkeleton
    {
        public PlayerSkeleton(UnityTransform root, ConcurrentDictionary<Bones, UnityTransform> bones)
        {
            Root = root;
            BoneTransforms = bones;
        }

        public UnityTransform Root { get; }
        public ConcurrentDictionary<Bones, UnityTransform> BoneTransforms { get; }
    }
}
