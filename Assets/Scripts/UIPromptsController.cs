using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class UIPromptsController : MonoBehaviour
{
    public Transform takedownPrompt;
    public Transform grabPrompt;

    private PlayerController currentPlayer = null;

    private void Update()
    {
        grabPrompt.gameObject.SetActive(SetGrabPrompt());
        takedownPrompt.gameObject.SetActive(SetTakedownPrompt());
    }
    private bool TryGetPlayer()
    {
        if (currentPlayer == null) currentPlayer = GameManager.Instance.CurrentPlayer;
        if (currentPlayer == null) return false;
        return true;
    }
    private bool SetGrabPrompt()
    {
        if (grabPrompt==null) return false;
        if (!TryGetPlayer()) return false;
        if (currentPlayer.targetedGrabObject==null) return false;
        if (currentPlayer.IsGrabbing) return false;
        grabPrompt.position = currentPlayer.targetedGrabObject.transform.position;
        return true;
    }
    private bool SetTakedownPrompt()
    {
        if (takedownPrompt == null) return false;
        if (!TryGetPlayer()) return false;
        if (currentPlayer.isTakedown) return false;
        if (currentPlayer.takedownTarget == null) return false;
        takedownPrompt.position = currentPlayer.takedownTarget.transform.position+Vector3.forward*.25f;
        return true;
    }
}
