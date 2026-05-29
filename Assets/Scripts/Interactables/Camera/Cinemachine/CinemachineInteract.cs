
/// <summary>
/// Allow interaction with objects to set cinemachine activation.
/// </summary>
public abstract class CinemachineInteract : CinemachineOrigami, IInteractable {
    /// <summary>
    /// On cancel interact with cinemachine object.
    /// </summary>
    public abstract void OnCancelInteract();
    /// <summary>
    /// On interact with cinemachine object.
    /// </summary>
    public abstract void OnInteract();
}