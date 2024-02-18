using UnityEngine;

namespace Michsky.DreamOS
{
    [DisallowMultipleComponent]
    [AddComponentMenu("DreamOS/Network/Network Container")]
    public class NetworkContainer : MonoBehaviour
    {
        // Resources
        [SerializeField] private NetworkManager networkManager;
      
        void Start()
        {
            ListNetworks();
        }

        public void ListNetworks()
        {
            // Check for network manager
            if (networkManager == null)
            {
                if (FindObjectsOfType(typeof(NetworkManager)).Length > 0) { networkManager = (NetworkManager)FindObjectsOfType(typeof(NetworkManager))[0]; }
                else { Debug.Log("<b>[Network Container]</b> Network Manager is missing.", this); return; }
            }

            // Delete each cached objects
            foreach (Transform child in transform) { Destroy(child.gameObject); }

            // Create a new network object for each item
            networkManager.ListNetworks(transform);
        }
    }
}