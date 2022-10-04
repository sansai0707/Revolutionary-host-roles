using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

using static Rewired.Utils.Classes.Utility.ObjectInstanceTracker;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace RevolutionaryHostRoles.Roles
{
    class RoleInfo
    {
        public Color color;
        public string RoleName;
        public string introDescription;
        public string shortDescription;
        public CustomRoleId roleId;
        public bool isNeutral;
        public bool isModifier;

        RoleInfo(string RoleName, Color color, CustomRoleId roleId)
        {
            this.color = color;
            this.RoleName = RoleName;
            this.roleId = roleId;
        }

        public static RoleInfo Tricker = new RoleInfo("トリッカー", Palette.ImpostorRed,　CustomRoleId.Tricker);
    }
}