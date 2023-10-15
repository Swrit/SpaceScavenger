using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    [Serializable]
    private struct ParallaxLayer
    {
        public Transform spriteObject;
        public Vector2 textureUnitSize;
        public Vector2 passiveScroll;
    }

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private List<ParallaxLayer> layers;


    private void Awake()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        for (int i = 0; i < layers.Count; i++)
        {
            ParallaxLayer modifiedLayer = layers[i];
            modifiedLayer.textureUnitSize = Calculate(layers[i].spriteObject);
            layers[i] = modifiedLayer;
        }
    }

    private void Update()
    {
        foreach (ParallaxLayer pl in layers)
        {
            ProcessMovement(pl);
        }

    }

    public Vector2 Calculate(Transform spriteObject)
    {
        Sprite sprite = spriteObject.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        return new Vector2(texture.width / sprite.pixelsPerUnit, texture.height / sprite.pixelsPerUnit);
    }

    private void ProcessMovement(ParallaxLayer layer)
    {
        Vector2 move = layer.passiveScroll * Time.deltaTime;

        layer.spriteObject.position += new Vector3(move.x, move.y, 0f);

        if (Mathf.Abs(cameraTransform.position.x - layer.spriteObject.position.x) >= layer.textureUnitSize.x)
        {
            float offset = (cameraTransform.position.x - layer.spriteObject.position.x) % layer.textureUnitSize.x;
            layer.spriteObject.position = new Vector3(cameraTransform.position.x + offset, layer.spriteObject.position.y, layer.spriteObject.position.z);
        }

        if (Mathf.Abs(cameraTransform.position.y - layer.spriteObject.position.y) >= layer.textureUnitSize.y)
        {
            float offset = (cameraTransform.position.y - layer.spriteObject.position.y) % layer.textureUnitSize.y;
            layer.spriteObject.position = new Vector3(layer.spriteObject.position.x, cameraTransform.position.y + offset, layer.spriteObject.position.z);
        }




    }
}
