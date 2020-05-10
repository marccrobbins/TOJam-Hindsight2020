using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManagerTest : MonoBehaviour
{
    IEnumerator Start()
    {
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.StartGame();

		if (ReInput.isReady)
		{
			Player player = ReInput.players.GetPlayer(0);

			if (player != null)
			{
				player.controllers.maps.SetMapsEnabled(false, "Menu");
				player.controllers.maps.SetMapsEnabled(true, "InGame");
			}
		}

		yield return null;
	}
}
