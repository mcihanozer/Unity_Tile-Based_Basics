using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Listens events and notify observers (RenderObjects) -Subject in the observer pattern
public class InputController
{
	private List<RenderObject> mObservers;

	public InputController()
	{
		mObservers = new List<RenderObject> ();
	}

	public void addObserver(RenderObject observer)
	{
		mObservers.Add(observer);
	}

	public void checkForInput()
	{
		checkTouchInput ();
	}

	private void checkTouchInput()
	{
		if (Input.GetMouseButton(0))
		{
			notifyAll (eEvenType.ON_DRAG);
		}

		if (Input.GetMouseButtonUp (0))
		{
			notifyAll (eEvenType.ON_DRAG_END);
		}
	}

	private void notifyAll(eEvenType _event)
	{
		for (int oi = 0; oi < mObservers.Count; oi++)
		{
			mObservers [oi].onNotify (_event);
		}
	}

}
