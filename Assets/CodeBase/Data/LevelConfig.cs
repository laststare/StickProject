using UnityEngine;

namespace CodeBase.Data
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "GameData/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int columnOffset;
        [SerializeField] private int minColumnDistance;
        [SerializeField] private float playerYPosition;
        [SerializeField] private float playerOnColumnXOffset;
        [SerializeField] private float cameraColumnXOffset;
        [SerializeField] private float destinationOffset;

        public int GetColumnOffset => columnOffset;
        public int GetMinColumnDistance => minColumnDistance;
        public float GetPlayerYPosition => playerYPosition;
        public float GetPlayerOnColumnXOffset => playerOnColumnXOffset;
        public float GetCameraColumnXOffset => cameraColumnXOffset;
        public float GetDestinationOffset => destinationOffset;
    }
}