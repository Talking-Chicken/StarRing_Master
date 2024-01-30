using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
	public class InitDreamOS : MonoBehaviour
	{
		[InitializeOnLoad]
		public class InitOnLoad
		{
			static InitOnLoad()
			{
				if (EditorPrefs.HasKey("DreamOSv2.Installed") && !EditorPrefs.HasKey("DreamOSv3.Installed"))
				{
                    EditorPrefs.SetInt("DreamOSv3.Installed", 1);
                    EditorUtility.DisplayDialog("Hello there!", "Thank you for upgrading DreamOS to the version 3." +
						"\r\rIf you need help, feel free to contact us through our support channels.", "Got it");
				}

				else if (!EditorPrefs.HasKey("DreamOSv3.Installed"))
				{
					EditorPrefs.SetInt("DreamOSv3.Installed", 1);
					EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing DreamOS." +
						"\r\rIf you need help, feel free to contact us through our support channels.", "Got it");
				}
			}
		}
	}
}