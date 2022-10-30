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
    public static class EndPatch
    {
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public class EndGameManagerSetUpPatch
        {
            public static bool IsHaison = false;
            public static TMPro.TMP_Text textRenderer;
            public static void Postfix(EndGameManager __instance)
            {
                

            }


            [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
            public static class BeginEnd
            {

                public static void Postfix()
                {

                }
            }
        }
    }
}