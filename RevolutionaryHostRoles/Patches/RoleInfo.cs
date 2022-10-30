using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using RevolutionaryHostRoles.Patches;
using RevolutionaryHostRoles;
using RevolutionaryHostRoles.Roles;

namespace RevolutionaryHostRoles
{
    class RoleInfo
    {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public CustomRoleId roleId;
        public bool isNeutral;
        public bool isModifier;

        RoleInfo(string name, Color color, string introDescription, string shortDescription, CustomRoleId roleId, bool isNeutral = false, bool isModifier = false)
        {
            this.color = color;
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        public static RoleInfo tricker;
        public static void Load()
        {
           tricker  = new RoleInfo("Tricker", RoleDatas.Tricker.color, "Get voted out", "Get voted out", CustomRoleId.Tricker, true);
        }

        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            tricker
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true)
        {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked

            // Special roles
            if (p.IsRole(CustomRoleId.Tricker)) infos.Add(tricker);

            // Default roles (just impostor, just crewmate, or hunter / hunted for hide n seek
            if (infos.Count == count)
            {
                /*
                if (p.Data.Role.IsImpostor)
                    infos.Add(impostor);
                else
                    infos.Add(crewmate);
            */
                }

            return infos;
        }
    }
}