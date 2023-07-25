using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class RewardView : MonoBehaviour
    {

        [SerializeField] private TMP_Text text;

        private void Start()
        {
            transform.DOMoveY(transform.position.y + 3, 2);
            text.DOFade(0, 2);
        }
        
    }
}