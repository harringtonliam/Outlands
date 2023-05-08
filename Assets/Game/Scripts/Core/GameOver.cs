using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using RPG.Control;


namespace RPG.Core
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] UnityEvent gameOverActions;
        [SerializeField] string gameOverSceneName;
        [Tooltip("Seconds to wait before loading Game Over Scene")]
        [SerializeField] float waitTime = 2f;

        private static GameOver _instance;

        public static GameOver Instance {  get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        public void GameOverActions()
        {
            gameOverActions.Invoke();
        }

        public void EndTheGame()
        {
            Debug.Log("Gameover end the game");

            DisablePlayerControl();
            gameOverActions.Invoke();
            StartCoroutine(LoadGameOverScene());
        }

        private IEnumerator LoadGameOverScene()
        {
            yield return new WaitForSeconds(waitTime);

            if (!string.IsNullOrEmpty(gameOverSceneName))
            {
                SceneManager.LoadScene(gameOverSceneName);
            }

        }

        private void DisablePlayerControl()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                player.GetComponent<ActionScheduler>().CancelCurrentAction();
                player.GetComponent<PlayerController>().enabled = false;
            }

        }


    }

}


