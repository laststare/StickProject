using CodeBase.Game.Gameplay.Player;
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
        public PlayerView PlayerView();
        public Transform Stick();
        public RewardView RewardView();
        public RewardConfig RewardConfig();
        public LevelConfig LevelConfig();
    }
}