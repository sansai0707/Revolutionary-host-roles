# coding: shift_jis
from tkinter import Text
from turtle import color


while True:
     RHR = "RevolutionaryHostRoles\\"

     #役職を入力
     RoleName = input("英語の役職名 : ")
     JapaneseRoleName = input("日本語の役職名 : ")
     Team = input("陣営(クルーorインポスター) : ")
     Color = input("色(red = 赤, blue= 青, yellow = 黄色など小文字and英語) : ")
     OptionId = input("オプションのId : ")
#ソースコード書き込み

     #RoleAssifnmentかきこみ
     with open(RHR + "Patches\\RoleAssignment.cs", "r", encoding="utf-8") as RAR:
          RA2 = RAR.read()

          if Team == "クルー":
             TeamText = "crew"
          elif Team == "インポスター":
               TeamText = "imp"
          else:
              print("陣営が「クルー」または「インポスター」ではありません。")
              exit()

          with open(RHR + "Patches\\RoleAssignment.cs", "w", encoding="utf-8") as RAW:
               text = RA2.replace(f"//{Team}", f"""{TeamText}Settings.Add((byte)CustomRoleId.{RoleName}, CustomOptionHolder.{RoleName}Option.GetSelection());\n            //{Team}""")
               RAW.write(text)
    #RoleId書き込み
     with open(RHR + "Roles\\RoleOption\\RoleId.cs", "r", encoding="utf-8") as RoleIdR:
          RoleIdR2 = RoleIdR.read()
          with open(RHR + "Roles\\RoleOption\\RoleId.cs", "w", encoding="utf-8") as RoleIdW:
               RoleIdtext = RoleIdR2.replace(f"//なんでも役職", f"""{RoleName},\n        //なんでも役職""")
               RoleIdW.write(RoleIdtext)
    #RoleDatas
     with open(RHR + "Roles\\RoleOption\\RoleDatas.cs", "r", encoding="utf-8") as RoleDatasR:
          RoleDatasR2 = RoleDatasR.read()
          with open(RHR + "Roles\\RoleOption\\RoleDatas.cs", "w", encoding="utf-8") as RoleDatasW:
               RoleDatastext = RoleDatasR2.replace(f"//RoleDatas", "        public static class {RoleName}\n         {\n            public static Color color = Color.{playercolor};\n            public static List<PlayerControl> {RoleName}Player;\n            public static void DataLoad()\n            {\n                {RoleName}Player = new();\n            }\n        }\n//RoleDatas").replace("{RoleName}", RoleName).replace("{playercolor}", Color)
               RoleDatasW.write(RoleDatastext)
     #CustomRPC          
     with open(RHR + "Patches\\CustomRPC.cs", "r", encoding="utf-8") as CustomRPCR:
          CustomRPCR2 = CustomRPCR.read()
          with open(RHR + "Patches\\CustomRPC.cs", "w", encoding="utf-8") as CustomRPCW:
               CustomRPCtext = CustomRPCR2.replace("//Roleせっと", f"case CustomRoleId.{RoleName}:\n                            RoleDatas.{RoleName}.{RoleName}Player.Add(player);\n                            break;\n                        //Roleせっと")
               CustomRPCW.write(CustomRPCtext)
    #Helper
     with open(RHR + "Helper.cs", "r", encoding="utf-8") as HelpersR:
          HelpersR2 = HelpersR.read()
          with open(RHR + "Helper.cs", "w", encoding="utf-8") as HelpersW:
               Helperstext = HelpersR2.replace(f"//MODの役職",f"else if (RoleDatas.{RoleName}.{RoleName}Player.IsCheckListPlayerControl(p)) return CustomRoleId.{RoleName};\n            //MODの役職")
               HelpersW.write(Helperstext)
     with open(RHR + "Helper.cs", "r", encoding="utf-8") as HelperR:
          HelperR2 = HelperR.read()
          with open(RHR + "Helper.cs", "w", encoding="utf-8") as HelperW:
               HelperNametext = HelperR2.replace("//RoleNameText", f"""case CustomRoleId.{RoleName}:\n                    return Helpers.cs(Color.{Color}, "{JapaneseRoleName} + ");\n                //RoleNameText""")
               HelperW.write(HelperNametext)
     #CustomOption
     with open(RHR + "Patches\\GameOptions\\CustomOptionHolder.cs", "r", encoding="utf-8") as CustomOptionHolderR:
          CustomOptionHolderR2 = CustomOptionHolderR.read()
          with open(RHR + "Patches\\GameOptions\\CustomOptionHolder.cs", "w", encoding="utf-8") as CustomOptionHolderW:
               CustomOptionHoldertext = CustomOptionHolderR2.replace(f"//CustomOptionHolder3です", f"public static CustomOption {RoleName}Option;\n        //CustomOptionHolder3です")
               CustomOptionHolderW.write(CustomOptionHoldertext)
     with open(RHR + "Patches\\GameOptions\\CustomOptionHolder.cs", "r", encoding="utf-8") as CustomOptionHoldersR:
          CustomOptionHoldersR2 = CustomOptionHoldersR.read()
          with open(RHR + "Patches\\GameOptions\\CustomOptionHolder.cs", "w", encoding="utf-8") as CustomOptionHoldersW:
               if Team == "クルー":
                  TeamTexts = "Crewmate"
                  TeamText2 = "Crew"
               elif Team == "インポスター":
                    TeamTexts = "Impostor"
                    TeamText2 = "Impo"
               CustomOptionHoldertext2 = CustomOptionHoldersR2.replace(f"//CustomOptionHolder2です", f"""{RoleName}Option = CustomOption.Create({TeamText2} + {OptionId}, Types.{TeamTexts}, cs(Color.{Color}, "{JapaneseRoleName}"), rates, null, true);\n            //CustomOptionHolder2です""")
               CustomOptionHoldersW.write(CustomOptionHoldertext2)
    #MakeText
     print("作成できたぞおおおおおお！！！")