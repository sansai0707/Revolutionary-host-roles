using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using InnerNet;
using Rewired;
using UnityEngine;

using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    public static class CheckNameInRHR
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.MakePublic))]
        class MakePublicPatch
        {
            public static bool Prefix(GameStartManager __instance)
            {
                if (AmongUsClient.Instance.GetHost().PlayerName.ToUpper().Contains("RHR"))
                    return true;
                else
                {
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                        if (p.AmOwner)
                    p.SendChatPrivate(p, "入ってきた人が何部屋か分からなくなる場合があるため、名前にRHRを付けてください");
                    return false;
                }
            }
        }
    }
}