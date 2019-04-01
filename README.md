# Global Outline
Global Outline - Unity (3D/UI/Sprites) full-screen masked outline shader

![logo](https://raw.githubusercontent.com/rickomax/globaloutline/master/screen.png)

This repository contains scripts and shaders used to add a full-screen outline effect to visual Unity components (Graphic or Renderer). 

To use it, add the "OutlineManager.cs" script to the camera that will render the outline effect and call the "AddGameObject" method to add the effect to any GameObject that contains Graphics or Renderers. You can configure the effect size, color, and smoothness on the script properties.

You can also add the "OutlineTests.cs" script to an empty GameObject and fill the "ObjectsToApply" array with the objects you want to apply the effect to.

Known issues:<br>
* Multiple outline effects aren't possible at the moment

License (BSD License 2.0):
Copyright (c) 2019, Ricardo Reis
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
* Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright
notice, this list of conditions and the following disclaimer in the
documentation and/or other materials provided with the distribution.
* Neither the name of the nor the
names of its contributors may be used to endorse or promote products
derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
