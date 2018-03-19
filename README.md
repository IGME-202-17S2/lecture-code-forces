# lecture-code-forces

This is not an exercise or anything that is due for a grade. This is just some code to discuss during class together!

You see before you a scene with three spheres. They are instances of the PhysicsSphere prefab, which has a PhysicsObject script attached to it.

If you run the document, you'll see an additional 3 spheres appear. All of the spheres fall and bounce. They fall at the same rate (because gravity works that way), but they'll be blown to the right at varying rates, because they have varying masses.

In Unity, check the hierarchy for the MainScripts game object. It has an ObjectManager script attached to it. Since I saw a lot of folks had trouble managing their Bullets/Asteroids collections on Project 2, I'm modelling two different approaches in this script.

One approach exposes a public List<PhysicsObject> scenePO … and uses the inspector to drag and drop the Light/Middle/HeavySphere instances to be part of that list.

The other approach has a private List<PhysicsObject> generatedPO … and uses Instantiate to create and populate that list.

Things to Try
Change the strength / direction of the forces.
Add another force.
Remove gravity.
Add another PhysicsSphere through the hierarchy.
Add another PhysicsSphere through the GeneratePOs() method.
Tweak the value of mu for friction.
Tweak the elasticity for bouncing.
