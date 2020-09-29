using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    [AddComponentMenu("Managers/Master Manager")]
    [HideMonoScript]
    public class ManagerController : MonoBehaviour
    {
        private ManagerController _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            DOTween.SetTweensCapacity(500, 50);

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static class Manager
    {
        public static GameBoardManager Board => _board ? _board : throw new UnityException($"No defined board manager.");

        public static void Register(BaseManager baseManager)
        {
            if (baseManager is GameBoardManager boardManager)
                _board = boardManager;
            else
                Debug.LogWarning($"[MasterManage]I don't have reference to {baseManager.GetType()}");
        }
        
        #region Managers
        
        private static GameBoardManager _board;
        
        #endregion
    }
}