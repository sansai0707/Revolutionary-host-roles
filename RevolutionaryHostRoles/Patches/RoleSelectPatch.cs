using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using Hazel;
using MonoMod.Cil;
using RevolutionaryHostRoles.Roles;
using Rewired.Demos;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
   
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public class SelectRolesPatch
    {
        public static bool Prefix()
        {
            List<PlayerControl> AllPlayers = new();
            List<PlayerControl> ImpostorPlayer = new();
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (!player.Data.Disconnected)
                {
                    AllPlayers.Add(player);
                }
            }
            for (int i = 0; i < GameOptionsManager.Instance.CurrentGameOptions.NumImpostors; i++)
            {
                if (AllPlayers.Count != 0)
                {
                    var Impostor = Helpers.SetRandom(AllPlayers);
                    ImpostorPlayer.Add(Impostor);
                    AllPlayers.RemoveAll(a => a.PlayerId == Impostor.PlayerId);
                }
            }
            foreach (PlayerControl Impostor in ImpostorPlayer)
            {
                Impostor.RpcSetRole(RoleTypes.Impostor);
            }
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (!player.Data.Disconnected)
                {
                    if (!player.IsImpostor())
                    {
                        player.RpcSetRole(RoleTypes.Crewmate);
                    }
                }
            }
            return false;
        }
    }
}
