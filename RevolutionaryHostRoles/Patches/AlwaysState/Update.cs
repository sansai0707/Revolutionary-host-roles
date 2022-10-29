using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RevolutionaryHostRoles.Patches.AlwaysState
{
    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    class LateUpdate
    {
        public static void Postfix()
        {
            LateTask.Update(Time.deltaTime);
        }
    }
}
