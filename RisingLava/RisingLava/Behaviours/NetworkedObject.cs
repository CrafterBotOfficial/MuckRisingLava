using UnityEngine;
using UnityEngine.SceneManagement;

namespace RisingLava.Behaviours
{
    internal class NetworkedObject : MonoBehaviour
    {
        private void Start()
        {
            transform.position = new Vector3(0, 7, 0);
            SceneManager.activeSceneChanged += (Scene scene, Scene scene1) =>
            {
                gameObject.SetActive(false);
                Destroy(this);
            };
        }
        private void FixedUpdate()
        {
            if (LocalClient.serverOwner)
            {
                transform.position += Vector3.up * (Main.LavaSpeed.Value/1000);
                Networking.NetworkManager.MoveLava(transform.position.y);
            }
            if (PlayerMovement.Instance.transform.position.y < transform.position.y)
            {
                PlayerMovement.Instance.transform.position = new Vector3(PlayerMovement.Instance.transform.position.x, transform.position.y, PlayerMovement.Instance.transform.position.z);
                PlayerStatus.Instance.DealDamage(1);
            }
        }
    }
}
