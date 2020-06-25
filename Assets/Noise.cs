using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Noise
{
    private const float minScale = 0.001f;

    public static float[,] Generate(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) 
    {
        float[,] noiseMap = new float[width,height];

        if (scale <= 0) {
            scale = minScale;
        }

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            octaveOffsets[i] = new Vector2(rng.Next(-100000, 100000) + offset.x, rng.Next(-100000, 100000) + offset.y);
        }


        Vector2 scaleOffset = new Vector2(width / 2f, height / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float coordX = (x - scaleOffset.x) / scale * frequency + octaveOffsets[i].x;
                    float coordY = (y - scaleOffset.y) / scale * frequency + octaveOffsets[i].y;   
    
                    float val = Mathf.PerlinNoise(coordX, coordY) * 2 - 1;
                    noiseHeight += val * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxHeight) {
                    maxHeight = noiseHeight;
                } else if (noiseHeight < minHeight) {
                    minHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;   
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp (minHeight, maxHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}