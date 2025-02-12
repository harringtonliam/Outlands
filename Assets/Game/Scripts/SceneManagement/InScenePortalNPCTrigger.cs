using UnityEngine;



namespace RPG.SceneManagement
{
    public class InScenePortalNPCTrigger : MonoBehaviour
    {
        InScenePortal inScenePortal;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            inScenePortal = transform.parent.GetComponent<InScenePortal>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") return;
            inScenePortal.UpdatePortalActivator(other.gameObject);
        }

    }

}
