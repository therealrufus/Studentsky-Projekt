using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Photon.Pun
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        static public RoomManager Instance;

        private GameObject instance;

        [SerializeField]
        private GameObject playerPrefab;

        private void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Lobby");

                return;
            }

            if (NetworkPlayerMovement.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                GameObject instance = PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            }
            else
            {

                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        void Update()
        {
            // "back" button of phone equals "Escape". quit app if that's pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
