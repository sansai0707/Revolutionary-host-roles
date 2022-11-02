# coding: shift_jis
while True:
     RHR = "RevolutionaryHostRoles\\"

     #役職を入力
     RoleName = input("役職名 : ")
     Team = input("陣営(クルーorインポスター) : ")
#ソースコード書き込み
     with open(RHR + "Patches\\RoleAssignment", "r", encoding="utf-8") as RAR:
          RA2 = RAR.read()
          if Team == "クルー":
             TeamText = "crew"
          elif Team == "インポスター":
               TeamText = "imp"
          else:
              exit()
          with open(RHR + "Patches\\RoleAssignment", "w", encoding="utf-8"):
               text = RA2.replace(f"//{Team}", f"{TeamText}Settings.Add((byte)CustomRoleId.{RoleName} CustomOptionHolder.{RoleName}Option.GetSelection());\n            //{Team}")