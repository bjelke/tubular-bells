# 3D Virtual Chimes
### by Kayla Beckham & Brighten Jelke
Inspired by the IEEE 3DUI contest from 2015, this project recreates orchestral chimes, also known as tubular bells, 
in virtual reality using Unity, SteamVR, and the HTC Vive (headset and two standard controllers).
## How to Build and Run

## How to Play
![Image of HTV Vive controller buttons]
(./images/vive_controllers.jpg)
This is a diagram of all the buttons on a Vive controller, as seen in the [Unity Manual](https://docs.unity3d.com/Manual/OpenVRControllers.html). Numbers below will correspond to buttons on this diagram to indicate the location of buttons on the controller.

When the program begins, one of the controllers should be rendered as a hammer, and the other should look like a Vive controller. The trigger button on the back of the controller (7) that is rendered as a controller can toggle that hand to be a hammer or back to a controller.

When the hand is a controller, you can use it to group chimes together to play chords. This is also the hand that can turn the particle visualizations on and off and do sound dampening.

Hit the chimes with the head of your hammer(s) to play a note. Each chime plays a different note, but the sound is not affected by where on the chime you hit, unless you hit the chime on an activated group marker. This will play all the chimes that have the same group marker.

When you have a controller, you can touch a chime with the controller and use the trackpad on the front of the controller (2) to assign a group to the chime. You know you are touching the controller because it appears transparent with a pink outline. If you click a section of the trackpad that is orange, green, or blue, the chime will be added to the group of that color and a marker will appear on the chime. Hit the marker with the head of your hammer(s) to play every chime in that group.

Use the small round menu button at the top of the controller (1) to toggle the visual particle response on or off.

Use the side grip buttons (8) on the same controller to dampen sound.

You can toggle the particle effects and dampen the sound whether the controller appears as a controller or a hammer.

## Overall Architecture
This project was developed in Unity, so a lot of the structure was determined by the editor. The TubularBells scene has our  game objects and scripts acting together to run during play.
## Important Performance Issues
Poor tracking the headset and controllers in the corners of the cave / HTC camera range can cause disorientation and dizziness in some users.

Dampening the sound of the chimes is currently not implemented to our satisfaction. When you press the dampening button, volume is turned down for the duration that you hold the button. It would be more realistic to make the sounds playing at the time of dampening quieter for the rest of the time that they are playing, or to stop them playing altogether by dampening them for long enough. Additionally, we would be happier if dampening could be mapped to a gesture instead of a button press, or if there was a way to dampen one chime at a time.
## Known Bugs
The capacity of particle effects is currently set to 500. It is difficult, but possible, to hit the capacity. No particle 
effects will be rendered for a few seconds until some of them go out of range.

We wrote the code with the intention of taking in button input to dampen sound and toggle the visual particle effects from both controllers. The SteamVR input handler recieves the signal from button presses on both controllers, but because the script that implements the effects is only attached to the "Controller (left)" game object, only that controller can execute those changes in practice.
## Results
