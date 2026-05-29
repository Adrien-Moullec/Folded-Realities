/// <summary>
/// Base script for entities or objects that can be interacted with.
/// </summary>
public interface IInteractable {
    public void OnInteract();
    public void OnCancelInteract();
}