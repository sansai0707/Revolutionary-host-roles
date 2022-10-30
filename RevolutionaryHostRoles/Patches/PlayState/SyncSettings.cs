using HarmonyLib;
using Hazel;
using RevolutionaryHostRoles.Patches;
using RevolutionaryHostRoles;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using RevolutionaryHostRoles.Roles;

namespace RevolutionaryHostRoles
{
    //SuperNewRolesより！！！！！ありがとうございます！！！！
    public static class SyncSetting
    {
        public static GameOptionsData OptionData;
        public static void CustomSyncSettings(this PlayerControl player)
        {
            if (!AmongUsClient.Instance.AmHost) return;
            var role = player.GetRole();
            var optdata = OptionData.DeepCopy();
            switch (role)
            {
                case CustomRoleId.Tricker:
                    optdata.KillCooldown = RoleDatas.Tricker.IsChangeKillCool ? CustomOptionHolder.TrickerKillCool.GetFloat() * 2 : CustomOptionHolder.TrickerKillCool.GetFloat();
                    break;
                case CustomRoleId.SecretlyKiller:
                    optdata.KillCooldown = RoleDatas.SecretlyKiller.IsChangeKillCool ? CustomOptionHolder.SecretlyKillerKillCool.GetFloat() * 2 : CustomOptionHolder.SecretlyKillerKillCool.GetFloat();
                    break;
                case CustomRoleId.UnderDog:
                    optdata.KillCooldown = UnderDog.IsLastImpostor() ? CustomOptionHolder.UnderDogChangeKillCool.GetFloat() : CustomOptionHolder.UnderDogKillCool.GetFloat();
                    break;
            }
            if (player.IsDead()) optdata.AnonymousVotes = false;
            optdata.RoleOptions.ShapeshifterLeaveSkin = false;
            if (player.AmOwner) PlayerControl.GameOptions = optdata;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.SyncSettings, SendOption.None, player.GetClientId());
            writer.WriteBytesAndSize(optdata.ToBytes(5));
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        public static float KillCoolSet(float cool) { return cool <= 0 ? 0.001f : cool; }
        public static void CustomSyncSettings()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (!p.Data.Disconnected)
                {
                    CustomSyncSettings(p);
                }
            }
        }
        public static GameOptionsData DeepCopy(this GameOptionsData opt)
        {
            var optByte = opt.ToBytes(5);
            return GameOptionsData.FromBytes(optByte);
        }
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGame))]
        public class StartGame
        {
            public static void Postfix()
            {
                OptionData = PlayerControl.GameOptions.DeepCopy();
            }
        }
    }
}