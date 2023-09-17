using Managers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PrometeoCarController _carController;
        [SerializeField] private GameObject _canvasObject;
        [SerializeField] private GameObject _camera;

        [HideInInspector] public int id;

        public Player photonPlayer;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        [PunRPC]
        public void Initialize(Player player)
        {
            photonPlayer = player;
            id = player.ActorNumber;
            GameManager.Instance.players[id - 1] = this;

            if (!photonView.IsMine)
            {
                _rigidbody.isKinematic = true;
                _carController.enabled = false;
                _canvasObject.SetActive(false);
                _camera.SetActive(false);
            }
        }
    }
}