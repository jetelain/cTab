# Mission Makers and Server Documentation

See also [Install and key bindings](EndUser.md)

For Mission Makers
------------------

### Mod Settings (CBA) ###

You can adjust mod behavior using [CBA settings](https://github.com/CBATeam/CBA_A3/wiki/CBA-Settings-System).

    CONFIGURE > CONFIGURE ADDONS
    Select "cTab" from the ADDON dropdown

* Synchronisation rate (`ctab_core_sync_time`) :<br /> Time required for new devices to be discovered. This setting is always enforced by server or mission.
  - 30 seconds by default
* Blue Force Tracking (`ctab_core_bft_mode`) : 
  - Disabled
  - Real time (default) 
  - At each synchronisation (more realistic)
* Helmet Cam (`ctab_core_helmetcam_mode`) : 
  - Disabled 
  - Enabled (default)
* UAV feeds (`ctab_core_uav_mode`) : 
  - Disabled 
  - Enabled (default)
* Compass unit (`ctab_core_useMils`) : 
  - Mils 
  - Degrees (default)
* Default map mode (`ctab_core_defMapStyle`) : 
  - Topographic 
  - Satellite (default)

All settings can be enforced by mission or server.

### Class Names ###

```SQF
ItemcTab // Commander Tablet
ItemAndroid // Android based Blue Force Tracker
ItemcTabHCam // Helmet Cam
ItemMicroDAGR // MicroDAGR GPS
```

If [ACE3](https://github.com/acemod/ACE3) is used, you can also use
```SQF
ACE_MicroDAGR // ACE MicroDAGR GPS
```
You may disable ACE MicroDAGR support usings mods settings (`ctab_core_useAceMicroDagr`).

### Add items to a box ###
Place this in the initialisation of an Ammo box to add 10 of each item:

```SQF
this addItemCargo ["ItemcTab",10];this addItemCargo ["ItemcTabHCam",10];this addItemCargo ["ItemAndroid",10];this addItemCargo ["ItemMicroDAGR",10];
```

### Add item to a unit directly ###
Place this in the initialisation of a soldier:

```SQF
this addItem "ItemcTab";
```

Note: This will add the item to the actual inventory, but not assign it to the GPS slot. The unit will have to have enough space in its inventory to fit the item, otherwise it won't be assigned. `addItem` assigns the item to the next best inventory container that fits the item, in the order of uniform, vest and backpack. The Tablet (`itemcTab`) for example won't fit in most uniforms, so there has to be space in either the vest or backpack. Use `addItemToBackpack` to add the item directly to the unit's backpack and `addItemToVest` to directly assign it to the vest. To assign an item directly to the GPS slot (it has no space restrictions, but will still count towards the unit's total load), use ´linkItem´ instead.

This will for example assign the MicroDAGR to the GPS slot and place the Tablet into the unit's backpack:

```SQF
this linkItem "ItemMicroDAGR";this addItemToBackpack "ItemcTab";
```

### Set cTab side-specific encryption keys ###
If you wish multiple factions to share cTab data, you will have to set their encryption keys to be the same. These are the variables that hold the encryption keys with their default values:

```SQF
cTab_encryptionKey_west = "b";
cTab_encryptionKey_east = "o";
cTab_encryptionKey_guer = "i";
cTab_encryptionKey_civ = "c";
```

Note: It is advised to keep the encryption keys as short as possible since some actions use them to exchange data across the network, so by keeping them short, there is less data exchanged.

So if you want to have for example OPFOR and GUER share cTab data, put this at the **TOP** of your `init.sqf`:

```SQF
// set GUER encryption key to be the same as the default BLUEFOR encryption key
cTab_encryptionKey_guer = "b";
```

Note: If `GUER` is set to be friendly with either `WEST` or `EAST`, `GUER` will by default have the same encryption key as the friendly faction. Set `cTab_encryptionKey_guer = "i";` to override.

### Override vehicle types that have FBCB2 or TAD available ###
If you wish to override the list of vehicles that have FBCB2 or TAD available, put this at the **TOP** of your `init.sqf`:

```SQF
// only make FBCB2 available to MRAPs, APCs and tanks
cTab_vehicleClass_has_FBCB2 = ["MRAP_01_base_F","MRAP_02_base_F","MRAP_03_base_F","Wheeled_APC_F","Tank"];

// make TAD available to all helicopters and planes with the exception of the MH-9 Hummingbird and AH-9 Pawnee
cTab_vehicleClass_has_TAD = ["Heli_Attack_01_base_F","Heli_Attack_02_base_F","Heli_Light_02_base_F","Heli_Transport_01_base_F","Heli_Transport_02_base_F","I_Heli_light_03_base_F","Plane"];
```

### Override helmet classes with enabled helmet camera ###

```SQF
// Only have BWmod helmets with a camera simulate a helmet camera
cTab_helmetClass_has_HCam = ["BWA3_OpsCore_Schwarz_Camera","BWA3_OpsCore_Tropen_Camera"];
```

### Change display name of a group ###
Groups are displayed on cTab devices with their groupIDs. To define custom groupIDs, add the following code to the group leader's initialization:

```SQF
// Change the unit's groupID to "Red Devils"
this setGroupId ["Red Devils"];
```

### Change display name of a vehicle ###
By default all vehicles will be shown with their group names next to them. This can make it difficult to distinguish multiple vehicles of the same type when they are in the same group. To change that, use the following code in the vehicle's initialization:

```SQF
// Change this vehicle's identification displayed on all cTab Blue Force Trackers to "Fox"
this setVariable ["cTab_groupId","Fox",true];
```

Server only settings
--------------------

### Define vehicle types that have FBCB2 or TAD available ###
To configure the list of vehicle types that have FBCB2 or TAD available, edit the `cTab_vehicleClass_has_FBCB2` and `cTab_vehicleClass_has_TAD` arrays in the configuration file on the server, which can be found in the ArmA 3 folder `...\Arma 3\userconfig\cTab\ctab_settings.hpp`.

```SQF
class cTab_settings {
    cTab_vehicleClass_has_FBCB2[] = {"MRAP_01_base_F","MRAP_02_base_F","MRAP_03_base_F","Wheeled_APC_F","Tank","Truck_01_base_F","Truck_03_base_F"};
    cTab_vehicleClass_has_TAD[] = {"Helicopter","Plane"};
};
```

Note: This is a server-side setting, i.e. whatever is set on the client-side userconfig will be overridden by the userconfig on the server.

### Define helmet classes with enabled helmet camera ###
To configure the list of helmet classes that enable helmet cameras, edit the `cTab_helmetClass_has_HCam` array in the configuration file on the server, which can be found in the ArmA 3 folder `...\Arma 3\userconfig\cTab\ctab_settings.hpp`. It needs to be within the class `cTab_settings` (same area as above).

```SQF
class cTab_settings {
    cTab_helmetClass_has_HCam = {"H_HelmetB_light","H_Helmet_Kerry","H_HelmetSpecB","H_HelmetO_ocamo","BWA3_OpsCore_Fleck_Camera","BWA3_OpsCore_Schwarz_Camera","BWA3_OpsCore_Tropen_Camera"};
};
```

Note: This is a server-side setting, i.e. whatever is set on the client-side userconfig will be overridden by the userconfig on the server.