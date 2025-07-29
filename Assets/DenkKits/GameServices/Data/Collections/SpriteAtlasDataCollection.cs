using DenkKits.GameServices.Manager.ResourceManager;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace DenkKits.GameServices.Data.Collections
{
	[CreateAssetMenu(
	    fileName = "SpriteAtlasDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "SpriteAtlasDataCollection",
	    order = 120)]
	public class SpriteAtlasDataCollection : Collection<SpriteAtlasData>
	{
	}
}