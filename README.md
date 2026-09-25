# ArkBase

《杀戮尖塔 2》明日方舟系列模组的公共前置。ArkBase 不包含可露希尔角色，也不依赖任何角色扩展；其他模组可以把它作为基础，复用支援牌、状态效果与扩展接口。

## 当前内容

- 11 张支援牌，独立的浅蓝色牌框、支援牌图鉴分类及中英文文本。支援牌不会直接进入普通角色牌池，由扩展模组按需生成。
- 共用效果：迟钝、由迟钝触发的眩晕、沉默、麻痹、战栗，以及支援牌“余烬”需要的效果。
- 普通支援牌的精二映射：无声润物→氤氲、熔核巨影→余烬、破虏→秉烛照影、枚影觅迹→萃血。普通升级仍是常规升级；精二会保留牌当前的升级状态。
- 对应插图、状态图标、关键词解释和战斗机制。

## 安装

需要与当前游戏版本匹配的 `STS2-RitsuLib`（本项目清单要求 0.5.20）。发布包 `ArkBase-版本号.zip` 中包含 `ArkBase` 文件夹，将其放到游戏的 `mods` 目录；不要把文件散放在 `mods` 根目录。单独安装 ArkBase 不会添加可露希尔角色。

## 本地构建

需要 .NET 9 SDK、Godot 4.5.1 Mono、游戏本体及 RitsuLib 兼容包。将 `local.props.template` 复制为 `local.props` 并填写游戏、Godot 路径。`local.props` 不会提交到仓库。

```powershell
& .\build.ps1
```

脚本只使用本项目源码，编译、导出资源并生成 `dist\ArkBase-版本号.zip`，默认也会安装至游戏。使用 `-NoInstall` 只生成发布包；`-GameDir`、`-GodotExe` 可覆盖本机配置。先安装 ArkBase，再构建依赖它的扩展。

## 扩展接口

其他模组引用 `ArkBase.dll`，并在自己的模组 JSON 清单中声明 `ArkBase` 依赖。推荐入口：

| API | 用途 |
| --- | --- |
| `ArkBase.Api.StatusEffects` | 对目标施加迟钝、战栗、麻痹、沉默和眩晕。 |
| `ArkBase.Api.SupportCards.GetCards(rarity)` | 按稀有度获取公共支援牌。 |
| `ArkBase.Api.SupportCards.CanPromote` / `Promote` | 判断及执行普通支援牌精二，保留升级。 |
| `ArkBase.Api.ISluggishAppliedListener` | 响应迟钝施加。 |
| `ArkBase.Api.ISluggishDamageOverride` | 为特定来源忽略迟钝减伤。 |
| `ArkBase.Api.ISluggishThresholdModifier` | 修改迟钝进入眩晕的阈值。 |
| `ArkBase.Api.IStunPlayBypass` | 允许指定卡牌绕过己方眩晕出牌限制。 |
| `ArkBase.Keywords.ArkKeywords` | 使用公共关键词 ID。 |

具体支援牌和效果类型公开于 `ArkBase.Cards`、`ArkBase.Powers`。接口不反向引用任何角色模组；新增扩展不需要修改 ArkBase 的源码。

## 兼容性

本项目从旧版可露希尔模组拆出，共用卡牌、效果与关键词的 ID 改为 `ARK_BASE_*`。旧版运行中的存档不保证与拆分版兼容，建议开新局。仓库不包含可露希尔的专属卡牌、事件或遗物，也不包含已移除的“平局”彩蛋。
