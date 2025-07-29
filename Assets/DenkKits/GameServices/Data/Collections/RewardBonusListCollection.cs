using ScriptableObjectArchitecture;
using UnityEngine;

namespace DenkKits.GameServices.Data.Collections
{
	[CreateAssetMenu(
	    fileName = "RewardBonusListCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "RewardBonusListCollection",
	    order = 120)]
	public class RewardBonusListCollection : Collection<RewardBonusList>
	{
	}
}