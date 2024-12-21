using UnityEngine;

namespace RPG.GameTime
{
    public class MoonLightController : MonoBehaviour
    {
        [SerializeField] private Light moonLight;
        [SerializeField] private int dayVisibleFrom;
        [SerializeField] private int dayVisibleTo;


        GameTimeContoller gameTimeContoller;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameTimeContoller = GetComponent<GameTimeContoller>();
            gameTimeContoller.dayHasPassed += UpdateMoonLight;
            UpdateMoonLight();

        }

        private void OnDisable()
        {
            try
            {
                gameTimeContoller.dayHasPassed -= UpdateMoonLight;
            }
            catch (System.Exception ex)
            {
                Debug.Log("MoonLightController OnDisable " + ex.Message);
            }
        }


        private void UpdateMoonLight()
        {
            moonLight.gameObject.SetActive(IsMoonVisible());
        }

        private bool IsMoonVisible()
        {
            return gameTimeContoller.CurrentLocalDayOfMonth >= dayVisibleFrom && gameTimeContoller.CurrentLocalDayOfMonth <= dayVisibleTo;
        }
    }


}


