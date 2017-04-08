using System.Collections;
using UnityEngine;

public class ModifyTerrain : MonoBehaviour
{
    public DepthWrapper dw;
    public Terrain terr;
    public float[,] heights;
    public int smoothIterations = 1;
    public float smoothBlend = 1.0f;
    public int Tw;
    public int Th;
    public int neighbourhood = 0;
    public int count = 0;
    public float[,] tempHeights;
    public float factor;

    public float numSec = 1.0F;
    public float startTime;

    // Use this for initialization
    void Start()
    {
        terr = Terrain.activeTerrain;
        terr.terrainData.SetDetailResolution(1024, 8);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (dw.pollDepth())
        {
            Tw = terr.terrainData.heightmapWidth;
            Th = terr.terrainData.heightmapHeight;
            heights = terr.terrainData.GetHeights(0, 0, Tw, Th);

            //normalize, then extreme noise filter
            for (int ii = 0; ii < 320 * 240; ii++)
            {
                //get x and y coords
                int x = ii % 320;
                int y = ii / 320;

                float b = dw.depthImg[ii];
                b = b / 7800;
                b = 1 - b;
                if (b > 0.7F)
                {
                    heights[y, x] = 0;
                }
                else
                {
                    heights[y, x] = b;
                }
            }
            
            //temporal filter, and set scene to modify terrain one every second
            if (Time.time - startTime < numSec)
            {
                if (count == 0)
                {
                    tempHeights = (float[,])heights.Clone();
                }
                else
                {
                    for (int Ty = 0; Ty < Th; Ty++)
                    {
                        for (int Tx = 0; Tx < Tw; Tx++)
                        {
                            tempHeights[Tx, Ty] += heights[Tx, Ty];
                        }
                    }
                }
                count++;
            }
            else
            {
                factor = 1.0F / count;
                for (int Ty = 0; Ty < Th; Ty++)
                {
                    for (int Tx = 0; Tx < Tw; Tx++)
                    {
                        tempHeights[Tx, Ty] *= factor;
                    }
                }
                tempHeights = smooth(tempHeights, new Vector2(Tw, Th)); //smoothing filter applied
                terr.terrainData.SetHeights(0, 0, tempHeights); //set the terrain to display the updated heightmap
                count = 0;
                startTime = Time.time;
            }
        }
    }

    //smoothing filter
    private float[,] smooth(float[,] heightMap, Vector2 arraySize)
    {
        int Tw = (int)arraySize.x;
        int Th = (int)arraySize.y;
        int xNeighbours;
        int yNeighbours;
        int xShift;
        int yShift;
        int xIndex;
        int yIndex;
        int Tx;
        int Ty;
        // Start iterations...
        for (int iter = 0; iter < smoothIterations; iter++)
        {
            for (Ty = 0; Ty < Th; Ty++)
            {
                // y...
                if (Ty == 0)
                {
                    yNeighbours = 2;
                    yShift = 0;
                    yIndex = 0;
                }
                else if (Ty == Th - 1)
                {
                    yNeighbours = 2;
                    yShift = -1;
                    yIndex = 1;
                }
                else
                {
                    yNeighbours = 3;
                    yShift = -1;
                    yIndex = 1;
                }
                for (Tx = 0; Tx < Tw; Tx++)
                {
                    // x...
                    if (Tx == 0)
                    {
                        xNeighbours = 2;
                        xShift = 0;
                        xIndex = 0;
                    }
                    else if (Tx == Tw - 1)
                    {
                        xNeighbours = 2;
                        xShift = -1;
                        xIndex = 1;
                    }
                    else
                    {
                        xNeighbours = 3;
                        xShift = -1;
                        xIndex = 1;
                    }
                    int Ny;
                    int Nx;
                    float hCumulative = 0.0f;
                    int nNeighbours = 0;
                    for (Ny = 0; Ny < yNeighbours; Ny++)
                    {
                        for (Nx = 0; Nx < xNeighbours; Nx++)
                        {
                            float heightAtPoint = heightMap[Tx + Nx + xShift, Ty + Ny + yShift]; // Get height at point
                            hCumulative += heightAtPoint;
                            nNeighbours++;
                        }
                    }
                    float hAverage = hCumulative / nNeighbours;
                    heightMap[Tx + xIndex + xShift, Ty + yIndex + yShift] = hAverage;
                }
            }
        }
        return heightMap;
    }
}
