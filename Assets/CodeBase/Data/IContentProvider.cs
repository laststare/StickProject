using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.Player;
using CodeBase.Game.Gameplay.ScoreCounter;
using CodeBase.Game.Gameplay.Stick;
using CodeBase.Game.MainMenu;
using UnityEngine;

namespace CodeBase.Data
{
    public interface IContentProvider
    {
        public MainMenuView MainMenuView();
        public ScoreCounterView ScoreCounterView();
        public GameObject LevelColumn();
        public CameraView CameraView();
        public PlayerView PlayerView();
        public Transform Stick();
        public RewardView RewardView();
        public RewardConfig RewardConfig();
    }
}