using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using RevolutionaryHostRoles.Roles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace RevolutionaryHostRoles
{
    public static class Helpers
    {
        private delegate bool DelegateLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        private static DelegateLoadImage _callLoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _callLoadImage ??= IL2CPP.ResolveICall<DelegateLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>)data;

            return _callLoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }
        public static unsafe Texture2D loadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteTexture = new byte[stream.Length];
                var read = stream.Read(byteTexture, 0, (int)stream.Length);
                LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }
        public static Sprite LoadSpriteFromResources(string name, float size)
        {

            try
            {
                var texture = Helpers.loadTextureFromResources(name);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), size);
            }
            catch
            {

            }
            return null;
        }
        public static T SetRandom<T>(List<T> list)
        {
            var indexdate = UnityEngine.Random.Range(0, list.Count);
            return list[indexdate];
        }
        public static bool IsCrew(this PlayerControl p)
        {
            return !p.Data.Role.IsImpostor;
        }
        public static bool IsImpostor(this PlayerControl p)
        {
            return p.Data.Role.IsImpostor;
        }
        public static bool IsRole(this PlayerControl p, CustomRoleId role, bool IsChache = true)
        {
            CustomRoleId MyRole;
            MyRole = p.GetRole();
            return MyRole == role;
        }
        public static bool IsAlive(this PlayerControl p)
        {
            return !p.Data.IsDead;
        }
        public static bool IsDead(this PlayerControl p)
        {
            return p.Data.IsDead;
        }

        public static CustomRoleId GetRole(this PlayerControl p)
        {
            if (RoleDatas.Tricker.TrickerPlayer.IsCheckListPlayerControl(p)) return CustomRoleId.Tricker;
            else return CustomRoleId.NormalRoles;
        }
        public static bool IsCheckListPlayerControl(this List<PlayerControl> ListDate, PlayerControl CheckPlayer)
        {
            foreach (PlayerControl Player in ListDate)
            {
                if (Player.PlayerId == CheckPlayer.PlayerId)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static class NameHelper
    {
        public static Dictionary<int, string> PlayerNames;
        public static string PlayerName(this PlayerControl p)
        {
          
            var playerid = p.PlayerId;
            if (PlayerNames.ContainsKey(playerid))
            {
                return PlayerNames[playerid];
            }
            else
            {
                PlayerNames[playerid] = p.Data.PlayerName;
                return PlayerNames[playerid];
            }
        }
        public static string RoleName(this PlayerControl p)
        {
            if (RoleDatas.Tricker.TrickerPlayer.IsCheckListPlayerControl(p)) return "<color=#ff0000>トリッカー</color>";
            else
            {
                if (p.IsCrew())
                    return "<color=#00ffff>クルーメイト</color>";
                else if (p.IsImpostor())
                    return "<color=#ff0000>インポスター</color>";
                else return "例外きたあああああああああ";
            }
        }

        /*=====================ykundesuさんありがとうございます！！！=========================*/
        public static InnerNet.ClientData GetClient(this PlayerControl player)
        {
            var client = AmongUsClient.Instance.allClients.ToArray().Where(cd => cd.Character.PlayerId == player.PlayerId).FirstOrDefault();
            return client;
        }
        public static int GetClientId(this PlayerControl player)
        {
            var client = player.GetClient();
            return client == null ? -1 : client.Id;
        }
        public static void RpcSetNamePrivate(this PlayerControl TargetPlayer, string NewName, PlayerControl SeePlayer = null)
        {
            if (TargetPlayer == null || NewName == null || !AmongUsClient.Instance.AmHost) return;
            if (SeePlayer == null) SeePlayer = TargetPlayer;
            var clientId = SeePlayer.GetClientId();
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(TargetPlayer.NetId, (byte)RpcCalls.SetName, SendOption.Reliable, clientId);
            writer.Write(NewName);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        /*=====================ykundesuさんありがとうございます！！！=========================*/
    }
}