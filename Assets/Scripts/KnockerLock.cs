using System;
using UnityEngine;

public class KnockerLock : Lock {

    public int knockCounter= 5;
    public float knockerRadius = 5;

    private int currentKnockCounter;
    private Transform knockerTransform;

	// Use this for initialization
	void Start () {
        knockerTransform = transform.GetChild(0).transform;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFromInspector();
        UpdateState();
	}

    void UpdateFromInspector() {
        Bounds knobBounds = knockerTransform.GetComponent<SpriteRenderer>().sprite.bounds;

        knockerTransform.localScale = new Vector2(knockerRadius / knobBounds.size.x, knockerRadius / knobBounds.size.y);

    }
    void UpdateState() {
        if (Input.GetMouseButtonDown(0) && knockerIsTouched(getPointerInput())) {
            currentKnockCounter++;
        }

        float t = Mathf.Min(currentKnockCounter, knockCounter) * 1f / knockCounter;
        Vector4 colorVector = new Vector4(Mathf.Lerp(0, 1, 1-t), 0, Mathf.Lerp(0, 1,  t), 1);

        if (knockerIsTouched(getPointerInput())) {
            colorVector += new Vector4(0, 1, 0, 0);
        }

        knockerTransform.GetComponent<SpriteRenderer>().color = colorVector;
    }

    public override bool isLocked() {
        return currentKnockCounter < knockCounter;
    }

    private bool knockerIsTouched(Vector2 pointerLocation) {
        float distance = Vector2.Distance(knockerTransform.position, pointerLocation);
        return distance <= knockerRadius;
    }
}
