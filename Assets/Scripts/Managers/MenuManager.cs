using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        [Header("Screens")]
        [SerializeField] private GameObject _loginScreen;
        [SerializeField] private GameObject _lobbyScreen;

        [Header("Login Screen")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;

        [Header("Lobby Screen")]
        [SerializeField] private TextMeshProUGUI _playerListText;
        [SerializeField] private TextMeshProUGUI _lobbyNameText;
        [SerializeField] private Button _startGameButton;

        private void Start()
        {
            //Disable buttons at start
            ChangeLoginScreenButtonsStatus(false);
        }

        private void ChangeLoginScreenButtonsStatus(bool status)
        {
            _createRoomButton.interactable = status;
            _joinRoomButton.interactable = status;
        }

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            // When connected to servers activate the buttons
            ChangeLoginScreenButtonsStatus(true);
        }

        public override void OnJoinedRoom()
        {
            // When player joined the room,
            // Set lobby screen
            // Update lobby name text on UI
            // Update player list for all users
            SetScreen(_lobbyScreen);
            _lobbyNameText.text = PhotonNetwork.CurrentRoom.Name;
            photonView.RPC("UpdateLobbyPlayersText", RpcTarget.All);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            // When anyone left the room Update player list on UI
            UpdateLobbyPlayersText();
        }

        #endregion

        [PunRPC]
        public void UpdateLobbyPlayersText()
        {
            _playerListText.text = string.Empty;

            //List All Players
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                _playerListText.text += PhotonNetwork.PlayerList[i].NickName + "\n";
            }

            //Enable start button if player is lobby owner
            if (PhotonNetwork.IsMasterClient)
                _startGameButton.interactable = true;
            else
                _startGameButton.interactable = false;
        }

        public void SetScreen(GameObject screen)
        {
            _loginScreen.SetActive(false);
            _lobbyScreen.SetActive(false);

            screen.SetActive(true);
        }

        public void OnCreateRoomButton(TMP_InputField roomNameInput)
        {
            NetworkManager.Instance.CreateRoom(roomNameInput.text);
        }

        public void OnJoinRoomButton(TMP_InputField roomNameInput)
        {
            NetworkManager.Instance.JoinRoom(roomNameInput.text);
        }

        public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }

        public void OnStartGameButton()
        {
            NetworkManager.Instance.photonView.RPC("ChangeScene", RpcTarget.All, 1);
        }

        public void OnLeaveRoomButton()
        {
            PhotonNetwork.LeaveRoom();
            SetScreen(_loginScreen);
        }
    }
}