using UnityEditor;
namespace UIMinMaxSlider  {
	public static class ContextMenuUtility {
		private const string ELEMENT_NAME_IN_RESOURCES = "UIMinMaxSlider"; 
		
		[MenuItem("GameObject/UIComponents/MinMaxSlider")]
		public static void CreateSwitcher(MenuCommand menuCommand) {
			CreateUtility.CreateUIElement(ELEMENT_NAME_IN_RESOURCES);
		}
	}
}