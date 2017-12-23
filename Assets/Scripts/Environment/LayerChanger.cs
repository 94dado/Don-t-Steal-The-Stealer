using System.Collections.Generic;
using UnityEngine;

public class LayerChanger {
    //list of all sprites
    protected List<SpriteRenderer> sprites;
    //layer to recognize player/AI
    protected int characterLayer;
    //map dimensions
    protected Transform min, max;
    //values for computing
    protected int sensitivity;
    protected float yMin, yMax, fittingConstant;

    public LayerChanger() {

    }

    public LayerChanger(LayerMask interactionLayer, Transform min, Transform max, int sensitivity) {
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

    //round float to sensitivity decimals
    float RoundFloat(float val, int decimals) {
        return (float)decimal.Round((decimal)val, decimals, System.MidpointRounding.AwayFromZero);
    }

    //setup constant values for this scene
    protected void SetupValues() {
        //get the min and max y values, rounded to sensitivity decimals
        yMin = RoundFloat(min.position.y, sensitivity);
        yMax = RoundFloat(max.position.y, sensitivity);
        fittingConstant = 1f / (yMax - yMin);
    }

    //update values to change floor correctly
    public void ChangeFloor(Transform min, Transform max) {
        this.min = min;
        this.max = max;
        SetupValues();
    }

    //search and saves all sprites renderer in the scene
    virtual protected void GetAllSpritesRenderer() {
        //get all the objects from the scene
        SpriteRenderer[] sprites = GameObject.FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        foreach(SpriteRenderer sprite in sprites){ 
            //if it's a moving object
            if (sprite.gameObject.layer == characterLayer) {
                this.sprites.Add(sprite);
            }
        }
    }

    //update sprites order for every sprite in the list
    public void UpdateOrder() {
        foreach (SpriteRenderer sprite in sprites) {
            int layer = CalculateLayer(sprite);
            sprite.sortingOrder = layer;
            foreach (Transform son in sprite.transform) {
                SpriteRenderer sonRender = son.GetComponent<SpriteRenderer>();
                if (sonRender != null) {
                    sonRender.sortingOrder = layer + 1;
                }
            }
        }
    }

    //using a continous distribution between [ymin,ymax] to calculate the orderInLayer value
    int CalculateLayer(SpriteRenderer sprite) {
        //get the y value of the bottom of the sprite
        Vector2 position = sprite.transform.position;
        float realY = sprite.transform.position.y - (sprite.bounds.size.y / 2f);
        //check if there is an offset for the sprite to use
        SpriteOffset offset = sprite.GetComponent<SpriteOffset>();
        if (offset != null) realY += offset.spriteOffset;
        float actualY = RoundFloat(realY, sensitivity);
        //convert the y value in a scale between [0,1]
        float fit = fittingConstant * (actualY - yMin);
        //invert the fitness value
        fit = 1f - fit;
        //round fit to sensitivity decimal
        fit = RoundFloat(fit, sensitivity);
        //calculate the order in layer removing floating point to fit value
        float orderInLayer = fit * Mathf.Pow(10f, sensitivity);
        return (int)RoundFloat(orderInLayer, 0);
    }


}
