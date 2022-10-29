using HarmonyLib;
using Hazel;
using InnerNet;
using Rewired;
using System;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace RevolutionaryHostRoles.Patches
{
    public static class SendChat
    {
        /// <summary>
        /// 特定の人にチャットを送る、使い方 : (送るPlayer(PlayerControl)).SendChatPrivate((送られるPlayer(PlayerControl)), (送るテキスト(string)), 送ったメッセージが見えるプレイヤー(送り主にしか見えない場合は書かなくて良い))
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="Text"></param>
        /// <param name="see"></param>
        public static void SendChatPrivate(this PlayerControl player, PlayerControl target, string Text, PlayerControl see = null)//特定の人にチャットを送る
        {
            //player = 送るプレイヤー
            //target = 送られるプレイヤー
            PlayerControl SeePlayer = see;//見えるプレイヤー
            if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SendChat, SendOption.None, SeePlayer.GetClientId());
            //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
            Writer.Write(Text);
            MessageExtensions.WriteNetObject(Writer, target);
            AmongUsClient.Instance.FinishRpcImmediately(Writer);
        }
        public static void RpcSetNames(this PlayerControl player, string Text, PlayerControl see = null)//特定の人の名前を一定時間変える
        {
            string PlayerName = player.Data.PlayerName;
            //player = 送るプレイヤー
            //target = 送られるプレイヤー
            PlayerControl SeePlayer = see;//見えるプレイヤー
            if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SetName, SendOption.None, SeePlayer.GetClientId());
            //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
            Writer.Write(Text);
            AmongUsClient.Instance.FinishRpcImmediately(Writer);

            new LateTask(() => 
            {
                PlayerControl SeePlayer = see;//見えるプレイヤー
                if (see == null) SeePlayer = player;//見えるプレイヤーいないなら自分だけ見える
                MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)RpcCalls.SetName, SendOption.None, SeePlayer.GetClientId());
                //SendOptionをReliableにすると部屋追放されるから変更しないでね！！
                Writer.Write(PlayerName);
                AmongUsClient.Instance.FinishRpcImmediately(Writer);
            }, 0.05f, "ResetName");
        }


        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
        public class CoSpawnPlayerPatch
        {
            public static void Postfix(PlayerPhysics __instance)
            {
                if (AmongUsClient.Instance.AmHost)
                {

                    __instance.myPlayer.SendChatPrivate(__instance.myPlayer, $"この部屋はMOD【Revolutionary Host Roles】\n通称【RHR】が実装されている部屋です。\n/mや/grなどの他のMODの\nコマンドは控えて下さい\n現在のホストは【{AmongUsClient.Instance.GetHost().PlayerName}】です。\n/cと送ることでコマンド一覧を確認出来ます。");
                }
            }
        }
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        class AddChatPatch
        {
            public static bool Prefix([HarmonyArgument(0)] PlayerControl SourcePlayer, [HarmonyArgument(1)] string chatText)
            {
                var Commands = chatText.Split(" ");
                //switchの書き方わかんねぇ！！()
                if (Commands[0].Equals("/c", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/C", StringComparison.OrdinalIgnoreCase))//送られたメッセージ
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, $"コマンド一覧\n/c : コマンド一覧を記載します。\n/h : ホストの名前を記載します\n/Roles : 現在入っている役職を記載します\n/RolesS : 現在入っている役職の設定を記載します\n/Settings : 現在のRHRの設定を記載します");
                    return false;
                }
                else if (Commands[0].Equals("/h", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/H", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, $"ホスト : {AmongUsClient.Instance.GetHost().PlayerName}");
                    return false;
                }
                else if (Commands[0].Equals("/Roles", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/roles", StringComparison.OrdinalIgnoreCase))
                {
                    if (SourcePlayer.AmOwner) SourcePlayer.RpcSetNames( $"入っている役職一覧\n{GameOptionsDataPatch.buildRoleOptions()}");
                    
                   SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                        return false;
                }
                else if (Commands[0].Equals("/RolesS", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/roleS", StringComparison.OrdinalIgnoreCase))
                {
                    if (SourcePlayer.AmOwner) SourcePlayer.RpcSetNames( $"入っている役職の設定一覧\n{GameOptionsDataPatch.buildRoleSettings()}");
                    SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                    return false;
                }
                else if (Commands[0].Equals("/Settings", StringComparison.OrdinalIgnoreCase) || Commands[0].Equals("/settings", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.RpcSetNames($"RHRの設定一覧\n{GameOptionsDataPatch.buildOptionsOfType(CustomOption.CustomOptionType.General, false)}");
                    if (SourcePlayer.AmOwner) SourcePlayer.SendChatPrivate(SourcePlayer, $"\n");
                    return false;
                }
                else if (Commands[0].ToUpper().Contains("/gr", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/n", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/ar", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/l", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/ar", StringComparison.OrdinalIgnoreCase) || Commands[0].ToUpper().Contains("/w", StringComparison.OrdinalIgnoreCase))
                {
                    SourcePlayer.SendChatPrivate(SourcePlayer, "他モッドのコマンドを送らないでください");
                    return false;
                }
                else
                {
                    return true;
                }


            }
        }
    }
}