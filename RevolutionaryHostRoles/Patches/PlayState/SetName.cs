using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    public static class SetNamePatch
    {

        public static void SetRoleName()
        {
            string AddName = "";
            string TaskText = "";
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (AmongUsClient.Instance.GameState == AmongUsClient.GameStates.Started)
                {
                    /*
                    if ()
                    {*/

                        p.RpcSetNamePrivate("<size=75%>" + p.RoleName() + "</size>\n" + p.PlayerName());
                    /*}
                    */
                }
            }
        }
    }
}