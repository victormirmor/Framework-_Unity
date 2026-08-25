using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigationManager : MonoBehaviour{

        [SerializeField] private GameObject panelToClose;
        [SerializeField] private GameObject panelToOpen;
        [SerializeField] private Selectable objectToSelect;

        public void ExecuteTransition()
        {
            panelToOpen.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(objectToSelect.gameObject);

            panelToClose.SetActive(false);
        }
    }
