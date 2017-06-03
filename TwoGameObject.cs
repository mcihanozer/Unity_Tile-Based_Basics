using UnityEngine;

// Base class for door, key, teleport, etc.
// TODO Need some updates:
// - mCollider, mCollider2, getcollider() - COLLIDER from RigidObject, etc. is not good.
//   Depending on the project direction, the whole class may need to be updated.
public class TwoGameObject : RigidObject
{
	private QuadCollider mCollider2;

    public TwoGameObject(int id, string name, string prefabName, Vector3 screenPos, Vector2 pivot, QuadCollider collider,
                         Vector3 screenPos2, Vector2 pivot2, QuadCollider collider2, bool isWorld = false)
    {
        init(id, name, prefabName, screenPos, pivot, collider, screenPos2, pivot2, collider2, isWorld );
    }

	private void init(	int id, string name, string prefabName, Vector3 screenPos, Vector2 pivot, QuadCollider collider,
						 Vector3 screenPos2, Vector2 pivot2, QuadCollider collider2, bool isWorld)
	{
        mGObject = GameObject.Instantiate(Resources.Load(mPath + prefabName)) as GameObject;

        if (!isWorld)
        {
           screenPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenPos.z));
           screenPos2 = Camera.main.ScreenToWorldPoint(new Vector3(screenPos2.x, screenPos2.y, screenPos2.z));
        }

        mCollider = collider;
		mGObject.GetComponentInChildren<Transform> ().Find ("First").GetComponent<RectTransform>().position = screenPos;
        mGObject.GetComponentInChildren<Transform>().Find("First").GetComponent<RectTransform>().pivot = pivot;

        mCollider.updatePos(screenPos);

        mCollider2 = collider2;
		mGObject.GetComponentInChildren<Transform> ().Find ("Second").GetComponent<RectTransform>().position = screenPos2;
        mGObject.GetComponentInChildren<Transform>().Find("Second").GetComponent<RectTransform>().pivot = pivot2;

        mCollider2.updatePos(screenPos2);
    }

    // TODO UNSAFE (if you pass a child without the name), MAKE BETTER AND SAFE (maybe just send child index?)
    public Vector2 getPivot(string childName)
    {
        return mGObject.GetComponentInChildren<Transform>().Find(childName).GetComponent<RectTransform>().pivot;
    }

    public Vector3 getWorldPos(string childName)
    {
        return mGObject.GetComponentInChildren<Transform>().Find(childName).GetComponent<RectTransform>().position;
    }

    public Vector3 getScreenPos(string childName)
    {
        return Camera.main.WorldToScreenPoint(getWorldPos(childName));
    }

    // Returns the collider with name
    public QuadCollider getcollider(string childName)
    {
       if(childName == "First")
        {
            return mCollider;
        }
       else if(childName == "Second")
        {
            return mCollider2;
        }

        return null;
    }

    // Returns the last collider
    public override QuadCollider getCollider()
    {
        return mCollider2;
    }

    // Collision detection code
    public override bool collide(QuadCollider other)
	{
		bool res = mCollider2.checkCollision(other);
		return res;
	}


}
