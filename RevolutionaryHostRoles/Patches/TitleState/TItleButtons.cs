using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using Rewired.Utils;
using System.Linq;
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
        public static void Postfix(MainMenuManager __instance)
        {
            GameObject button = GameObject.Find("HowToPlayButton").transform.FindChild("Text_TMP").gameObject;
            GameObject.Destroy(button.GetComponent<TextTranslatorTMP>());
            TMPro.TextMeshPro text = button.GetComponent<TMPro.TextMeshPro>();
            text.text = "ぷれいほうほう";
            GameObject.Destroy(button.GetComponent<TextTranslatorTMP>());

            GameObject Disbutton = GameObject.Find("ExitGameButton").transform.FindChild("Text_TMP").gameObject;
            GameObject Dsisbutton = GameObject.Find("ExitGameButton").gameObject;
            GameObject.Destroy(Disbutton.GetComponent<TextTranslatorTMP>());
            TMPro.TextMeshPro texts = Disbutton.GetComponent<TMPro.TextMeshPro>();
            texts.text = "<color=#ff0000>破壊</color>";
        }
    }
}