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
using TownOfHost;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    /*
    public static class RoleSelectPatch
    {

        public static CustomRpcSender sender = null;

        public static CustomRpcSender RoleSelect(CustomRpcSender send)
        {
            sender = send;
            RpcSetRole(RoleDatas.Tricker.TrickerPlayer, RoleTypes.Shapeshifter);
            return sender;
        }
        public static void RpcSetRole(List<PlayerControl> player, RoleTypes roleTypes)
        {
            foreach (PlayerControl p in player)
            {
                sender.RpcSetRole(p, roleTypes);
            }
        }
    }
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public class SelectRolesPatch
    {
        public static List<PlayerControl> SelectPlayers = new();
        public static List<PlayerControl> ImpostorPlayers = new();

        public static bool Prefix()
        {

            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (!player.Data.Disconnected)
                {
                    SelectPlayers.Add(player);
                }
            }
            CustomRpcSender sender = CustomRpcSender.Create("SelectRoles Sender", SendOption.Reliable);
            for (int i = 0; i < PlayerControl.GameOptions.NumImpostors; i++)
            {
                if (SelectPlayers.Count >= 1)
                {
                    var newimpostor = Helpers.SetRandom(SelectPlayers);
                    ImpostorPlayers.Add(newimpostor);
                    newimpostor.Data.Role.Role = RoleTypes.Impostor;
                    newimpostor.Data.Role.TeamType = RoleTeamTypes.Impostor;
                    SelectPlayers.RemoveAll(a => a.PlayerId == newimpostor.PlayerId);//もし2回同じ人をインポスター選出したらもう一回
                }

            }//インポスター
            //ロールアサイン
            RoleSelectPatch.RoleSelect(sender);
            return false;
        }

    }
*/
}
