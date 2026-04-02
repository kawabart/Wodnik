using UnityEngine;
using UnityEngine.Rendering;

public class PlayerPostProcessController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField]
    private Volume volumeHidden;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    public void Update()
    {
        if (!volumeHidden) return;

        if (playerController.Hidden) volumeHidden.weight += .1f;
        else volumeHidden.weight *= .9f;
    }
}
