using System;
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
            public GameObject Levelcolumn => levelcolumn ;
            [SerializeField] private GameObject levelcolumn;
        }

        [Serializable]
        public class SettingsContent
        {
            public LevelConfig LevelConfig => levelConfig;
            [SerializeField] private LevelConfig levelConfig;
        }
    }
}