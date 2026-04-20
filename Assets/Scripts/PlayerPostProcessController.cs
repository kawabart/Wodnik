using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerController))]
public class PlayerPostProcessController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField]
    private Volume volumeHidden;
    [SerializeField]
    private Volume volumeHurt;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    public void Update()
    {
        if (volumeHidden != null)
        {
            if (playerController.Hidden) volumeHidden.weight += .1f;
            else volumeHidden.weight *= .9f;
        }
        if (volumeHurt != null)
        {
            volumeHurt.weight = (1 - playerController.Health / playerController.MaxHealth) * .5f;
        }
    }
}
