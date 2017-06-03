using System;
using UnityEngine;

//The class that is controlled by the player
// TODO Remove old Player class and start using this one
public class GamePlayer : RigidObject
{
	bool mIsTouched;

    RectTransform mSelfTransform;

	public GamePlayer(){}

    public GamePlayer(  int id, string objName, string prefabName, Vector3 position,
                        float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
        : base(id, objName, prefabName, position, colliderWidth, colliderHeight, colliderTopLeftWorldPos, isWorld)
    {
        init();
    }

    public GamePlayer(  int id, string objName, string prefabName, Vector2 pivot, Vector3 position,
                        float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
        : base(id, objName, prefabName, pivot, position, colliderWidth, colliderHeight, colliderTopLeftWorldPos, isWorld)
    {
        init();
    }

    public GamePlayer(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, QuadCollider collider, bool isWorld = false)
        : base(id, objName, prefabName, pivot, position, collider, isWorld)
    {
        init();
    }

    private void init()
    {
        mIsTouched = false;

        mSelfTransform = mGObject.GetComponent<RectTransform>();
    }

	public bool isTouched
	{
		get{ return mIsTouched; }
	}

	public void unTouch()
	{
		mIsTouched = false;
	}

	public void setPosition(Vector3 pos)
	{
		if (mIsTouched)
		{
            //mGObject.transform.position = Camera.main.ScreenToWorldPoint (pos);
            mSelfTransform.position = Camera.main.ScreenToWorldPoint(pos);
            mCollider.updatePos(WORLD_POS);
        }

	}

	public bool checkForTouch(Vector3 pos)
	{
        // Gives same result
        //Debug.Log("mGObject.transform.position " + mGObject.transform.position);
        //Debug.Log("mSelfTransform.position " + mSelfTransform.position);

        Vector3 test = Camera.main.ScreenToWorldPoint(pos);
        test.z = mSelfTransform.position.z;
        // mSelfTransform.rect.Contains(test) does not work because of a reason } It seems like there is a general problem with it.
        // Because, RectTransform.rect.Contains() always return true somehow...
        mIsTouched = mGObject.GetComponent<SpriteRenderer>().bounds.Contains(test);

        return mIsTouched;
	}

    public override QuadCollider getCollider()
    {
        return COLLIDER;
    }

    public override bool collide(QuadCollider other)
    {
        return other.checkCollision(mCollider);
    }
}
