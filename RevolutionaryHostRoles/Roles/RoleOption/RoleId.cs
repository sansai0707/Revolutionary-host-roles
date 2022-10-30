using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles
{
    public enum CustomRoleId
    {
        NormalRoles,
        Tricker,
        Bait,
        SecretlyKiller,
        UnderDog
    }
}