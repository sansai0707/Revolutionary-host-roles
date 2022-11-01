→ [日本語](README.md)<br>
[English](README-English.md)<br>

# Revolutionary-host-roles
![RHRlogo](/images/RHRLogoIcon.png)

## MODについて
これはInnerslothが作ったものじゃなくて個人が作ったものなのでInnerslothは関係ないです。あとこの中にある素材の一部はInnerslothのものです。<br>
また、このMODについてInnerslothに問い合わせるのはお控えください。

## リンク
[discord](https://discord.gg/KC3G57CWeU)

## クレジット
- [TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles) このMODが無ければRHRは作れていません(LoadSpriteFromResourcesやCustomOption、ToByte、RoleAssignment、CustomRpcやそれ以上にソースコードをお借りしています)！！そして、マフィアのアイデア元になっています！！！TheOtherRolesAU](https://github.com/TheOtherRolesAU)さんや、その他開発者さんありがとうございます！！！！

- [SuperNewRoles](https://github.com/ykundesu/SuperNewRoles) こちらも[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)同様、沢山のソースコードを参考、お借りしています！！！(RpcSetNamePrivate、SyncSetting)、そして、シークレットリーキラーのアイデア元です！！！、[ykundesu](https://github.com/ykundesu)さんや、その他SNR開発者さんありがとうございます！！！

- [TownOfHost](https://github.com/tukasa0001/TownOfHost) LateTaskやCustomRpcSenderをお借りしました！！！[tukasa0001](https://github.com/tukasa0001)さんや、TOHの開発者さんさんありがとうございます！！！

- [TownOfUs](https://github.com/slushiegoose/Town-Of-Us) アンダードッグのアイデア元です！[slushiegoose](https://github.com/slushiegoose)さんや、その他の開発者さんありがとうございます！！
## 開発者
- [山菜](https://github.com/sansai0707)([Twitter](https://twitter.com/sansai_yukkuri))/([YouTube](https://youtube.com/channel/UCj1SxnfqEKlnwXkhCG_VZ7w))
- [れもんず](https://github.com/remons123)([Twitter](https://twitter.com/abcremons))
- [ハロン](https://github.com/Haroweeeeen)(Twitter)/(YouTube)
- [シャンパン](https://github.com/Shanpan2)([Twitter](https://twitter.com/shanpanus?s=21&t=VkDFSOnM3bkZQ7Rdw1vNHA))/(YouTube)
## 開発協力者

## 内容
### コマンド
|キー  |コマンドの内容                            |
-------|------------------------------------------|
| /c   |コマンド一覧を確認できます        　　　  |
| /h   |現在のホストを確認出来ます        　　　  |
| /ar  |全ての役職の選出する確率を記載します      |
| /ar c|全てのクルー役職の選出する確率を記載します|
| /ar i|全てのインポ役職の選出する確率を記載します|
| /ar n|全ての第三陣営役職の選出確率を記載します  |
| /re  |全ての役職の設定を記載します  　　　　    |
| /re c|全てのクルーの役職の設定を記載します  　　      |
| /re i|全てのインポスターの役職の設定を記載します  |
| /re n|全ての第三陣営の役職の設定を記載します    |
| /se  |全てのRHRの設定を記載します               |
### 役職
|[クルーメイト役職](#クルーメイト役職)  |      [インポスター役職](#インポスター役職)       |[第三陣営役職](#第三陣営役職)|
-------------------|-----------------------------|------------|
| [ベイト](#ベイト)           |[トリッカー](#トリッカー)                   |            |
|                  |[シークレットリーキラー](#シークレットリーキラー)       |    　　　  |
|                  |[アンダードッグ](#アンダードッグ)             |    　　　  |
|                  |[マフィア](#マフィア)　　　　　　　　　　　       |    　　　  |
|                  |　　　　　　　　　　　       |    　　　  |
|                  |　　　　　　　　　　　       |    　　　  |
|                  |　　　　　　　　　　　       |    　　　  |
|                  |　　　　　　　　　　　       |    　　　  |
|                  |　　　　　　　　　　　       |    　　　  |

## クルーメイト役職<br>
## ベイト<br>
陣営 : クルーメイト
ベイトがキルされてからn秒後にベイトをキルした人にベイト自身の死体を通報させます。
置き換え役職 : クルー
### 設定項目
キルされてから通報されるまでの時間
## インポスター役職<br>
## トリッカー<br>
陣営 : インポスター
シェイプした後のキルでは死体が出ません。
置き換え役職 : シェイプシフター
### 設定項目
キルクール
## シークレットリーキラー<br>
陣営 : インポスター
キルをしてもキルワープが出ません。
置き換え役職 : インポスター
[SuperNewRoles](https://github.com/ykundesu/SuperNewRoles)からの引用役職です。
### 設定項目
キルクール
## アンダードッグ<br>
陣営 : インポスター
最後のインポスターになった時にキルクールがn秒になります
置き換え役職 : インポスター
[TownOfUs](https://github.com/slushiegoose/Town-Of-Us)からの引用役職です。
### 設定項目
通常時のキルクール<br>
最後のインポスターになった時のキルクール
## マフィア<br>
陣営 : インポスター<br>
最後のインポスターになった時にキルが出来るようになります。<br>
逆に最後のインポスターではない場合キルは出来ません。<br
置き換え役職 : インポスター<br>
[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)からの引用役職です。
### 設定項目
なし
## 第三陣営役職
