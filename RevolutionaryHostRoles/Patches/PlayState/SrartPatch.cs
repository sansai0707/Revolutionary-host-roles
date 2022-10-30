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
    public static class StartPatch
    {

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.CoShowIntro))]
        public static class ShowIntro
        {
            public static bool Prefix()
            {
                RoleDatas.DataLoads();
                NameHelper.PlayerNames = new();
                ReportDeadBodyPatch.DiePlayers = new();
                return true;
            }
            public static void Postfix()
            {

            }

        }
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.StartGame))]
        public static class BeginStart
        {
            public static bool IsStart;
            public static void Postfix()
            {
                IsStart = true;

            }
        }
    }
}
