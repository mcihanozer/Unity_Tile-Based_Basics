using UnityEngine;

// The base for any objects that we will be render on the screen
public class RenderObject : BaseObject, IObserver
{
    // Due to Unity obstacles replaced with a Prefab. So, only GameObject is needed
    //protected Sprite mSprite;
    //protected SpriteRenderer mRenderer;
    //protected GameObject mGObject;
    //protected Vector2 mPivot;

    protected string mPath = "Ciyan/Prefabs/";
    protected GameObject mGObject;

    public RenderObject(){}

    //public RenderObject(int id, string objName, string texturePath, Vector3 screenPos)
    public RenderObject(int id, string objName, string prefabName, Vector3 position, bool isWorld = false)
        : base(id, objName)
	{
        // If no pivot given, we assume bottom left (0,0)
        initSelf(prefabName, new Vector2(0, 0), position, isWorld);
	}

    //public RenderObject(int id, string objName, string texturePath, Vector3 screenPos, Vector2 pivot)
    public RenderObject(int id, string objName, string prefabName, Vector2 pivot, Vector3 position, bool isWorld = false)
        : base(id, objName)
	{
        initSelf(prefabName, pivot, position, isWorld);
    }

	// TODO Not so sure about making this as virtual. Make a detailed search about C# virtuals.
	// It seems that they behave different than C++ virtuals.
	//private void initSelf(string texturePath, Vector3 screenPos, Vector2 pivot)
    private void initSelf(string prefabName, Vector2 pivot, Vector3 position, bool isWorld)
	{
		//mPivot = pivot;

		//// TODO This load is bullshit! Update this with a real texture loader
		//// https://docs.unity3d.com/ScriptReference/Texture2D.LoadImage.html
		//// https://forum.unity3d.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
		//// https://forum.unity3d.com/threads/load-png-into-sprite-then-draw-sprite-onto-screen.433489/
		//// https://docs.unity3d.com/ScriptReference/Texture2D.LoadRawTextureData.html
		//Texture2D texture = Resources.Load<Texture2D>(texturePath) as Texture2D;
		//Rect rect = new Rect (0, 0, texture.width, texture.height);
		//mSprite = Sprite.Create (texture, rect, pivot);

		//mGObject = new GameObject ();
		//mGObject.name = mName;
		//mGObject.transform.position = Camera.main.ScreenToWorldPoint (screenPos);
		//mGObject.AddComponent<SpriteRenderer>();
		//mRenderer = mGObject.GetComponent<SpriteRenderer>();
		//mRenderer.sprite = this.mSprite;

        mGObject = GameObject.Instantiate(Resources.Load(mPath + prefabName)) as GameObject;
        mGObject.GetComponent<RectTransform>().pivot = pivot;

        if(isWorld)
        {
            mGObject.GetComponent<RectTransform>().position = position;
        }
        else
        {
            mGObject.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, position.z));
        }

    }

	// Override if you need aobserver
	// TODO Make RenderObject abstract as RigidObject
	public virtual void onNotify(eEvenType _event){}

    public Vector2 PIVOT
    {
        get { return mGObject.GetComponent<RectTransform>().pivot; }
    }

	public Vector3 WORLD_POS
	{
        get { return mGObject.GetComponent<RectTransform>().position; }
	}

	public Vector3 SCREEN_POS
	{
        get { return Camera.main.WorldToScreenPoint(WORLD_POS); }
    }
}
