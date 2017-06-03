using UnityEngine;

// will read LEVEL.XML(?) files and construct the level
// Related with Gamecontroller and GameLevel classes
// TODO Has no proper functionality for construction any levels other than TEST LEVEL
public class LevelController
{
	enum eTerrainType{ LAVA, GROUND, WATER, VALLEY};

	private string mTilePath = "Ciyan/Tiles/";
    private string mPrefabPath = "Ciyan/Prefabs/";

	public LevelController()
	{

	}

	// TODO You can get rid of these two (setTestLevel() and helper()) before shipping or after
	// the file based level system is introduced.
	private int helper(int _y, int nY, int nX, int tileWH, int _id, string name, string path, GameLevel level)
	{
		int counter = 0;
		int id = _id;

		for (int y = 0; y < nY; y++)
		{
			for(int x = 0; x < nX; x++)
			{
				counter++;
				int locX = tileWH * x;
				int locY = tileWH * (y + _y);
				Vector3 pos = new Vector3 (locX, locY, 1);

				//level.insert (id++, name, path, pos);
				//level.insertToRender (id++, name, path, pos);
			}
		}
		return counter;
	}

    private struct GridInfo
    {
        public int rowNumber, colNumber;
        public float tileWidth, tileHeight;
        public Vector2 originInWorld;
    }

    private void getGridInfo(ref GridInfo gridInfo)
    {
        GameObject twoSide = GameObject.Instantiate(Resources.Load(mPrefabPath + "Locater")) as GameObject;

        twoSide.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));
        twoSide.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);  // Sprite'ın pivotu ile aynı olmalı

        //float xDiff = twoSide.GetComponent<RectTransform>().offsetMax.x - twoSide.GetComponent<RectTransform>().offsetMin.x;
        //float yDiff = twoSide.GetComponent<RectTransform>().offsetMax.y - twoSide.GetComponent<RectTransform>().offsetMin.y;

        gridInfo.tileWidth = twoSide.GetComponent<RectTransform>().offsetMax.x - twoSide.GetComponent<RectTransform>().offsetMin.x;
        gridInfo.tileHeight = twoSide.GetComponent<RectTransform>().offsetMax.y - twoSide.GetComponent<RectTransform>().offsetMin.y;

        //float newYPos = twoSide.GetComponent<RectTransform>().position.y;
        //float newXPos = twoSide.GetComponent<RectTransform>().position.x;
        //float newZPos = twoSide.GetComponent<RectTransform>().position.z;

        gridInfo.originInWorld = new Vector2(twoSide.GetComponent<RectTransform>().position.x, twoSide.GetComponent<RectTransform>().position.y);

        GameObject gI = GameObject.Instantiate(Resources.Load(mPrefabPath + "Locater")) as GameObject;
        gI.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);   // Sprite'ın pivotu ile aynı olmalı
        gI.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1));

        float yDistance = gI.GetComponent<RectTransform>().offsetMin.y - twoSide.GetComponent<RectTransform>().offsetMin.y;

        GameObject gII = GameObject.Instantiate(Resources.Load(mPrefabPath + "Locater")) as GameObject;
        gII.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);  // Sprite'ın pivotu ile aynı olmalı
        gII.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1));

        float xDistance = gII.GetComponent<RectTransform>().offsetMin.x - twoSide.GetComponent<RectTransform>().offsetMin.x;

        //int rowNumber = Mathf.CeilToInt(yDistance / yDiff);
        //int colNumber = Mathf.CeilToInt(xDistance / xDiff);

        gridInfo.rowNumber = Mathf.CeilToInt(yDistance / gridInfo.tileHeight);
        gridInfo.colNumber = Mathf.CeilToInt(xDistance / gridInfo.tileWidth);

        GameObject.Destroy(twoSide);
        GameObject.Destroy(gI);
        GameObject.Destroy(gII);
    }

	public GameLevel setTestLevel()
	{
        // Introduce terrain
        // Read terrain
        // Generate GameLevel object

        //// TODO 24 is constant now, but make it smarter
        //int tileWH = 24; // TODO Find out scale factor applied by the camera } NO! Answer is TODO in Mesh2D.cs
        //int nX = Mathf.CeilToInt(((float)mScreenWidth / tileWH));  // This column of the grid
        //int nY = Mathf.CeilToInt(((float)mScreenHeight / tileWH)); // This row of the grid

        GridInfo gridInfo = new GridInfo();
        getGridInfo(ref gridInfo);

        //GameLevel newLevel = new GameLevel (nY, nX, tileWH);

        GameLevel newLevel = new GameLevel( gridInfo.rowNumber, gridInfo.colNumber,
                                            ((gridInfo.tileWidth > gridInfo.tileHeight) ? gridInfo.tileWidth : gridInfo.tileHeight),
                                            gridInfo.originInWorld);

        
        // Add objects into the lists

        Vector3 mid = new Vector3(Screen.width / 2, Screen.height / 2, 1);

        TwoGameObject tgo = new TwoGameObject(0, "door", "TwoObject", new Vector3(0, 0, 1), new Vector2(0, 0), new QuadCollider(gridInfo.tileWidth, gridInfo.tileHeight, new Vector2(0, 0),
                                                new Vector2(gridInfo.originInWorld.x, gridInfo.originInWorld.y + gridInfo.tileHeight)),
                                               mid, new Vector2(0, 0), new QuadCollider(gridInfo.tileWidth, gridInfo.tileHeight, new Vector2(0, 0), new Vector2(mid.x, mid.y + gridInfo.tileHeight)));

        // Create the player

        mid = tgo.getWorldPos("First");
        GamePlayer gp = new GamePlayer(-1, "player", "Player", new Vector2(0.5f, 0.5f), new Vector3(mid.x + gridInfo.tileWidth / 2, mid.y + gridInfo.tileHeight / 2, 1),
                new QuadCollider(gridInfo.tileWidth, gridInfo.tileHeight, new Vector2(0.5f, 0.5f), new Vector3(mid.x + gridInfo.tileWidth / 2, mid.y + gridInfo.tileHeight / 2, 1)), true);

        newLevel.initPlayer(gp);
        newLevel.insertRigidList(tgo);

        return newLevel;
        

        //// TODO In a real scenario, it is supposed to be like:
        //// - Read next tile info from the level file (probably XML)
        //// - Insert the new tile to the GameLEvel object
        //// TEST CASE:
        //// Height / 5: Valley, Height / 5 Lava, Height / 5 Ground, Height / 5 Water, Height / 5 Lava
        //int y = 0;
        //int id = 0;

        //id += helper (y, (int)Mathf.CeilToInt(((float)nY / 5)), nX, tileWH, id, "valley", mFilePath + "valley", newLevel);
        //y += Mathf.CeilToInt(((float)nY / 5));

        //id += helper (y, (int)Mathf.CeilToInt(((float)nY / 5)), nX, tileWH, id, "lava", mFilePath + "lava", newLevel);
        //y += Mathf.CeilToInt(((float)nY / 5));

        //id += helper (y, (int)Mathf.CeilToInt(((float)nY / 5)), nX, tileWH, id, "ground", mFilePath + "ground", newLevel);
        //y += Mathf.CeilToInt(((float)nY / 5));

        //id += helper (y, (int)Mathf.CeilToInt(((float)nY / 5)), nX, tileWH, id, "water", mFilePath + "water", newLevel);
        //y += Mathf.CeilToInt(((float)nY / 5));

        //id += helper (y, (int)Mathf.CeilToInt(((float)nY / 5)), nX, tileWH, id, "lava", mFilePath + "lava", newLevel);
        //y += Mathf.CeilToInt(((float)nY / 5));

        // TODO Make player init better and also add into mRenderList


        //TwoGameObject newObj = new TwoGameObject ();
        //newObj.init (-6, "red", mFilePath + "lava", new Vector3 (200, 240, 1), new Vector2 (0.5f, 0.5f), new QuadCollider (24, 24, new Vector2 (176, 216)),
        //	mFilePath + "green", new Vector3 (600, 240, 1), new Vector2 (0.5f, 0.5f), new QuadCollider (24, 24, new Vector2(576, 216)));

        //newLevel.mPlayer.init(	-1, "player", mFilePath + "player", new Vector3 (400, 240, 1),
        //						new Vector2 (0.5f, 0.5f), new QuadCollider(24, 24, new Vector2(376, 216)));

        //newLevel.insertToCollision (id++, "down", new Vector2 (mScreenHeight / 5 * 2, 0), new Vector2 (mScreenWidth, 0));
        //newLevel.insertRigidArea(id++, "down", new QuadCollider(800, 170, new Vector2(0, 170)) );

        //return newLevel;
    }
}
