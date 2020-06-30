using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode{NoiseMap, ColorMap, Mesh};

    const int chunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float mapScale;

    public int octaves;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity; 

    public float heightMultiplier;
    public AnimationCurve meshHeightCurve;

    public int seed;
    public Vector2 offset;
    public bool autoUpdate;

    public DrawMode drawMode;

    public TerrainType[] terrains;

    public void GenerateMap() {
        float[,] noiseMap = Noise.Generate(chunkSize, chunkSize, seed, mapScale, octaves, persistance, lacunarity, offset);
        
        Color[] colorMap = this.ConvertNoiseToRegions(noiseMap);

        MapRenderer mr = FindObjectOfType<MapRenderer>();

        if (drawMode == DrawMode.NoiseMap) {
            mr.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if (drawMode == DrawMode.ColorMap) {
            mr.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, noiseMap.GetLength(0), noiseMap.GetLength(1)));
        } else if (drawMode == DrawMode.Mesh) {
            mr.DrawMesh(MeshGenerator.GenereateTerrainMesh(noiseMap, heightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, chunkSize, chunkSize));
        }
        
    }

    public void OnValidate() {
        if (octaves < 0) {
            octaves = 0;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }

    }

    private Color[] ConvertNoiseToRegions(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        
        Color[] coloredArray = new Color[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int arrayIndex = y * width + x;
                float currentHeight = noiseMap[x, y];
                
                for (int i = 0; i < terrains.Length; i++)
                {
                    if (currentHeight <= terrains[i].height) {
                        coloredArray[arrayIndex] = terrains[i].color;
                        break;
                    }
                }
            }
        }

        return coloredArray;
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}
