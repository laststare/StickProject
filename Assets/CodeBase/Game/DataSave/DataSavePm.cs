using External.Framework;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.DataSave
{
    public class DataSavePm : BaseDisposable, IDataSave
    {
        public struct Ctx
        {
            public ReactiveProperty<IDataSave> dataSave;
        }
        private readonly Ctx _ctx;

        public DataSavePm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.dataSave.Value = this;
        }

        public void SaveBestScore(int bestScore)
        {
            PlayerPrefs.SetInt(Constant.SavedScore, bestScore);
        }

        public int LoadBestScore()
        {
            return PlayerPrefs.GetInt(Constant.SavedScore);
        }
    }
}