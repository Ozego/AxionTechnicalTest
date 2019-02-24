# AxionTechnicalTest
Technical Artist Test for True Axion game studio
Thank you for a challenging test.
I attemped to do my best at doing all 6 tasks within the time allocated. 
 ![My image](https://raw.githubusercontent.com/Ozego/AxionTechnicalTest/master/ScreenShots/Unity_0P5AyefrOW.png)
1. There are several solutions for this depending on desired behaviour inside the game.
I created Assets\Scripts\Animation\ArrayAnimator.cs which allows a variety of ways to animate an array of animation asyncronous.
2. Every data in this shader is encoded as float4, this is not effecient at all in video memory. See code notation for details. there is also some garbage which calculates 1 through a ineffecient function, quite annoying.
3. Line 14. I also noticed some misbehaving fog.
4. Assets\Scripts\Editor\MaterialAssigner.cs
    Tools\Ozego\MaterialAssigner
    I advice against this method, it is better to manage it with prefab variations. I included realtime reflection example since it needs reflection probe. I also needed a trail renderer in the SFX balls for later assignment.
    But tool works if you want. Assigning sfx setup through tool is also possibility if desired.
5. Assets/Prefabs/ImpactFX.prefab
6. Assets/Prefabs/Walls.prefab
    I based my design around simply skinned mesh to spline baking. Easy for fast walls. I modelled the rig in blender.

https://github.com/Ozego/AxionTechnicalTest