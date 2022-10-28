using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using RevolutionaryHostRoles.Roles;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    public static class RoleSelectPatch
    {
        [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
        public class SelectRolesPatch
        {

            public static bool Prefix()
            {

                return true;
            }

        }
        public static RoleTypes SetRoleImpostor(PlayerControl p)
        {
            switch (p.GetRole())
            {
                case CustomRoleId.Tricker:
                    return RoleTypes.Shapeshifter;
                default: return RoleTypes.Impostor;
            }
        }
    }
}