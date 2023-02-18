using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 2f))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.OnInteraction();
                }
            }
        }
    }

}
