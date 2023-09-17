using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Managers
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance == this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        [PunRPC]
        public void ChangeScene(int sceneIndex)
        {
            PhotonNetwork.LoadLevel(sceneIndex);
        }

        #region Photon Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Servers!");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected!");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room Created!\nRoom Name = " + PhotonNetwork.CurrentRoom.Name);
        }
        #endregion
    }
}