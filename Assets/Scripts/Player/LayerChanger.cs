using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LayerChanger : MonoBehaviour {

    //sprites attributes
    private List<SpriteRenderer> fixedSprites, characterSprites;
    public LayerMask interactionLayer;
    int characterLayer;
    //map dimensions
    public Transform min;
    public Transform max;
    //values for computing
    public int sensitivity;
    float yMin, yMax, fittingConstant;

    void Awake() {
        fixedSprites = new List<SpriteRenderer>();
        characterSprites = new List<SpriteRenderer>();
        characterLayer = Mathf.RoundToInt(Mathf.Log(interactionLayer,2f));
        GetAllSpritesRenderer();
        //setup fixed values for this scene
        SetupValues();
        //set the correct order in layer for static objects
        UpdateOrder(fixedSprites);
    }

    // Update is called once per frame
    void Update () {
        //update order in layer only for characters
        UpdateOrder(characterSprites);
	}

    //round float to sensitivity decimals
    float RoundFloat(float val, int decimals) {
        return (float)decimal.Round((decimal)val, decimals, System.MidpointRounding.AwayFromZero);
    }

    //setup constant values for this scene
    void SetupValues() {
        //get the min and max y values, rounded to sensitivity decimals
        yMin = RoundFloat(min.position.y, sensitivity);
        yMax = RoundFloat(max.position.y, sensitivity);
        fittingConstant = 1f / (yMax - yMin);
    }

    //search and saves all sprites renderer in the scene
    void GetAllSpritesRenderer() {
        //get all the objects from the scene
        SpriteRenderer[] sprites = FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        foreach(SpriteRenderer sprite in sprites) {
            //if it's a moving object
            if (sprite.gameObject.layer == characterLayer) {
                characterSprites.Add(sprite);
            }
            //it's a fixed object, but not background
            else if (sprite.sortingLayerName != "Background" && sprite.transform.parent.GetComponent<SpriteRenderer>() == null) {
                fixedSprites.Add(sprite);
            }
        }
    }

    //update sprites order for every sprite in the list
    void UpdateOrder(List<SpriteRenderer> sprites) {
        foreach (SpriteRenderer sprite in sprites) {
            int layer = CalculateLayer(sprite);
            sprite.sortingOrder = layer;
            foreach(Transform son in sprite.transform) {
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
        //Debug.Log(fit);
        //round fit to sensitivity decimal
        fit = RoundFloat(fit, sensitivity);
        //calculate the order in layer removing floating point to fit value
        float orderInLayer = fit * Mathf.Pow(10f, sensitivity);
        return (int)RoundFloat(orderInLayer, 0);
    }
}
