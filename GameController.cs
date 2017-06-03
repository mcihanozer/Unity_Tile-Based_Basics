using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will evolve to the game controller. It includes game loop (update())
// and also the only MonoBehaviour inherited class.
public class GameController : MonoBehaviour
{
	private LevelController mLevelController;
	private GameLevel mLevel;

	// Use this for initialization
	void Start ()
	{
        Application.targetFrameRate = 60;

		if (!init ())
		{
			Debug.Log ("init() at GameController.cs FAILED!");
		}

	}

	private bool init()
	{
		// TODO Make more generic by gettin level information
		mLevelController = new LevelController ();
		mLevel = mLevelController.setTestLevel ();
		return true;
	}

    // TODO will be moved to the InputManager or sth
	private void onDrag()
	{
		bool isPlayerMove = false;

        if (!mLevel.mPlayer.isTouched)
        {
            if (mLevel.mPlayer.checkForTouch(Input.mousePosition))
            {
                isPlayerMove = true;
            }
        }
        else
        {
            isPlayerMove = true;
        }

        if (isPlayerMove)
        {
            mLevel.mPlayer.setPosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        }
    }

    // TODO will be moved to the InputManager or sth
    private void onTouchEnd()
	{
		mLevel.mPlayer.unTouch ();
	}

    // TODO will be moved to the InputManager
    private void handleInput()
	{
		// TODO Introduce Observer method. Player should handle everything below depending
		//      on the event that it gets.
		if (Input.GetMouseButton(0))
		{
			onDrag ();
		}

		if (Input.GetMouseButtonUp (0))
		{
			onTouchEnd ();
		}

	}

	private void callFrameMid()
	{
		// Handle collision detection of the player
		if(mLevel.checkPlayerCollision())
		{
			Debug.Log ("COLLISION HAPPENED");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Clear screen

		// Early update frame: NPC update

		// Get user input
		handleInput();

		// Mid-frame update: NPC - User interaction 
		callFrameMid();

		// Frame end operations

	}
}
