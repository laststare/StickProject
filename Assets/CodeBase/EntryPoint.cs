using CodeBase.Data;
using UnityEngine;

namespace CodeBase
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private ContentProvider contentProvider;
        [SerializeField] private RectTransform uiRoot;
        private Root _root;
        void Start()
        {
            var rootCtx = new Root.Ctx
            {
                contentProvider = contentProvider,
                uiRoot = uiRoot,
              
            };
            _root = Root.CreateRoot(rootCtx);
        }
        
    }
}
