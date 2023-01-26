# Global Outline
Global Outline - Unity (3D/UI/Sprites) full-screen masked outline shader

![logo](https://raw.githubusercontent.com/rickomax/globaloutline/master/screen.png)

This repository contains scripts and shaders used to add a full-screen outline effect to visual Unity components (Graphic or Renderer). 

To use it, add the "OutlineManager.cs" script to the camera that will render the outline effect and call the "AddGameObject" method to add the effect to any GameObject that contains Graphics or Renderers. You can configure the effect size, color, and smoothness on the script properties.

You can also add the "OutlineTests.cs" script to an empty GameObject and fill the "ObjectsToApply" array with the objects you want to apply the effect to.

Known issues:<br>
* Multiple outline effects aren't possible at the moment
