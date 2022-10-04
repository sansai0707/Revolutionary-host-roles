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
    static class AdditionalTempData
    {
        // Should be implemented using a proper GameOverReason in the future
        public static List<PlayerRoleInfo> playerRoles = new();
        public static GameOverReason gameOverReason;

        public static Dictionary<int, PlayerControl> plagueDoctorInfected = new();
        public static Dictionary<int, float> plagueDoctorProgress = new();

        public static void Clear()
        {
            foreach (var p in GameData.Instance.AllPlayers)
            PlayerRoleInfo.PlayerName = p.DefaultOutfit.PlayerName;
            playerRoles.Clear();
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            RoleDatas.Tricker.TrickerPlayer.Remove(p);
        }
        internal class PlayerRoleInfo
        {
            public static string PlayerName { get; set; }
            public static string NameSuffix { get; set; }
            public string RoleString { get; set; }
            public int TasksCompleted { get; set; }
            public int TasksTotal { get; set; }
            public int PlayerId { get; set; }
            public int ColorId { get; set; }
        }
    }
    public static class EndPatch
    {
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public class EndGameManagerSetUpPatch
        {
            public static bool IsHaison = false;
            public static TMPro.TMP_Text textRenderer;
            public static void Postfix(EndGameManager __instance)
            {
                AdditionalTempData.Clear();
            }


            [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
            public static class BeginEnd
            {

                public static void Postfix()
                {
                    AdditionalTempData.Clear();
                }
            }
        }
    }
}