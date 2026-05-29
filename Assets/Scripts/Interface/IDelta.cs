/// <summary>
/// Old interface for objects that go through an animation with delta 0->1
/// </summary>
public interface IDelta {
    public void StartDelta();
    public void UpdateDelta(float delta);
    public void EndDelta();
}
