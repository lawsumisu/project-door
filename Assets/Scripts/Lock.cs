using UnityEngine;

public abstract class Lock : MonoBehaviour {

    public abstract bool isLocked();

    protected Vector2 getPointerInput() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
