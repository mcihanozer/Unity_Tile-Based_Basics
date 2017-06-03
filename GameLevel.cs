using System.Collections;
using System.Collections.Generic;
using UnityEngine;	// TODO Only for Vector3, adding all UnityEngine is not good...

// The current game level: From XML(?) to object
// Keeps the list of objects that will be used in GameController
public class GameLevel
{
	// Everything is public for easy access in GameController


	// TODO Also introduce List<> to Grid because of the reason below } Maybe Array of List<>s?
	public Grid mCollisionGrid;	// Keeps collision detection objetcs (Only static ones for now)

	//TODO Can we change List<> with RenderObject[]?: Yes, becayse Grid needs to know size of the terrain
	// But! What if I need to introduce new render objects? Like bullet! So, keep the List<>.
	public List<RenderObject> mRenderList; // Every object in the list

    public List<RigidObject> mRigidList;

	// TODO RigidArea and RigidObject needs a common class
	//		??? Make RigidObject base class and introduce RigidRenderObject?
	public List<RigidArea> mAreaList;

	public GamePlayer mPlayer;

	public GameLevel(int rowCount, int colCount, float cellSize, Vector2 gridOriginWorld)
	{
		mRenderList = new List<RenderObject> ();
        mRigidList = new List<RigidObject>();
        mAreaList = new List<RigidArea> ();

		mCollisionGrid = new Grid (rowCount, colCount, cellSize, gridOriginWorld);
	}


    // -------------------------------   INSERTION   ------------------------------- //
    public bool insertRenderList(RenderObject obj)
    {
        mRenderList.Add(obj);
        return true;
    }

    public bool insertRenderList(int id, string objName, string prefabName, Vector3 position, bool isWorld = false)
    {
        mRenderList.Add(new RenderObject(id, objName, prefabName, position, isWorld));
        return true;
    }

    public bool insertRenderList(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, bool isWorld = false)
    {
        mRenderList.Add(new RenderObject(id, objName, prefabName, pivot, position, isWorld));
        return true;
    }

    // TODO When the types are certain, we can add an factory to create them
    // TODO Check Builder vs Factory patterns (I think Factory is more suitable, but be sure)
    public bool insertRigidList(RigidObject obj)
    {
        if(obj == null)
        {
            Debug.Log("NULL OBJECT IN RIGID LIST");
        }
        else
        {
            mRigidList.Add(obj);
            mCollisionGrid.insert(obj.ID, obj.getCollider());
            return true;
        }

        return false;
    }

    public void initPlayer(GamePlayer player)
    {
        mPlayer = player;
    }

    public void initPlayer( int id, string objName, string prefabName, Vector3 position,
                            float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
    {
        mPlayer = new GamePlayer(id, objName, prefabName, position, colliderWidth, colliderHeight, colliderTopLeftWorldPos, isWorld);
    }

    public void initPlayer(int id, string objName, string prefabName, Vector2 pivot, Vector3 position,
                        float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
    {
        mPlayer = new GamePlayer(id, objName, prefabName, pivot, position, colliderWidth, colliderHeight, colliderTopLeftWorldPos, isWorld);
    }

    public void initPlayer(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, QuadCollider collider, bool isWorld = false)
    {
        mPlayer = new GamePlayer(id, objName, prefabName, pivot, position, collider, isWorld);
    }

    // -------------------------------   INSERTION   ------------------------------- //

    // -------------------------------   COLLISION   ------------------------------- //

    public bool checkCollision(QuadCollider collider)
    {
        List<int> colIds = mCollisionGrid.checkCollision(collider);

        if (colIds.Count > 0)
        {
            bool isCollided = false;

            for (int id = 0; id < colIds.Count; id++)
            {
                if(mRigidList[colIds[id]].collide(mPlayer.COLLIDER))
                {
                    isCollided = true;
                    Debug.Log("COLLISION HAPPENED! COL_ID: " + colIds[id] + "   and PLAYER!");
                }

            }

            return isCollided;

        }

        return false;
    }

    public bool checkPlayerCollision()
    {
        if (mPlayer.isTouched)
        {
            //return checkCollision (mPlayer.ScreenPos);
            return checkCollision(mPlayer.COLLIDER);
        }

        return false;
    }

    // -------------------------------   COLLISION   ------------------------------- //

  

    ////	public bool checkCollision(Vector3 pos)
    ////	{
    ////		int colId = mCollisionGrid.checkCollision (pos);
    ////
    ////		if (colId > -1)
    ////		{
    //////			Debug.Log ("COL ID: " + colId);
    //////			Debug.Log ("REND ID: " + mRenderList[colId].ID);
    //////			Debug.Log ("REND NAME: " + mRenderList[colId].Name);
    //////			Debug.Log ("SCR POS: " + mRenderList[colId].ScreenPos);
    //////			Debug.Log ("WOR POS: " + mRenderList[colId].WorldPos);
    ////
    ////			Debug.Log ("COL ID: " + colId);
    ////			Debug.Log ("AREA ID: " + mAreaList[colId].ID);
    ////			Debug.Log ("AREA NAME: " + mAreaList[colId].NAME);
    ////
    ////			return true;
    ////		}
    ////
    ////		return false;
    ////	}

}
