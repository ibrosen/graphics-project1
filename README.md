# graphics-project1
Due 4pm, Monday 4th September

Modelling of fractal landscape (3 marks):
  – You must automatically generate a randomly seeded fractal landscape at each invocation of the program, via a correct implementation of the diamond-square algorithm.
  – You must use Unity’s architecture appropriately to generate and render the landscape.
  – There must be no notable problems or artifacts with the polygonal representation.

Camera motion (3 marks):
  – Your camera controlsshould be implemented in a ‘flight simulator’ style, with the following specifications:
      ∗ Moving the mouse should control the relative pitch and yaw of the camera
      ∗ The ’w’ and ’s’ keys should cause the camera to move forwards and backwards respectively, relative the camera’s current orientation
      ∗ The ’a’ and ’d’ keys should cause the camera to move left and right respectively, relative to the camera’s current orientation
      ∗ The ’q’ and ’e’ keys should control the roll of the camera
  – You must allow the user to move anywhere in the world (including up into the sky), and prohibit the user from moving “underground” or outside the bounds of the landscape.
  – The camera must not become ‘stuck’ upon nearing or impacting the terrain, i.e. reversing and continuing to move must always be possible.
  – You must utilise perspective projection, and choose a suitable default perspective, so that the landscape is clearly visible from the start.

Surface properties (4 marks):
  – The colour of the terrain must correspond in a sensible way with the height of the terrain at any particular point (for example rocky outcrops or snow on top of mountains and grass or soil in valleys).
  – Suitable lighting must be present based on the Phong illumination model (diffuse, specular and ambient components). You should use a custom Cg/HLSL shader for this.
  – The direction of the lighting must change with time, to simulate the effect of a sun rising
and setting.
  – The sun itself must also be drawn, in order to help verify the correctness of your lighting implementation. You may use any simple geometric shape such as a pyramid, cube or sphere to represent the sun.
  – A constant and reasonable frame refresh rate must be maintained during program execution (i.e., 30 frames per second or more)
