public interface IGrabbable
{
    bool Grab(HairController hairController);

    void LetGo(HairController hairController);

    bool CanBeGrabbed();
}
