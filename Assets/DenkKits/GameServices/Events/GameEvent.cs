using System.Collections.Generic;
using UnityEngine;

namespace DenkKits.GameServices.Events
{
	
	[System.Serializable]
	public abstract class GameEvent<T> : ScriptableObject
	{
		private List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

		public void Raise(Object sender, T sendData)
		{
			for (int i = listeners.Count - 1; i >= 0; i--)
				listeners[i].OnEventRaised(sender, sendData);
		}

		public void RegisterListener(IGameEventListener<T> listener)
		{ listeners.Add(listener); }

		public void UnregisterListener(IGameEventListener<T> listener)
		{ listeners.Remove(listener); }

	}
}
