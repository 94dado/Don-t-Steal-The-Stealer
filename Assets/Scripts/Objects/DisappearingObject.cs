public class DisappearingObject : InteractableObject {

    //defined empty to NOT execute the father start that is not needed
    public void Start() {

    }

    public override int Interact() {
        Destroy(gameObject);
        return value;
    }
}
