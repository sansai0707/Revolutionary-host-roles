using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace RevolutionaryHostRoles.Patches
{
    /* [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    */
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class TitleButtons
    {
        public static void Postfix()
        {
            GameObject button = GameObject.Find("HowToPlayButton").transform.FindChild("Text_TMP").gameObject;
            GameObject.Destroy(button.GetComponent<TextTranslatorTMP>());
            TMPro.TextMeshPro text = button.GetComponent<TMPro.TextMeshPro>();
            text.text = "遊ぶ方";
            GameObject.Destroy(button.GetComponent<TextTranslatorTMP>());
        }
        [HarmonyPatch(typeof(HowToPlayController), nameof(HowToPlayController.Start))]
        public static class HowToButton
        {
            public static bool Prefix()
            {
                return true;
            }
        }
    }
}