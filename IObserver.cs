// The interface to give an object observer functionality

public enum eEvenType { ON_DRAG, ON_DRAG_END };

interface IObserver
{
	void onNotify(eEvenType _event);
}
