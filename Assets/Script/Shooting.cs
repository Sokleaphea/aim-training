using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            // if (Physics.Raycast(ray, out RaycastHit hit))
            // {
            //     if (hit.collider.CompareTag("Target"))
            //     {
            //         hit.collider.GetComponentInParent<Target>().OnHit(hit.point);
            //     }
            // }
            // GameManager.Instance.RegisterShot();
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = playerCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Target"))
                {
                    Target targetScript = hit.collider.GetComponentInParent<Target>();
                    if (targetScript != null)
                        targetScript.OnHit(hit.point);
                }
                GameManager.Instance.RegisterShot();

            }
        }
    }
}
