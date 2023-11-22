using CodeBase.Game.Gameplay.ScoreCounter;
using CodeBase.Game.MainMenu;
using UnityEngine;

namespace CodeBase.Data
{
    public interface IContentProvider
    {
        public MainMenuView MainMenuView();
        public ScoreCounterView ScoreCounterView();
        public GameObject LevelColumn();
        public Transform Camera();
        public Transform Player();
        public Transform Stick();
        public Transform Reward();
        public RewardConfig RewardConfig();
        public LevelConfig LevelConfig();
    }
}