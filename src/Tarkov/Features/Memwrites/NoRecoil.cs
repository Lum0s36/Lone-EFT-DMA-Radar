using LoneEftDmaRadar.DMA;
using LoneEftDmaRadar.Tarkov.GameWorld.Player;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LoneEftDmaRadar.Tarkov.Features.MemWrites
{
    /// <summary>
    /// Removes weapon recoil and sway by modifying effector intensities.
    /// </summary>
    public sealed class NoRecoil : MemWriteFeature<NoRecoil>
    {
        private bool _lastEnabledState;
        private ulong _cachedBreathEffector;
        private ulong _cachedShotEffector;
        private ulong _cachedNewShotRecoil;

        // Used to avoid unnecessary writes every tick
        private float _lastRecoilAmount = MemWriteConstants.DefaultIntensity;
        private float _lastSwayAmount   = MemWriteConstants.DefaultIntensity;

        // Optional: track "session" so we can invalidate cache when weapon changes
        private ulong _lastPwaPtr;

        [Flags]
        public enum EProceduralAnimationMask
        {
            Breathing      = 1,
            Walking        = 2,
            MotionReaction = 4,
            ForceReaction  = 8,
            Shooting       = 16,
            DrawDown       = 32,
            Aiming         = 64,
            HandShake      = 128,
        }

        private const int ORIGINAL_PWA_MASK =
            (int)(EProceduralAnimationMask.MotionReaction |
                  EProceduralAnimationMask.ForceReaction |
                  EProceduralAnimationMask.Shooting |
                  EProceduralAnimationMask.DrawDown |
                  EProceduralAnimationMask.Aiming |
                  EProceduralAnimationMask.Breathing);

        public override bool Enabled
        {
            get => App.Config.MemWrites.NoRecoilEnabled;
            set => App.Config.MemWrites.NoRecoilEnabled = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(MemWriteConstants.NoRecoilDelayMs);

        public override void TryApply(LocalPlayer localPlayer)
        {
            //DebugLogger.LogDebug($"[NoRecoil] TryApply called - Enabled: {Enabled}");

            try
            {
                if (localPlayer == null)
                {
                    //DebugLogger.LogDebug("[NoRecoil] LocalPlayer is null");
                    return;
                }

                var stateChanged = Enabled != _lastEnabledState;
                //DebugLogger.LogDebug($"[NoRecoil] State changed: {stateChanged}, LastState: {_lastEnabledState}, CurrentState: {Enabled}");

                if (!Enabled)
                {
                    if (stateChanged)
                    {
                        //DebugLogger.LogDebug("[NoRecoil] Disabling - calling ResetNoRecoil");
                        ResetNoRecoil(localPlayer);
                        _lastEnabledState = false;
                        //DebugLogger.LogDebug("[NoRecoil] Disabled");
                    }
                    return;
                }

                //DebugLogger.LogDebug($"[NoRecoil] LocalPlayer.PWA: 0x{localPlayer.PWA:X}");

                // Early-out if PWA is not valid (not in raid / no weapon)
                if (!MemDMA.IsValidVirtualAddress(localPlayer.PWA))
                {
                    //DebugLogger.LogDebug("[NoRecoil] PWA is invalid - no weapon equipped?");
                    if (stateChanged)
                        //DebugLogger.LogDebug("[NoRecoil] Enabled but PWA invalid ¨C waiting for raid/weapon");
                    ClearCache();
                    _lastEnabledState = Enabled;
                    return;
                }

                //DebugLogger.LogDebug("[NoRecoil] PWA is valid, calling ApplyNoRecoil");
                ApplyNoRecoil(localPlayer);

                if (stateChanged)
                {
                    _lastEnabledState = true;
                    //DebugLogger.LogDebug("[NoRecoil] Enabled (state changed)");
                }
            }
            catch
            {
                //DebugLogger.LogDebug($"[NoRecoil] Error in TryApply: {ex}");
                //DebugLogger.LogDebug($"[NoRecoil] Stack trace: {ex.StackTrace}");
                ClearCache();
            }
        }

private void ApplyNoRecoil(LocalPlayer localPlayer)
{
    try
    {
        // Convert UI percentages (0 = normal, 100 = fully removed) into intensity scalars.
        // 1.0 = default behaviour, 0 = fully removed.
        float recoilAmount = Math.Clamp(1f - (App.Config.MemWrites.NoRecoilAmount / 100f), 0f, 1f);
        float swayAmount   = Math.Clamp(1f - (App.Config.MemWrites.NoSwayAmount   / 100f), 0f, 1f);

        var (breathEffector, shotEffector, newShotRecoil) = GetEffectorPointers(localPlayer);

        if (!ValidatePointers(breathEffector, shotEffector, newShotRecoil))
        {
            ClearCache();
            return;
        }

        // Read CURRENT values from game memory
        float currentBreath = Memory.ReadValue<float>(
            breathEffector + Offsets.BreathEffector.Intensity, false);

        if (currentBreath < MemWriteConstants.MinBreathIntensity || 
            currentBreath > MemWriteConstants.MaxBreathIntensity)
        {
            ClearCache();
            return;
        }

        Vector3 currentRecoil = Memory.ReadValue<Vector3>(
            newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, false);

        int currentMask = Memory.ReadValue<int>(
            localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask, false);

        // Compare CURRENT vs TARGET
        if (Math.Abs(currentBreath - swayAmount) > MemWriteConstants.FloatComparisonTolerance)
        {
            Memory.WriteValue(
                breathEffector + Offsets.BreathEffector.Intensity,
                swayAmount);
        }

        // Compare CURRENT recoil vs TARGET
        var recoilVec = new Vector3(recoilAmount, recoilAmount, recoilAmount);
        if (Vector3.Distance(currentRecoil, recoilVec) > MemWriteConstants.FloatComparisonTolerance)
        {
            Memory.WriteValue(
                newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors,
                recoilVec);
        }

        // Mask management
        int targetMask;
        if (recoilAmount <= MemWriteConstants.MinimalMaskThreshold && 
            swayAmount <= MemWriteConstants.MinimalMaskThreshold)
        {
            targetMask = (int)EProceduralAnimationMask.Shooting;
        }
        else
        {
            targetMask = ORIGINAL_PWA_MASK;
        }

        if (currentMask != targetMask)
        {
            Memory.WriteValue(
                localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask,
                targetMask);
        }

        _lastRecoilAmount = recoilAmount;
        _lastSwayAmount   = swayAmount;
    }
    catch
    {
        throw;
    }
}

        private void ResetNoRecoil(LocalPlayer localPlayer)
        {
            try
            {
                var (breathEffector, shotEffector, newShotRecoil) = GetEffectorPointers(localPlayer);

                if (ValidatePointers(breathEffector, shotEffector, newShotRecoil))
                {
                    Memory.WriteValue(
                        breathEffector + Offsets.BreathEffector.Intensity,
                        MemWriteConstants.DefaultIntensity);

                    Memory.WriteValue(
                        newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors,
                        new Vector3(MemWriteConstants.DefaultIntensity, MemWriteConstants.DefaultIntensity, MemWriteConstants.DefaultIntensity));

                    Memory.WriteValue(
                        localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask,
                        ORIGINAL_PWA_MASK);
                }

                _lastRecoilAmount = MemWriteConstants.DefaultIntensity;
                _lastSwayAmount   = MemWriteConstants.DefaultIntensity;
                ClearCache();
            }
            catch
            {
            }
        }

        private (ulong breathEffector, ulong shotEffector, ulong newShotRecoil) GetEffectorPointers(LocalPlayer localPlayer)
        {
            var pwa = localPlayer.PWA;

            //DebugLogger.LogDebug($"[NoRecoil] GetEffectorPointers - PWA: 0x{pwa:X}, LastPWA: 0x{_lastPwaPtr:X}");

            // If PWA changed (weapon swap, death, etc.), drop cache
            if (pwa != _lastPwaPtr)
            {
                //DebugLogger.LogDebug("[NoRecoil] PWA changed, clearing cache");
                ClearCache();
                _lastPwaPtr = pwa;
            }

            // Return cached if valid
            if (MemDMA.IsValidVirtualAddress(_cachedBreathEffector) &&
                MemDMA.IsValidVirtualAddress(_cachedShotEffector) &&
                MemDMA.IsValidVirtualAddress(_cachedNewShotRecoil))
            {
                //DebugLogger.LogDebug("[NoRecoil] Using cached pointers");
                return (_cachedBreathEffector, _cachedShotEffector, _cachedNewShotRecoil);
            }

            //DebugLogger.LogDebug("[NoRecoil] Cache invalid, reading from memory...");

            if (!MemDMA.IsValidVirtualAddress(pwa))
            {
                //DebugLogger.LogDebug("[NoRecoil] PWA invalid, returning zeros");
                return (0, 0, 0);
            }

            var breathEffector = Memory.ReadPtr(pwa + Offsets.ProceduralWeaponAnimation.Breath, false);
            var shotEffector   = Memory.ReadPtr(pwa + Offsets.ProceduralWeaponAnimation.Shootingg, false);

            //DebugLogger.LogDebug($"[NoRecoil] Read - BreathEffector: 0x{breathEffector:X}, ShotEffector: 0x{shotEffector:X}");

            if (!MemDMA.IsValidVirtualAddress(breathEffector) ||
                !MemDMA.IsValidVirtualAddress(shotEffector))
            {
                //DebugLogger.LogDebug("[NoRecoil] Invalid breathEffector or shotEffector");
                return (0, 0, 0);
            }

            var newShotRecoil = Memory.ReadPtr(
                shotEffector + Offsets.ShotEffector.NewShotRecoil, false);

            //DebugLogger.LogDebug($"[NoRecoil] Read - NewShotRecoil: 0x{newShotRecoil:X}");

            if (!MemDMA.IsValidVirtualAddress(newShotRecoil))
            {
                //DebugLogger.LogDebug("[NoRecoil] Invalid newShotRecoil");
                return (0, 0, 0);
            }

            // Cache the pointers
            _cachedBreathEffector = breathEffector;
            _cachedShotEffector   = shotEffector;
            _cachedNewShotRecoil  = newShotRecoil;

            //DebugLogger.LogDebug("[NoRecoil] Cached new pointers");

            return (breathEffector, shotEffector, newShotRecoil);
        }

        private static bool ValidatePointers(ulong breathEffector, ulong shotEffector, ulong newShotRecoil)
        {
            return MemDMA.IsValidVirtualAddress(breathEffector) &&
                   MemDMA.IsValidVirtualAddress(shotEffector)   &&
                   MemDMA.IsValidVirtualAddress(newShotRecoil);
        }

        private void ClearCache()
        {
            //DebugLogger.LogDebug("[NoRecoil] Clearing cache");
            _cachedBreathEffector = 0;
            _cachedShotEffector   = 0;
            _cachedNewShotRecoil  = 0;
        }

        public override void OnRaidStart()
        {
            //DebugLogger.LogDebug("[NoRecoil] OnRaidStart called");
            _lastEnabledState  = default;
            _lastRecoilAmount  = MemWriteConstants.DefaultIntensity;
            _lastSwayAmount    = MemWriteConstants.DefaultIntensity;
            _lastPwaPtr        = 0;
            ClearCache();
        }
    }
}
