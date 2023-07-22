﻿using System;
using CodeBase.Game.Gameplay.Camera;
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
            
        }

        [Serializable]
        public class ViewsContent
        { 
            public GameObject Levelcolumn => levelcolumn;
            public CameraView CameraView => cameraView;
            [SerializeField] private GameObject levelcolumn;
            [SerializeField] private CameraView cameraView;
        }

        [Serializable]
        public class SettingsContent
        {
            public LevelConfig LevelConfig => levelConfig;
            [SerializeField] private LevelConfig levelConfig;
        }
    }
}