cTab+
=====
**Commander's Tablet - FBCB2, Blue Force Tracker, UAV, Helmet Cam and Messaging Interface**
*Created by Riouken*

Documentation
- For users, see [Install and Key bindings](docs/EndUser.md)
- For mission makers, see [Mission Makers and Server Settings](docs/MissionMaker.md)
- [Contributors and special thanks](AUTHORS.md)
- [Changelog](CHANGELOG.md)
- [License](LICENSE) : GNU GPL v2

Features
--------
+ Commander's tablet
+ Working FBCB2 Blue Force Tracker system
+ User placed makers - place markers for enemy, medical and general purpose
+ Tracks all crewed Bluefor Military vehicles
+ Tracks any dismounted troops with the proper equipment
+ Android based Blue Force Tracker
+ MicroDAGR hand-held GPS with Blue Force Tracking
+ Commanders Tablet can view live UAV streams
+ Commanders Tablet can view live Helmet Cam streams
+ Vehicle mounted FBCB2 interface, Blue Force Tracking
+ Tactical Awareness Display (TAD) for air vehicles, Blue Force Tracking
+ This system is available to only one side at a time, there is a mission configurable parameter to choose sides
+ None of the markers or icons show on maps, need one of the devices to view
+ Settings for mission makers to adjust features, and make them more realistic
+ cTab on your mobile device with additional [cTab IRL](docs/cTabIRL/README.md) mod

Known Issues
------------
+ Switching to or from the full screen views while in a vehicle can cause issues (fixed by exiting vehicle)
  [*BIS issue with command, please vote for a fix*](http://feedback.arma3.com/view.php?id=11577)
+ If your are viewing yourself from the UAV or Helmet Cam in PiP screen, your textures can bug on your unit
+ Even though items go into the GPS slot they are not required to be there for cTab to operate, they can go anywhere in your inventory, i.e. your vest or uniform
+ Players that are experiencing conflicts with help screens (uses `H`as a key as well) are advised to rebind cTab `IF_MAIN`, for example to `SHIFT`+ `H`. This used to be an issue with Zeus but has been resolved as of cTab version 2.1. There might be other such cases though.
+ Helicopter pilots (and co-pilots) that are using RAVEN's LIFTER mod are advised to rebind cTab `IF_MAIN` to something other than the default as `H` is used by that mod and cannot be changed (as of this writing).
+ When a UAV is being actively piloted and a cTab user is already connected to the UAV's gunner turret, it is currently impossible to detect that there is a gunner connected in order to prevent a second player to connect to the same gunner turret. Wonky things happen to the player in the gunner seat if a second player connects and it might break the game. [Please vote for a fix](http://feedback.arma3.com/view.php?id=23693).

Required
--------
+ [CBA_A3 3.15 or later](https://github.com/CBATeam/CBA_A3/releases/latest), also available on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=450814997)

Optional
--------
+ [ACE3](http://ace3mod.com/), also available on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=463939057)

Media
-----
[![Tablet BFT](http://i.imgur.com/HnHLiv7m.jpg)](http://i.imgur.com/HnHLiv7.jpg)
[![Tablet UAV night](http://i.imgur.com/ehvx1tFm.jpg)](http://i.imgur.com/ehvx1tF.jpg)
[![3D models](http://i.imgur.com/Niynrvmm.jpg)](http://i.imgur.com/Niynrvm.jpg)
[![Vehicle FBCB2](http://i.imgur.com/bjarZTqm.jpg)](http://i.imgur.com/bjarZTq.jpg)
[![TAD small](http://i.imgur.com/ngtjm2Dm.jpg)](http://i.imgur.com/ngtjm2D.jpg)
[![TAD large night](http://i.imgur.com/OVyYrkpm.jpg)](http://i.imgur.com/OVyYrkp.jpg)
[![Android small](http://i.imgur.com/0lOIuvem.jpg)](http://i.imgur.com/0lOIuve.jpg)
[![Android large night](http://i.imgur.com/aaPccktm.jpg)](http://i.imgur.com/aaPcckt.jpg)
[![MicroDAGR small](http://i.imgur.com/ZI6XZznm.jpg)](http://i.imgur.com/ZI6XZzn.jpg)
[![MicroDAGR large](http://i.imgur.com/aE3zcxjm.jpg)](http://i.imgur.com/aE3zcxj.jpg)
[![cTab 1.0 release overview](http://img.youtube.com/vi/2fFSOej_GPk/0.jpg)](http://youtu.be/2fFSOej_GPk)

Download / Links
----------------
New version :
* [cTab+ on Steam Workshop](https://steamcommunity.com/workshop/filedetails/?id=2262006564)
* [cTab IRL on Steam Workshop](https://steamcommunity.com/workshop/filedetails/?id=2262009445)

Previous versions from Riouken
* [Armaholic](http://www.armaholic.com/page.php?id=22992)
* [ArmA World](http://armaworld.de/threads/156-cTab-Commander-s-Tablet)
* [withSix](http://play.withsix.com/Arma-3/mods/4KfaixFS4xGnygAVF72WTA)
* [BI Forum](http://forums.bistudio.com/showthread.php?166488)
* [GitHub](https://github.com/Riouken/cTab)
