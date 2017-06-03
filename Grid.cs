using System.Collections;
using System.Collections.Generic;
using UnityEngine;	// TODO We have it for Mathf.max(). Is it safe to use C# max instead of Unity Max?

// Uniform grid for collision detection. I'm planning to feed the Grid with only
// player position (right now, mouse clicks) for collision detection
public class Grid
{
	// TODO Right now only supports a single object per cell
	private int mRowSize, mColSize; 
	private float mCellSize, mInvCellSize;
	private Vector2 mWorldOrigin;

	//TODO List of List<int> seems like a better option than array of List<>
	//	   or just array...
	//private int[,] mGrid;
	private List<List<List<int>>> mGrid;


	public Grid(int rowCount, int colCount, float cellSize, Vector2 originWorld)
	{
		mRowSize = rowCount;
		mColSize = colCount;
		mCellSize = cellSize;

		mInvCellSize = 1.0f / mCellSize;

		mWorldOrigin = originWorld;

		mGrid = new List<List<List<int>>>();
		for (int row = 0; row < mRowSize; row++)
		{
			mGrid.Add (new List<List<int>> ());
			for (int col = 0; col < mColSize; col++)
			{
				mGrid [row].Add(new List<int> ());
			}

		}
	
	}

    private struct CellInfo
    {
        public int startRow, startCol, endRow, endCol;
    }

    private void getCells(ref CellInfo cell, QuadCollider collider)
    {
        Vector2 top = project(collider.TOP_LEFT);
        Vector2 bot = project(new Vector2(collider.BOTTOM_RIGHT_X, collider.BOTTOM_RIGHT_Y));

        float EPSILON = 0.00001f;

        cell.startRow = Mathf.Max(0, (int)((top.y - EPSILON) * mInvCellSize));
        cell.startCol = Mathf.Max(0, (int)((top.x + EPSILON) * mInvCellSize));
        cell.endRow = Mathf.Min(mGrid.Count - 1, (int)((bot.y + EPSILON) * mInvCellSize));
        cell.endCol = Mathf.Min(mGrid[0].Count - 1, (int)((bot.x - EPSILON) * mInvCellSize));

        if (cell.endRow < cell.startRow)
        {
            int temp = cell.endRow;
            cell.endRow = cell.startRow;
            cell.startRow = temp;
        }

        if (cell.endCol < cell.startCol)
        {
            int temp = cell.endCol;
            cell.endCol = cell.startCol;
            cell.startCol = temp;
        }
    }

	private Vector2 project(Vector2 vec)
	{
		return new Vector2( (vec.x - mWorldOrigin.x), (vec.y - mWorldOrigin.y) );
	}

	public void insert(int id, QuadCollider collider)
	{
        CellInfo cell = new CellInfo();
        getCells(ref cell, collider);

		for (int row = cell.startRow; row <= cell.endRow; row++)
		{
			for (int col = cell.startCol; col <= cell.endCol; col++)
			{
				mGrid[row][col].Add(id);
			}
		}

	}

	public List<int> checkCollision(QuadCollider collider)
	{
		List<int> ids = new List<int>();

        CellInfo cell = new CellInfo();
        getCells(ref cell, collider);

        for (int row = cell.startRow; row <= cell.endRow; row++)
		{
			for (int col = cell.startCol; col <= cell.endCol; col++)
			{
				if(mGrid[row][col].Count > 0)
				{

					for (int ei = 0; ei < mGrid [row] [col].Count; ei++)
					{
						ids.Add (mGrid [row] [col] [ei]);
					}
				}
			}
		}

		return ids;
	}
}
