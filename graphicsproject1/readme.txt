RESOURCES

Information on how to implement the diamond square algorithm was sourced from the following video: https://www.youtube.com/watch?v=1HV8GbFnCik. As a result, the DiamondSquareTerrain and MeshGenerator classes have similarities.


TERRAIN GENERATION - DIAMOND SQUARE ALGORITHM
The diamond square algorithm works by creating a plane with initially 4 vertices. A number of diamond and square steps are performed to ultimately transform the flat plane into a natural looking landscape with mountains and valleys. The number of diamond and square steps is determined by the specified number of divisions.

Diamond step:
In the diamond step, the height of the midpoint for each square is set to be the average of its corner’s heights, plus a random number. Adding this midpoint to the square creates 4 diamonds. Note that this is the first step of the algorithm because the plane is initially a single square.

Square step:
Next, is the square step, in this step, the height of the midpoint of each diamond is set to be the average of its vertices, plus a random number.

The magnitude of the random number is lessened on each subsequent step, to keep the potential rise or fall of the midpoint proportional to the size of the square/diamond its in.


TERRAIN COLOURING
After terrain generation, the height of each vertex is used to colour the terrain. Colour boundaries are set to specific heights so that mountain peaks, grass, sand and water can be logically coloured. These boundaries depend on the minimum and maximum height values of the terrain. Some terrain areas are also linearly interpolated to make landscape colouring smoother.


LIGHTING
A Gouraud shader is attached to the terrain and the water. Note that the water version of the shader contains specular light whilst the terrain version does not. The code for these shaders were slightly altered from the ones used in tutorials for the subject. The Gouraud shader uses the Phong illumination model to calculate the illumination (combination of ambient, specular and diffuse) on each vertex of the landscape and interpolates the illumination over each polygon.


CAMERA MOVEMENT
The camera uses continuous (a posteriori) collision detection. On collision, the camera isn’t allowed to pass through the terrain as we attached a mesh collider and rigid body to it. To stop unintended interactions after collision (the camera would freely spin and move), the angular velocity and velocity is set to 0, as the camera doesn’t need to move outside of user input.