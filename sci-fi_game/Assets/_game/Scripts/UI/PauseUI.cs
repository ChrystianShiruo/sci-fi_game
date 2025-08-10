using Game.GameLoop;
using System;
using UnityEngine;

namespace Game.UI {
    public class PauseUI : MonoBehaviour {

        [SerializeField] private GameObject _pausePanel;

        private void Awake() {
            ShowPausePanel(false);
        }
        private void OnEnable() {
            GameStateManager.OnStateChanged += CheckState;
        }       

        private void OnDisable() {
            GameStateManager.OnStateChanged -= CheckState;
        }


        private void CheckState(GameStateManager.GameState state) {
            ShowPausePanel(state == GameStateManager.GameState.Paused);
        }

        private void ShowPausePanel(bool paused) {
            _pausePanel.SetActive(paused);
        }
    }
}