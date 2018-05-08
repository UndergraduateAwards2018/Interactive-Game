using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_colour : MonoBehaviour {

	public Sprite spriteY;
	public Sprite spriteO; 
	public Sprite spriteP; 
	public Sprite spriteG;
	public GameController stateMachine; 

	public SpriteRenderer spriteRenderer; 

	public void StartChanging ()
	{
		
		if (spriteRenderer.sprite == null) 
			spriteRenderer.sprite = spriteY ;
	}

	public void UpdateSprite ()
	{
		if (stateMachine.currentState == GameController.GameState.Orange) {

			spriteRenderer.sprite = spriteO;
		}

		if (stateMachine.currentState == GameController.GameState.Purple) {

			spriteRenderer.sprite = spriteP;
		}

		if (stateMachine.currentState == GameController.GameState.Green) {

			spriteRenderer.sprite = spriteG;
		}


		}
	}
