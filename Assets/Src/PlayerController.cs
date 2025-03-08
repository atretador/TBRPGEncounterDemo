using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    public Entity entityToControl;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(entityToControl.OnEncounter)
        {
            EncounterControls();
        }
        else
        {
            FreeRoamControls();
        }
    }

    public void EncounterControls()
    {
        // this is just a basic implementation, so we are calling the player UI here
        // but should probably be used in the Party class
        entityToControl.ShowEntityInfo(true);

        Entity ClickedOn = null;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                ClickedOn = hit.collider.GetComponent<Entity>();
                if (ClickedOn != null)
                {
                    entityToControl.Attack(ClickedOn);
                }
            }
        }
    }

    public void FreeRoamControls()
    {
        if(Input.GetMouseButtonDown(0) && entityToControl != null && entityToControl.isReady)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            entityToControl.StartMoveToVector3Position(mouseWorldPos);
        }
    }
}
