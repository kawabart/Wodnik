public interface IGrabbable
{
    bool Grab(HairController hairController);

    void LetGo();

    bool CanBeGrabbed();
}
