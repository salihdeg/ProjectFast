using Controllers;
using Photon.Pun;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager Instance;

        [Header("Stats")]
        public bool gameEnd = false;

        [Header("Players")]
        public string playerPrefabLocation;

        public Transform[] spawnPoints;
        public PlayerController[] players;

        private int _playersInGame;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            players = new PlayerController[PhotonNetwork.PlayerList.Length];

            photonView.RPC("IAmInGame", RpcTarget.All);
        }

        [PunRPC]
        private void IAmInGame()
        {
            _playersInGame++;

            //If All Players Connected Spawn Players
            if (_playersInGame == PhotonNetwork.PlayerList.Length)
            {
                SpawnPlayer();
            }
        }

        [PunRPC]
        private void WinGame(int playerId)
        {
            gameEnd = true;

            PlayerController player = GetPlayer(playerId);

            GameUIManager.Instance.SetWinTextAndActivate(player.photonPlayer.NickName);

            Invoke("BackToMenu", 3f);
        }

        public void BackToMenu()
        {
            PhotonNetwork.LeaveRoom();
            NetworkManager.Instance.ChangeScene(0);
            Destroy(NetworkManager.Instance.gameObject);
        }

        private void SpawnPlayer()
        {
            GameObject playerObj =
                PhotonNetwork.Instantiate(
                    playerPrefabLocation + PhotonNetwork.LocalPlayer.ActorNumber,
                    new Vector3(0f, 0f, 0f),
                    Quaternion.Euler(0f, 90f, 0f));

            PlayerController controller = playerObj.GetComponent<PlayerController>();

            playerObj.transform.position = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;

            controller.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }

        public PlayerController GetPlayer(int playerId)
        {
            return players.First(p => p.id == playerId);
        }

        public PlayerController GetPlayer(GameObject playerObj)
        {
            return players.First(p => p.gameObject == playerObj);
        }
    }

}