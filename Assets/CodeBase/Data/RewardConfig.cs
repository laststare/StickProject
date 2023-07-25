using UnityEngine;

namespace CodeBase.Data
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "GameData/RewardConfig")]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField] private int oneColumnReward;
        public int OneColumnReward => oneColumnReward;
       
    }
}