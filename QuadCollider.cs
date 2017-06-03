using UnityEngine;

// Our rectangular collider
public class QuadCollider
{
	private float mWidth, mHeight;
	private Vector2 mTopLeftPos;
    private Vector2 mAnchor;

	public QuadCollider(float width, float height, Vector2 anchor, Vector2 topLeftWorldPos)
	{
		mWidth = width;
		mHeight = height;

        mAnchor = anchor;

        updatePos(topLeftWorldPos);
	}

	public float WIDTH
	{
		get{ return mWidth; }
	}

	public float HEIGHT
	{
		get{ return mHeight; }
	}

	public Vector2 TOP_LEFT
	{
		get{ return mTopLeftPos; }
	}

	public float TOP_LEFT_X
	{
		get{ return mTopLeftPos.x; }
	}

	public float TOP_LEFT_Y
	{
		get{ return mTopLeftPos.y; }
	}

    public Vector2 BOTTOM_RIGHT
    {
        get { return new Vector2(BOTTOM_RIGHT_X, BOTTOM_RIGHT_Y); }
    }

	public float BOTTOM_RIGHT_X
	{
		get{ return mTopLeftPos.x + mWidth; }
	}

	public float BOTTOM_RIGHT_Y
	{
        // Because +y is up
        get { return mTopLeftPos.y - mHeight; }
	}

    public void updatePos(Vector3 pos)
    {
        mTopLeftPos.x = pos.x - (mAnchor.x * mWidth);
        mTopLeftPos.y = pos.y + ((1 - mAnchor.y) * mHeight); // Because +y is up
    }

	public bool checkCollision(QuadCollider other)
	{
        if (mTopLeftPos.x < other.TOP_LEFT.x + other.WIDTH &&
            mTopLeftPos.x + mWidth > other.TOP_LEFT.x &&
            mTopLeftPos.y < other.TOP_LEFT.y + other.HEIGHT &&
            mTopLeftPos.y + mHeight > other.TOP_LEFT.y )
		{
			return true;
		}

		return false;
	}

}