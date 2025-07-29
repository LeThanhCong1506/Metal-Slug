using UnityEngine;

namespace DenkKits.GameServices.Events
{
	public interface IGameEventListener<T> 
	{
		void OnEventRaised(Object sender, T sendData);

	}
}
