# Kinect Sandbox Driving Sim

A simple driving game made in Unity that generates terrain in real-time using depth stream data from a Kinect.

## Getting started:

1. **What you need**
  + [Unity 5.5.2f1](https://unity3d.com/get-unity/download)
  + 1414 model v1 Kinect (the original for the XBox 360)
  + Kinect SDK 1.7/1.8, and the Unity Wrapper Package (for SDK 1.7). Links and API documentation for the wrapper package can be found [here](http://wiki.etc.cmu.edu/unity3d/index.php/Microsoft_Kinect_-_Microsoft_SDK). As a side note, the DepthWrapper class grabs the depth data in millimeters with a resolution of 320x240. Accuracy tends to be limited to about 3-5mm.
  + An IDE for working in C#, I use [Visual Studio](https://www.visualstudio.com/vs/).
  + Some material that can be used to generate a terrain, tarp or sand generally work pretty well.The v1 Kinect has an effective FOV of 60 degrees, and should be placed at least a meter or so above the material for best results. This information can be used to calculate your material dimensions using some basic trigonometry.
  
2. **How to run**
  + If you want to test out the release build directly, just download the release zip and extract the exe and build_data folder to the same directory. When running the release build, make sure the Kinect is kept on a stable platform at least one meter or so away from the terrain you're capturing. Extending this project to use the v2 Kinect is highly recommended, since its associated SDK's and wrapper packages are still being updated (not to mention the lower noise and higher resolution provided by the new hardware).
  + If you want to recreate the sandbox environment in Unity, create a new Unity project and add the files/folders provided here to the project directory (overwrite if necessary). The terrain can be replicated by simply creating a new Terrain object, and applying the modifyTerrain.cs script to it. A basic car can also be made by modifying the included prefab, and applying the Steering.cs and MouseLook.cs scripts. Feel free to play around with the parameters, and remember to set the appropriate instances for each of the scripts!

3. **Known bugs**
  + Occasionally, when working with SDK 1.8 in Unity, the Kinect streams will fail to initialize until the Kinect is unplugged and plugged back in.
  + Any vehicles traveling on the terrain will tend to clip through it if the surrounding heights change too suddenly. This usually happens when the terrain is reshaped too fast/close to the vehicle's position. A possible solution would be to overlay the top down view of the ingame sandbox on top of the actual sandbox via a projector.

## Build Notes:

![alt tag](http://i.imgur.com/KaYHZWN.jpg)
Building a 2ft x 2ft wooden box.



![alt tag](http://i.imgur.com/c7veHXs.jpg)
Doing some initial testing with the Kinect Toolkit to determine the ideal height for the Kinect.



![alt tag](http://i.imgur.com/HfrRFr8.png)
Initial testing with modifyTerrain.cs featuring yours truly.



![alt tag](http://i.imgur.com/4QwfxBJ.jpg)
Testing various parameters for smoothing filters to deal with noise, and finding acceptable height ranges for a decent driving experience.



![alt tag](http://i.imgur.com/JboODbO.jpg)
Getting fairly close to the final build. modifyTerrain.cs now uses a temporal and regular smoothing filter, used on the depth stream to update the terrain once every second. By sacrificing update speed for flatter/stabler terrain, we get a better overall driving experience.

Thanks to Sasank Madineri for providing the steering script.
