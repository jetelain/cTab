Changelog
---------
### 2.7.0 ###
* Added vehicle marker compass
  * In supported vehicles you have an horizontal compass displayed a the screen bottom
  * Enemy markers are displayed on the compass at the appropriate heading with distance shown in meters
* Added rangefinder integration
  * You can use ACE Vector, or game rangefinder, or vehicle rangefinder to get coordinates for a marker
  * First use the rangefinder, then open the tablet or android device, marker menu will appear at the coordinates 
  * Then select marker informations 
  * Compatible with cTab IRL : The menu immediatly pops up on your real life tablet or smartphone
* Added new markers
  * Coordination markers have been added
  * Additions to "Enemy" menu
    * Mine
    * IED
  * Additions to "General" menu
    * Dot marker with label (A to F)
    * Circle marker with label (A to F)
  * New menu "Control point"
    * Unspecified Control Point
    * Contact Point
    * Coordinating Point
    * CKP - Checkpoint
    * SOS - Distress call
    * EC - Entry Control
    * RLY - Rally Point
    * SP - Start Point
  * New menu "Manoeuvre"
    * Outpost
    * Combat outpost
    * Target reference Point
    * PD - Point of Departure
  * New menu "Sustainment"
    * CCP - Casualty Collection point
    * DET - Detainee Collection point
    * MED - MEDEVAC Pick-Up Point
    * R3P - Rearm, Refuel and Resupply Point
* Added cTab items as basic items
  * Tablet and Android device are also available as misc items
  * It will avoid unwanted in-game GPS related features
  * Original items have been kept for backward compatibility, you may blacklist them in Arsenal
* Updated user interface of rugged tablet
  * Interface style have been updated to Windows 10
* Updated Android device to Samsung S7 with Kägwerks case
* Updated user interface of marker menu
  * Menu includes images to easily pickup correct marker
  * Direction marker layout have been change to look as a compass
* Improved performances
  * Most used functions are 50% faster
* (cTab IRL) Added a Troop In Contact alert button
  * Creates immediatly a special marker your position
* Fixed Arma 2.10 compatibility

### 2.6.0 ###
* (cTab IRL) Electronic flight instrument system
  * Displays real-time instruments on your real life tablet to improve helicopter pilots immersion
* (cTab IRL) Improved performances
  * Enforce WebSockets to reduce server load and reduce latency

### 2.5.0 ###
* Added settings (in CBA Mods Settings)
  * Blue Force Tracking : Disabled, Real time (default) or at each synchronisation
  * Synchronisation rate : 30 seconds by default
  * Helmet Cam : Disabled or Enabled (default)
  * UAV feeds : Disabled or Enabled (default)
  * Bearing/heading : Mils or Degrees (default)
  * Default map mode : Topographic or Satellite (default)
* Added ACE integration
  * cTab devices are available from self-interaction menu (Ctrl + Windows by defaut)
  * ACE MicroDAGR as Blue Force Tracking device (can be disabled). It provides a great replacement for cTab MicroDAGR as it provides more features.
  * Change group name/callsign in game
* Fixes from Bamse andspyderblack723
* Fixed key bindings list in journal CBA entry
* Removed key bindings from ctab_settings.hpp : You now have to use CBA settings to setup key bindings.
* Added optional [cTab IRL mod](docs/cTabIRL/README.md) : cTab on your mobile device
  * Real time position and heading
  * Blue Force Tracking
  * User marker / Red Force Tracking
  * Messaging
* Fix helmet cam conflict with mine detector
* New maintener : jetelain aka GrueArbre

### 2.2.2 ###
* Fixed message composition and display elements having a black background after ArmA3 1.46 update

### 2.2.1 ###
* Added variable message decay to notification system (new-marker notifications are now shown for longer)
* Fixed error regarding the helmet cam icon when looking at a cTab-box placed with Zeus
* Fixed issue with notification system upon marker creation
* Marker menu will now stay inside the device screen boundaries
* Moved marker menu above brightness layer to allow it to show tool-tips properly
* Tweaked marker menu size on Android

### 2.2.0 ###
* Added ability to increase / decrease screen brightness of TAD (via BRT+/- rocker on lower right)
* Added ability to message oneself to take notes
* Added ability to switch interface position of TAD, Android and MicroDAGR in overlay mode from left to right default `CTRL + SHIFT + HOME`)
* Added new and improved tablet interface background and 3D model, both made by Raspu
* Added night mode to Tablet, Android and MicroDAGR (switches automatically based on light conditions outside), graphics mad by Raspu
* Added night mode to TAD (switched via the DAY/NIGHT switch on the lower left), graphics mad by Raspu
* Added on-device notifications that will appear when for example a message has been received or a new marker was added
* Changed the way names are displayed in messaging to be more in line with Helmet Cam screen
* Changed to brighter MicroDAGR interface background, made by Raspu
* Changed to reworked Android and MicroDAGR 3D models, both made by Raspu
* Fixed icons in list of messages being black
* Fixed marker text alignment (some recent ArmA update changed the default from right to center)
* General performance improvements
* Immediately stop showing team-members when player is leaving the group. Note: When joining a new group it will take up to 30 seconds for any cTab carrying group members to appear
* Interactive mode (large interfaces) screen position will now be remembered and restored on load (can be reset to screen center using default `CTRL + SHIFT + HOME`)
* Interface will now be closed if player is unconscious and ACE3 Medical is used
* Interface will now be closed if player lost the required device and ACE3 is used
* Minor interface tweaks
* No longer closes small MicroDAGR or Android interfaces when exeting a vehicle
* On-foot team members that have been assigned to a fire-team will now be coloured accordingly. If no fire team has been set, they will show up as the standard blue (slightly different colour than the blue fire team).
* Reclassified helmet cam item to show up as a generic item in Arsenal. You will find it by for example selecting your vest and selecting the `+` icon on the right side of the screen
* Speedup of interface startup
* Updated all device iventory icons to match, made by Raspu

### 2.1.1 ###
* Support for updated CBA keybinding API (introduced with CBA 1.20 RC6)
* Prevent TAD from being accessible when using a parachute
* Added 500m zoom-level to small TAD
* Fixed player's camera breaking when exiting UAV full screen view while in a vehicle
* Fixed control of new target designator turrets (introduced with marksman DLC) from the Tablet's UAV screen
* Immediately terminate UAV cameras if UAV is distroyed
* Added own helmet cam back into the list of accessible helmet cams (to reduce confusion)
* Sorted helmet cams by group ID
* Made area around the map gray instead of black to increas readability of off-map markers / units
* Made TAD map tools follow mouse cursor instead of map center. This also allows for measuring distances to off-map destinations.
* Discrepancies between cTab client and server versions will now be reported to RPT on both client and server via CBA versioning
* Available UAVs / helmet cams are now automatically refreshed on tablet whenever the lists have changed (lists are updated every 30 seconds), eliminating the need to switch modes or close and reopen the tablet for the display to refresh
* Added UAV type to list of available UAVs to help with orientation
* Re-Categorized helmet cam item to show up as face-wear (goggles) in Arsenal. Note: It can still be moved anywhere else in the inventory without losing its capability
* Fixed keybinds not working in Zeus and causing RPT errors after update to CBA 1.20 RC6

### 2.1.0 ###
* Added basic TvT support, so now cTab will work on any side out of the box. Note: A stolen enemy device will currently _not_ provide you with enemy intel, instead the device will inherit your encryption key.
* Provided encryption keys that can be set by the mission designer to allow or disallow cTab data to be shared.
* Improved Zeus support when remote controlling AI
* cTab can now be operated as Zeus even with default keybinding `H`
* Added MicroDAGR hand-held GPS.
  It features a self-centring small view mode that can be kept visible while navigating and a large view mode that allows for user interaction. The small view mode can be kept open while navigating and zoomed in and out using the `Zoom_In` and `Zoom_Out` keys. Only units with a cTab device in your own group are displayed.
  The MicroDAGR can also operate as a companion device to the Tablet (i.e. you are carrying both) and in that case the MicroDAGR will show you the same BFT data as your Tablet.
* Added configurable server-side list of helmets that define the presence of a helmet camera, defaulting to vanilla ArmA 3 and BWmod helmet models with a camera.
* Improved co-pilot seat detection for helicopters as previously the TAD could not be accessed when in the co-pilot seat of some community provided helicopters
* Enabled support for CBA Keybinding system to make key bind changes more user friendly and changeable without a restart. Userconfig settings now define the default keybinds.
* Helmet Cam item no longer occupies the radio slot when added to inventory. This is to prevent complications with TFAR.
* Changed weight of all items to be close to their real-world equivalents (before everything did weight 45g)
* Updated Android background graphic and added 3D model (for when you drop it on the ground)
* Completely reworked Android user interface
* Added messaging to Android interface
* Sent messages are now kept with the list of received messages
* Message receive notification has been made a lot less intrusive, both visually (now a small white envelope icon on the right side of the screen) and audibly (a humming sound similar to that of a mobile phone vibrating)
* The "delete messages" function will now only delete selected messages (instead of all)
* Added "small" version to Android interface, so you can now keep it visible while navigating
* Reworked Tablet user interface, switching modes feels a lot more natural now
* Reworked UAV camera positions, gunner and driver camera will now reflect what the UAV gunner and driver actually sees. Gunner camera now uses FLIR (white/hot) mode.
* Added function to lock the currently selected UAV gunner's camera to the location double-clicked on the map (select from double-click menu on BFT map). This is only available to the tablet.
* Lists of UAVs and helmet cameras are now alphabetically sorted
* Reworked helmet "full-screen" view to only occupy the whole screen of the Tablet instead of the whole screen
* Added ability to remove marker at mouse cursor position. The marker currently under the cursor will be highlighted and removed upon pressing DEL (delete on your keyboard).
* Made significant improvements to marker network synchronization. Instead of having an individual client manipulate the list of markers and then send it to every other client, a marker addition / deletion request will now be sent to the server and the server will inform all clients of the change by sending just what has changed.
* Players sitting in the back of the trucks (HMTT and Tempest by default) now no longer have access to the vehicle based FBCB2
* All interfaces are now restored to last mode of operation operation (BFT,Messaging,...) on open
* All interfaces will now restore last map position and zoom level on open
* Added topographical map mode to all devices
* Added black map mode to TAD
* Added switch (F6) to toggle map modes (satellite, topographical, black) via the "large" versions of the devices (the "small" versions will use the same setting)
* Added switch (F7) to center map on current player position
* Added current in-game time to all interfaces
* Added current grid location as well as current heading in degrees and octant to all interfaces
* Greatly enhanced user placed map markers to properly scale when zooming the map (including the directional arrow and group size denominators) as well as making sure the directional arrow does not interfere with the time-stamps
* Added new marker types for infantry (Rifle, AT, MG), static weapons (MG, AT, AA, Mortar) and wheeled MRAPs/APCs
* Added an "Exit" menu entry to the map double-click dialog (the one you place map markers with)
* Added map tools (can be toggled using F5) that show grid and elevation at mouse cursor as well as distance and direction from the current position to the mouse cursor position.
* Island sizes are now dynamically detected, no more waiting on islands being supported
* Tweaked map visuals to be easier to read
* General performance tweaks
* Added new keybinding (ALT + H) to allow access to alternative interface (i.e. to get access to the Tablet that is being carried while in one of the pilot seats of a helicopter)
* Infantry carrying a cTab device in your own group will now show with a smaller infantry icon that points in the direction of the unit as well as the index of that unit within the group (instead of a square infantry symbol with the full group ID). To anyone outside this group, there will only be a single icon shown for the group at the position of the group's leader (if equipped with a cTab device, otherwise the next unit in line).
* Infantry groups will now automatically show the group strength with dots above the icon
* Changed BFT list generation and display functions to help de-clutter everyone's view. For example transports will no longer show overlayed icons for other units with cTab equipment that are mounted. Instead, the names of groups and the group indices of units from your own group will be displayed to the left of the transport's icon.
* The arrow on the inner TAD range circle (that used to show north) will now rotate with the direction of the aircraft to help show the current direction of travel
* Made large TAD interface movable
* Excluded static weapons from showing up on BFT
* Added ability to define custom names for vehicles via the mission or scripts

### 2.0.1 ###
* Fixed "_chk_all_items" script error that could appear during Zeus (and probably other) missions

### 2.0.0 ###
* Reduced network traffic by moving the list generation of tracked elements from the server to the clients
  The server now sends an update pulse to all clients every 30 seconds (previous update interval was 20), instead of sending the whole list to every client. The clients then generate the list locally. This lessens the server load a little (shifting it to the clients), greatly reduces cTab related network traffic, while hopefully keeping the list content the same across clients since they update (almost) at the same time.
* Addition of messaging system (currently on Tablet only)
  Send text messages between team members equipped with tablets.
* Addition of cTab equipment box to be used in editor
  A simple box filled with your favourite cTab equipment. Includes 5 Tablets, 15 Android devices and 25 helmet cameras. The box is also available to Zeus.
* Tablet, Android and helmet camera are now available in Zeus when editing the contents of boxes
* Addition of TAD (Tactical Awareness Display) for use in air vehicles.
  There are two variants, one is small and can be kept visible while operating your aircraft, similar to the GPS but populated with information from your cTab network. The map will be kept centred on your current position. In the upper right hand corner of the screen you can see the current scale represented by the radius of the outer circle. The inner circle is exactly half of that.
  The other variant is a larger representation of the same TAD, but in an interactive mode and *you will lose your vehicle controls* while open.
  Overall the TAD has been modelled to have a similar look to the TAD found in the DCS A-10C module.
  All friendly aircraft icons are replaced with a little circle that has a small line attached to it, representing the current orientation of that vehicle.
  When in one of the pilot seats of an aircraft, press `IF_Main` to open or close the small TAD. Use the new `Zoom_In` and `Zoom_Out` keys to zoom the small TAD. `IF_Secondary` opens the large TAD.
* Addition of userconfig for key bindings
  This frees up the previously used `UserAction12`.
* Addition of vehicle arrays to userconfig (server side) to define vehicles equipped with on-board FBCB2 or TAD
  The lists will be read by the server and distributed to all clients. It can also be overridden by mission makers.
* cTab now closes when exiting a vehicle or when the player is killed
* Added Ifrit, Strider and Tempest to the default list of FBCB2 enabled vehicles.
* Players sitting in the cargo area of an FBCB2 enabled vehicle will no longer have access to the vehicle based FBCB2. The currently known exceptions are the trucks (HMTT and Tempest), they are working as before. We are waiting on a new command in ArmA3 release 1.26 to hopefully be able to address this.
* Fixed error appearing when helmet cam screen was selected on the tablet for the first time
* Added artillery and mortar symbols
* The increase / decrease font function now actually resizes the fonts (in addition to the symbols as before)
* General performance improvements and bug fixes
