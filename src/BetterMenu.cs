using System.Collections.Generic;
using System.Linq;

using Modding;

using Satchel.BetterMenus;

namespace EnemyHPBar {
	//Thank CutomKnight menu:https://github.com/PrashantMohta/HollowKnight.CustomKnight/blob/moreskin/CustomKnight/Menu/BetterMenu.cs
	internal static class BetterMenu {
		internal static int selectedSkin = 0;
		internal static Menu MenuRef = null;
		internal static MenuScreen GetMenu(MenuScreen lastMenu, ModToggleDelegates? toggleDelegates) {
			if (MenuRef == null) {

				MenuRef = PrepareMenu();
			}
			MenuRef.OnBuilt += (_, Element) => {


				if (EnemyHPBar.instance.CurrentSkin != null) {
					BetterMenu.SelectedSkin(EnemyHPBar.instance.CurrentSkin.GetId());
				}
			};
			return MenuRef.GetMenuScreen(lastMenu);
		}
		internal static void ApplySkin() {
			ISelectableSkin skinToApply = EnemyHPBar.SkinList[selectedSkin];
			BetterMenu.SetSkinById(skinToApply.GetId());
			EnemyHPBar.bossol = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetBossOutlineImage());
			EnemyHPBar.bossbg = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetBossBackgroundImage());
			EnemyHPBar.bossfg = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetBossForegroundImage());
			EnemyHPBar.ol = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetOutlineImage());
			EnemyHPBar.fg = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetForegroundImage());
			EnemyHPBar.mg = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetMiddlegroundImage());
			EnemyHPBar.bg = EnemyHPBar.instance.HPBarCreateSprite(ResourceLoader.GetBackgroundImage());
		}
		internal static string[] getSkinNameArray() => EnemyHPBar.SkinList.Select(s => HPBarList.MaxLength(s.GetName(), EnemyHPBar.instance.globalSettings.NameLength)).ToArray();
		internal static Menu PrepareMenu() => new Menu("EnemyHPBar", new Element[] {
				new HorizontalOption(
					"Select Skin", "The skin will be used for current",
					getSkinNameArray(),
					(setting) => { selectedSkin = setting; },
					() => selectedSkin,
					Id:"SelectSkinOption"),
				new MenuRow(
					new List<Element>{
						Blueprints.NavigateToMenu( "Skin List","Opens a list of Skins",()=> HPBarList.GetMenu(MenuRef.menuScreen)),
						 new MenuButton("Apply Skin","Apply The currently selected skin.",(_)=> ApplySkin()),
					},
					Id:"ApplyButtonGroup"
				){ XDelta = 400f},

			});
		internal static void SelectedSkin(string skinId) => selectedSkin = EnemyHPBar.SkinList.FindIndex(skin => skin.GetId() == skinId);
		public static ISelectableSkin GetSkinById(string id) => EnemyHPBar.SkinList.Find(skin => skin.GetId() == id) ?? GetDefaultSkin();
		public static ISelectableSkin GetDefaultSkin() {
			if (EnemyHPBar.instance.DefaultSkin == null) {
				EnemyHPBar.instance.DefaultSkin = GetSkinById("Default");
			}
			return EnemyHPBar.instance.DefaultSkin;
		}
		public static void SetSkinById(string id) {
			ISelectableSkin Skin = GetSkinById(id);
			if (EnemyHPBar.instance.CurrentSkin.GetId() == Skin.GetId()) { return; }
			EnemyHPBar.instance.CurrentSkin = Skin;
		}
	}
}
