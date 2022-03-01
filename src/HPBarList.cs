using System.Collections;

using Satchel.BetterMenus;

using UnityEngine;

namespace EnemyHPBar {
	public interface ISelectableSkin {
		/// <summary>
		///  GetId
		/// </summary>
		/// <returns>The unique id of the skin as a <c>string</c></returns>
		public string GetId();

		/// <summary>
		///  GetName
		/// </summary>
		/// <returns>The Name to be displayed in the menu as a <c>string</c></returns>
		public string GetName();
	}
	internal class HPBarList : ISelectableSkin {
		internal static Menu MenuRef;
		internal static MenuScreen lastMenu;
		private static bool applying = false;
		public string SkinDirectory = "";
		public HPBarList(string DirectoryName) => SkinDirectory = DirectoryName;
		public string GetId() => SkinDirectory;
		public string GetName() => SkinDirectory;
		internal static Menu PrepareMenu() {
			var menu = new Menu("Select a skin", new Element[]{
				new TextPanel("Select the Skin to Apply",Id:"helptext"),
				new TextPanel("Applying skin...",Id:"applying"){isVisible=false}
			});
			for (int i = 0; i < EnemyHPBar.SkinList.Count; i++) {
				menu.AddElement(ApplySkinButton(i));
			}

			return menu;
		}
		internal static string MaxLength(string skinName, int length) => skinName.Length <= length ? skinName : skinName.Substring(0, length - 3) + "...";
		internal static MenuButton ApplySkinButton(int index) {

			string ButtonText = MaxLength(EnemyHPBar.SkinList[index].GetName(), EnemyHPBar.instance.globalSettings.NameLength);
			return new MenuButton(ButtonText, "", (mb) => {
				if (!applying) {
					applying = true;
					// apply the skin
					BetterMenu.selectedSkin = index;
					GameManager.instance.StartCoroutine(applyAndGoBack());
				}
			}, Id: $"skinbutton{EnemyHPBar.SkinList[index].GetId()}");

		}
		private static void setSkinButtonVisibility(bool isVisible) {
			for (int i = 0; i < EnemyHPBar.SkinList.Count; i++) {
				Element btn = MenuRef?.Find($"skinbutton{EnemyHPBar.SkinList[i].GetId()}");
				if (btn != null) {
					btn.isVisible = isVisible;
				}
			}
			MenuRef.Update();
		}
		private static IEnumerator applyAndGoBack() {
			//update menu ui
			MenuRef.Find("helptext").isVisible = false;
			MenuRef.Find("applying").isVisible = true;
			setSkinButtonVisibility(false);
			yield return new WaitForSecondsRealtime(0.2f);
			BetterMenu.MenuRef?.Find("SelectSkinOption")?.updateAfter(_ => BetterMenu.ApplySkin());
			yield return new WaitForSecondsRealtime(0.2f);

			UIManager.instance.UIGoToDynamicMenu(lastMenu);
			yield return new WaitForSecondsRealtime(0.2f);

			//menu ui initial state
			MenuRef.Find("helptext").isVisible = true;
			MenuRef.Find("applying").isVisible = false;
			setSkinButtonVisibility(true);
		}
		internal static MenuScreen GetMenu(MenuScreen lastMenu) {
			if (MenuRef == null) {
				MenuRef = PrepareMenu();
			}

			applying = false;
			HPBarList.lastMenu = lastMenu;
			return MenuRef.GetMenuScreen(lastMenu);
		}
	}
}
