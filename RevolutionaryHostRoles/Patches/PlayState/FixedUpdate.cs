using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using RevolutionaryHostRoles.Roles;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagers
    {

        public static void Postfix()
        {
            SetNamePatch.SetRoleName();
            /*foreach (PlayerControl p in CachedPlayer.AllPlayers)
            if (p.IsImpostor())
                RoleDatas.Tricker.TrickerPlayer.Add(p);*/
        }
    }
}