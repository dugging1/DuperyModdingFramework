using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using DMF_Lib;
using DuperyModdingFramework.Internal;
using GRGLib.Extensions.Collections;
using GRGLib.Maybe;
using HarmonyLib;
using Tomlyn.Model;
using UnityEngine;

namespace DuperyModdingFramework;

// Extend Keywords Enum w/ new registered roles

class Patches
{
    //Return false skips original code.
    static bool isModdedRole<T>(ref T _result, Roles r, Func<ID, T> OnModded)
    {
        if ((int)r >= Enum.GetValues(typeof(Roles)).Length && (int)r < 10000)
        {
            ID id = RegistryRepo.RoleEnumValue.ReverseLookup((int)r);
            _result = OnModded(id);
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(CaseHelper), "createEmptyRole")]
    [HarmonyPrefix]
    static bool createEmptyRole(ref RoleData __result, Roles _role)
     => isModdedRole(ref __result, _role,
     id => RegistryRepo.RoleData.Lookup(id).RoleFactory.CreateEmpty());

    [HarmonyPatch(typeof(CaseHelper), "getClassificationFromRoleType")]
    [HarmonyPrefix]
    static bool getClassificationFromRoleType(ref RoleClassifications __result, Roles _role)
        => isModdedRole(ref __result, _role,
        id => RegistryRepo.RoleData.Lookup(id).StartingClassification);

#pragma warning disable Harmony003 // Harmony non-ref patch parameters modified
    [HarmonyPatch(typeof(CaseHelper), "createRole")]
    [HarmonyPrefix]
    static bool createRole(ref RoleData __result, Roles _role, int _address, Vector2Int _board_pos, System.Random _rand, Maybe<RoleData> _true_self)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleData.Lookup(id).RoleFactory.CreateSetRole(_address, (_board_pos.x, _board_pos.y), _rand, _true_self));
#pragma warning restore Harmony003 // Harmony non-ref patch parameters modified

    [HarmonyPatch(typeof(RoleHelper), "isClockRole")]
    [HarmonyPrefix]
    static bool isClockRole(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.CLOCK)
                || RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.NONE)
            );

    [HarmonyPatch(typeof(RoleHelper), "isDocksRole")]
    [HarmonyPrefix]
    static bool isDocksRole(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.DOCKS)
                || RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.NONE)
        );

    [HarmonyPatch(typeof(RoleHelper), "isCasinoRole")]
    [HarmonyPrefix]
    static bool isCasinoRole(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.CASINO)
                || RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.NONE)
        );

    [HarmonyPatch(typeof(RoleHelper), "isGenericRole")]
    [HarmonyPrefix]
    static bool isGenericRole(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleRegions.Lookup(id).Contains(Regions.NONE));

    [HarmonyPatch(typeof(RoleHelper), "isInnocent")]
    [HarmonyPrefix]
    static bool isInnocent(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleData.Lookup(id).StartingClassification == RoleClassifications.INNOCENT);

    [HarmonyPatch(typeof(RoleHelper), "isMeddler")]
    [HarmonyPrefix]
    static bool isMeddler(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleData.Lookup(id).StartingClassification == RoleClassifications.MEDDLER);

    [HarmonyPatch(typeof(RoleHelper), "isUnderling")]
    [HarmonyPrefix]
    static bool isUnderling(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleData.Lookup(id).StartingClassification == RoleClassifications.UNDERLING);

    [HarmonyPatch(typeof(RoleHelper), "isTraitor")]
    [HarmonyPrefix]
    static bool isTraitor(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => RegistryRepo.RoleData.Lookup(id).StartingClassification == RoleClassifications.TRAITOR);

    [HarmonyPatch(typeof(RoleHelper), nameof(RoleHelper.isTest))]
    [HarmonyPrefix]
    static bool isTest(ref bool __result, Roles _role)
        => isModdedRole(ref __result, _role,
            id => false);

    // [HarmonyPatch(typeof(RoleVisualDataSO), "Item", MethodType.Getter, [typeof(Roles)])]
    // [HarmonyPrefix]
    // static bool get_Item(ref Sprite __result, Roles _role)
    // {
    //     if ((int)_role >= Enum.GetValues(typeof(Roles)).Length && (int)_role < 10000)
    //     {
    //         ID id = RegistryRepo.RoleEnumValue.ReverseLookup((int)_role);
    //         if (RegistryRepo.RoleData.Lookup(id).SpriteID.is_some)
    //         {
    //             __result = RegistryRepo.Sprites.Lookup(RegistryRepo.RoleData.Lookup(id).SpriteID.value);
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    [HarmonyPatch(typeof(RoleHelper), nameof(RoleHelper.getRoleList), [typeof(Func<Roles, bool>), typeof(bool)])]
    [HarmonyPrefix]
    static bool getRoleList(ref List<Roles> __result, Func<Roles, bool> _predicate, bool _allow_test)
    {
        DuperyModdingFramework.Logger.LogInfo("Making role list.");
        List<Roles> ret = new();
        ret.addAll<Roles>();
        ret.AddRange(RegistryRepo.RoleEnumValue.KeyValue.Select(
            kv =>
            {
                DuperyModdingFramework.Logger.LogInfo($"Adding role with ID: {kv.Key}");
                return (Roles)kv.Value;
            }
        ));
        DuperyModdingFramework.Logger.LogInfo($"Total Role List has length: {ret.Count}");
        ret = ret.Where(_predicate).toListSafe();
        if (!_allow_test)
        {
            CollectionsExtension.removeAll(ret, RoleHelper.isTest);
        }
        DuperyModdingFramework.Logger.LogInfo($"Reduced role list has length: {ret.Count}");
        return false;
    }


    [HarmonyPatch(typeof(GameState), nameof(GameState.setRegion))]
    [HarmonyPrefix]
    static bool setRegion(RegionDataSO _region_settings)
    {
        List<Roles> availableRoles = getRegionDaataSO_availableRoles(_region_settings);
        foreach (var kv in RegistryRepo.RoleRegions.KeyValue)
        {
            if (kv.Value.Contains(_region_settings.region) || kv.Value.Contains(Regions.NONE))
            {
                availableRoles.Add((Roles)RegistryRepo.RoleEnumValue.Lookup(kv.Key));
                if (RegistryRepo.RoleStartingRegions.Lookup(kv.Key).Contains(_region_settings.region)
                    || RegistryRepo.RoleStartingRegions.Lookup(kv.Key).Contains(Regions.NONE))
                {
                    _region_settings.starting_available_roles.Add((Roles)RegistryRepo.RoleEnumValue.Lookup(kv.Key));
                }
            }
        }
        return true;
    }

    static List<Roles> getRegionDaataSO_availableRoles(RegionDataSO rd)
    {
        return (List<Roles>)typeof(RegionDataSO).GetField("allowed_roles",
            System.Reflection.BindingFlags.NonPublic
            | System.Reflection.BindingFlags.Instance).GetValue(rd);
    }

}
