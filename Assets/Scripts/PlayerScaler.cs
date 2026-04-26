using UnityEngine;

public class PlayerScaler : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField]
    private Vector3 hiddenScale;
    [SerializeField]
    private PlayerController playerController;
    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (playerController == null) return;
        if (playerController.Hidden && !playerController.isTakedown) transform.localScale = hiddenScale;
        else transform.localScale = originalScale;
    }
}
