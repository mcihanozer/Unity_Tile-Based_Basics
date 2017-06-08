using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will evolve to the game controller. It includes game loop (update())
// and also the only MonoBehaviour inherited class.
public class GameController : MonoBehaviour
{
	private InputController mInputController;
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
		mInputController = new InputController();
		mLevelController = new LevelController ();
		mLevel = mLevelController.setTestLevel ();

		mInputController.addObserver (mLevel.mPlayer);

		return true;
	}
		
    private void handleInput()
	{
		mInputController.checkForInput ();
	}

	private void callFrameMid()
	{
		// Handle collision detection of the player
		// TODO Introduce another handler for this? Or leave to level since it has the grid?
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
