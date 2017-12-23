using System.Collections.Generic;
using UnityEngine;

public class StaticLayerChanger : LayerChanger {
    private Transform floor;

    public StaticLayerChanger(Transform floor, LayerMask interactionLayer, Transform min, Transform max, int sensitivity){
        this.floor = floor;
        this.min = min;
        this.max = max;
        this.sensitivity = sensitivity;

        //setup variables
        sprites = new List<SpriteRenderer>();
        characterLayer = Mathf.RoundToInt(Mathf.Log(interactionLayer, 2f));
        GetAllSpritesRenderer();
        //setup fixed values for this scene
        SetupValues();
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
