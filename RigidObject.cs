using UnityEngine;

// Abstract class for making an object rigidody for collision
abstract public class RigidObject : RenderObject, ICollider
{
	protected QuadCollider mCollider;

	public RigidObject(){}

    public RigidObject(int id, string objName, string prefabName, Vector3 position, float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
        : base(id, objName, prefabName, position, isWorld)
    {
        initSelf(new QuadCollider(colliderWidth, colliderHeight, new Vector2(0,0), colliderTopLeftWorldPos));
    }

    public RigidObject(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, float colliderWidth, float colliderHeight, Vector2 colliderTopLeftWorldPos, bool isWorld = false)
        : base(id, objName, prefabName, pivot, position, isWorld)
    {
        initSelf(new QuadCollider(colliderWidth, colliderHeight, pivot, colliderTopLeftWorldPos));
    }

    public RigidObject(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, QuadCollider collider, bool isWorld = false)
        : base(id, objName, prefabName, pivot, position, isWorld)
    {
        initSelf(collider);
    }

    private void initSelf(QuadCollider collider)
    {
        mCollider = collider;
    }

    public QuadCollider COLLIDER
	{
		get
		{ 
			mCollider.updatePos(WORLD_POS);
			return mCollider;
		}
	}

    public abstract QuadCollider getCollider(); // Return action collider

    public abstract bool collide( QuadCollider other);  // Use for collision detection
    

}