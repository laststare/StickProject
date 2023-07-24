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
    public class ContentProvider : ScriptableObject
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
            public MainMenuView MainMenuView => mainMenuView;
            public ScoreCounterView ScoreCounterView => scoreCounterView;
            [SerializeField] private MainMenuView mainMenuView;
            [SerializeField] private ScoreCounterView scoreCounterView;
        }

        [Serializable]
        public class ViewsContent
        { 
            public GameObject Levelcolumn => levelcolumn;
            public CameraView CameraView => cameraView;
            public PlayerView PlayerView => playerView;
            public StickView StickView => stickView;
            [SerializeField] private GameObject levelcolumn;
            [SerializeField] private CameraView cameraView;
            [SerializeField] private PlayerView playerView;
            [SerializeField] private StickView stickView;
        }

        [Serializable]
        public class SettingsContent
        {
            public LevelConfig LevelConfig => levelConfig;
            [SerializeField] private LevelConfig levelConfig;
        }
    }
}