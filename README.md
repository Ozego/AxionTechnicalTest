# AxionTechnicalTest
Technical Artist Test for True Axion game studio
Thank you for a challenging test.
I attemped to do my best at doing all 6 tasks within the time allocated. 
 ![My image](https://raw.githubusercontent.com/Ozego/AxionTechnicalTest/master/ScreenShots/Unity_0P5AyefrOW.png)
1. I created Assets\Scripts\Animation\ArrayAnimator.cs which allows a variety of ways to animate an array of animation asyncronous.
2. Every data in this shader was encoded as float4. I set the correct datatypes for effecient use of video card memory. See code notation for details. There was also some garbage which calculates 1 through a ineffecient function I edited out. Basic cleaning of code.
3. Added shadow pass at Line 14. I also noticed some misbehaving fog.
4. Tools\Ozego\MaterialAssigner
    Search for gameobject name to replace material tool.
    I advice against this method; it is adviced to manage visuals with prefab variations. I included realtime reflection example since it requires reflection probe. Trail renderer is also needed for effect in task 5.
5. Assets/Prefabs/ImpactFX.prefab
    Blast effect in use in test scene.
6. Assets/Prefabs/Walls.prefab
    Simple spline walls.
    I based my design on baking a skinned mesh following a spline to a static mesh. Easy for fast walls. Wall rig can be found in blender files.

https://github.com/Ozego/AxionTechnicalTest