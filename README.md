# Kinect Sandbox Driving Sim

A simple driving game made in Unity that generates terrain in real-time using depth stream data from a Kinect. **Exe and build_data folders are both required to be in the same directory before running.**

This project uses Unity 5.5.2f1, the 1414 Kinect model (the original for the XBox 360),the corresponding Kinect SDK 1.7/1.8, and the Unity Wrapper Package (for SDK 1.7). Links for the last two and API documentation for the wrapper package can be found here: http://wiki.etc.cmu.edu/unity3d/index.php/Microsoft_Kinect_-_Microsoft_SDK.

When running the release build provided here, make sure the Kinect is kept on a stable platform at least one meter or so away from the terrain you're capturing. Extending this project to use the v2 Kinect is highly recommended, since its associated SDK's and wrapper packages are still being updated (not to mention the lower noise and higher resolution provided by the new hardware).

#### Build Notes:

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

