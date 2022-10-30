using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Il2CppSystem.Media;
using MonoMod.Cil;
using RevolutionaryHostRoles;
using Rewired.UI.ControlMapper;
using UnityEngine;
using static HarmonyLib.InlineSignature;
using static RevolutionaryHostRoles.Roles.RoleDatas;
using static Rewired.Utils.Classes.Utility.ObjectInstanceTracker;
using UnityEngine.UIElements;
using RevolutionaryHostRoles.Patches;

namespace RevolutionaryHostRoles
{
    enum CustomRPC
    {
        // Main Controls

        ResetVariables = 60,
        ShareOptions,
        SetRole,
        OverrideNativeRole,
        workaroundSetRoles
    }

    public static class RPCProcedure
    {

        // Main Controls
        public static void resetVariables()
        {

        }
        public static void ShareOptions(int numberOfOptions, MessageReader reader)
        {
            try
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.FirstOrDefault(option => option.id == (int)optionId);
                    option.updateSelection((int)selection);
                }
            }
            catch (Exception e)
            {
               　RevolutionaryHostRolesPlugin.Logger.LogError("Error while deserializing options: " + e.Message);
            }
        }

        public static void setRole(byte roleId, byte playerId)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == playerId)
                {
                    switch ((CustomRoleId)roleId)
                    {
                        case CustomRoleId.Tricker:
                            Tricker.TrickerPlayer.Add(player);
                            break;
                        case CustomRoleId.Bait:
                            Bait.BaitPlayer.Add(player);
                            break;
                        case CustomRoleId.SecretlyKiller:
                            SecretlyKiller.SecretlyKillerPlayer.Add(player);
                            break;
                    }
                }
        }

        public static void overrideNativeRole(byte playerId, byte roleType)
        {
            var player = Helpers.playerById(playerId);
            player.roleAssigned = false;
            DestroyableSingleton<RoleManager>.Instance.SetRole(player, (RoleTypes)roleType);
        }

        public static void workaroundSetRoles(byte numberOfRoles, MessageReader reader)
        {
            for (int i = 0; i < numberOfRoles; i++)
            {
                byte playerId = (byte)reader.ReadPackedUInt32();
                byte roleId = (byte)reader.ReadPackedUInt32();
                try
                {
                    setRole(roleId, playerId);
                }
                catch (Exception e)
                {
                    RevolutionaryHostRolesPlugin.Logger.LogError("Error while deserializing roles: " + e.Message);
                }
            }

        }
        public static void cleanBody(byte playerId)
        {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++)
            {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId)
                {
                    UnityEngine.Object.Destroy(array[i].gameObject);
                }
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.HandleRpc))]
        class CustomNetworkTransformRPCHandlerPatch
        {
            public static bool Prefix(CustomNetworkTransform __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                var rpcType = (RpcCalls)callId;
                MessageReader subReader = MessageReader.Get(reader);
                switch (rpcType)
                {
                    case RpcCalls.SnapTo:
                        Vector2 position = __instance.ReadVector2(subReader);
                        ushort minSid = subReader.ReadUInt16();
                        break;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        class RPCHandlerPatch
        {

            static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                byte packetId = callId;
                try
                {
                    switch (packetId)
                    {

                        // Main Controls

                        case (byte)CustomRPC.ResetVariables:
                            RPCProcedure.resetVariables();
                            break;
                        case (byte)CustomRPC.ShareOptions:
                            RPCProcedure.ShareOptions((int)reader.ReadPackedUInt32(), reader);
                            break;
                        case (byte)CustomRPC.SetRole:
                            byte roleId = reader.ReadByte();
                            byte playerId = reader.ReadByte();
                            RPCProcedure.setRole(roleId, playerId);
                            break;
                        case (byte)CustomRPC.OverrideNativeRole:
                            RPCProcedure.overrideNativeRole(reader.ReadByte(), reader.ReadByte());
                            break;
                        case (byte)CustomRPC.workaroundSetRoles:
                            RPCProcedure.workaroundSetRoles(reader.ReadByte(), reader);
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
