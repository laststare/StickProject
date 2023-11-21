using External.Framework;
using UniRx;

namespace CodeBase.Game.DataSave
{
    public class DataSaveEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<IDataSave> dataSave;
        }
        private readonly Ctx _ctx;
        private DataSavePm _pm;

        public DataSaveEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
        }

        private void CreatePm()
        {
            var dataSavePmCtx = new DataSavePm.Ctx()
            {
                dataSave = _ctx.dataSave
            };
            _pm = new DataSavePm(dataSavePmCtx);
            AddToDisposables(_pm);
        }
    }
}