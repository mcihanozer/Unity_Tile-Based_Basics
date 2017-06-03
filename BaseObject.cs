// Base abstract class for any kind of object that we will use
abstract public class BaseObject
{
	protected int mId;
	protected string mName;

	public BaseObject(){}

	public BaseObject(int id, string name)
	{
        mId = id;
        mName = name;
    }

	public int ID
	{
		get{ return mId; }
	}

	public string NAME
	{
		get{ return mName; }
	}
}
