using UnityEngine;

public class StaticLayerChanger : LayerChanger {
    private Transform floor;

    public StaticLayerChanger(Transform floor, LayerMask interactionLayer, Transform min, Transform max, int sensitivity) : base(interactionLayer, min, max, sensitivity) {
        this.floor = floor;
    }

    protected override void GetAllSpritesRenderer() {
        RecursiveGetAllSpritesRenderer(floor);
    }

    void RecursiveGetAllSpritesRenderer(Transform currentItem) {
        for(int i = 0; i < currentItem.childCount; i++) {
            RecursiveGetAllSpritesRenderer(currentItem.GetChild(i));
        }
        SpriteRenderer sprite = currentItem.GetComponent<SpriteRenderer>();
        if (sprite != null) {
            if (sprite.sortingLayerName != "Background" && sprite.sortingLayerName != "Default" && sprite.transform.parent != null && sprite.transform.parent.GetComponent<SpriteRenderer>() == null) {
                sprites.Add(sprite);
            }
        }
    }
}
