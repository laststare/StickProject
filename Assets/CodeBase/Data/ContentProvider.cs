using System;
using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.Player;
using CodeBase.Game.Gameplay.ScoreCounter;
using CodeBase.Game.Gameplay.Stick;
using CodeBase.Game.MainMenu;
using UnityEngine;

namespace CodeBase.Data
{
    [CreateAssetMenu(fileName = "ContentProvider", menuName = "GameData/ContentProvider")]
    public class ContentProvider : ScriptableObject, IContentProvider
    {
        public UIViewsContent UIViews => uiViews;
        public ViewsContent Views => views;
        public SettingsContent Settings => settings;
        
        [SerializeField] private ViewsContent views;
        [Space]
        [SerializeField] private UIViewsContent uiViews;
        [Space]
        [SerializeField] private SettingsContent settings;

        [Serializable]
        public class UIViewsContent
        {
            public MainMenuView mainMenuView;
            public ScoreCounterView scoreCounterView;
        }
        
        [Serializable]
        public class ViewsContent
        { 
            public GameObject levelColumn;
            public CameraView cameraView;
            public PlayerView playerView;
            public Transform stick;
            public RewardView rewardView;
        }
        
        [Serializable]
        public class SettingsContent
        {
            public RewardConfig rewardConfig;
            public LevelConfig levelConfig;
        }
        
        public MainMenuView MainMenuView() => UIViews.mainMenuView;
        public ScoreCounterView ScoreCounterView() => UIViews.scoreCounterView;
        public GameObject LevelColumn()  => Views.levelColumn;
        public CameraView CameraView()  => Views.cameraView;
        public PlayerView PlayerView()  => Views.playerView;
        public Transform Stick()  => Views.stick;
        public RewardView RewardView()  => Views.rewardView;
        public RewardConfig RewardConfig()  => Settings.rewardConfig;
        public LevelConfig LevelConfig() => Settings.levelConfig;
    }
}