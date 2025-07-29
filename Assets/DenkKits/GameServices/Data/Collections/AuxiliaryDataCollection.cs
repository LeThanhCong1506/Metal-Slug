using ScriptableObjectArchitecture;
using UnityEngine;

namespace DenkKits.GameServices.Data.Collections
{
	[CreateAssetMenu(
	    fileName = "AuxiliaryDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "AuxiliaryDataCollection",
	    order = 120)]
	public class AuxiliaryDataCollection : Collection<AuxiliaryData>
	{
	}
}