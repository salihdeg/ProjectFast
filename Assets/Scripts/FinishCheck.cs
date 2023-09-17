using Controllers;
using Managers;
using Photon.Pun;
using UnityEngine;

public class FinishCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            GameManager.Instance.gameEnd = true;
            GameManager.Instance.photonView.RPC("WinGame", RpcTarget.All, playerController.id);
            gameObject.SetActive(false);
        }
    }
}
