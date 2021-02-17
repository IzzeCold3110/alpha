using UnityEngine;

public class AddFreeFlyCamera : MonoBehaviour
{
    bool attached_ = false;
    bool changeToggle = false;

    public KeyCode ToggleKey;

    private bool isFlyCameraAttached()
    {
        return (Camera.main.transform.GetComponent<FreeFlyCamera>() != null);
    }

    void Start()
    {
        attached_ = isFlyCameraAttached();
    }

    void Detach()
    {
        attached_ = isFlyCameraAttached();
        if (attached_)
        {
            Destroy(Camera.main.gameObject.GetComponent<FreeFlyCamera>());
            attached_ = isFlyCameraAttached();
        }
    }

    void Attach()
    {
        attached_ = isFlyCameraAttached();
        if (!attached_)
        {
            Camera.main.transform.gameObject.AddComponent<FreeFlyCamera>();
            attached_ = isFlyCameraAttached();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(ToggleKey))
        {
            changeToggle = !changeToggle;
            if (changeToggle == true)
            {
                Attach();
            }
            else
            {
                Detach();
            }
        }
    }
}
