# End User Documentation

You can configure supported vehicles and some features. See [Mission makers and server settings](MissionMaker.md)

Install
-------

### Steam Workshop install (recommanded for end users)

Subscribe to the mod from the [Steam Workshop page](TODO). Then in the Arma3 game launcher, in the "Mods" tab check "CBA_A3" and "cTab - Blue Force Tracking".

### Full manual install (required for servers)
Place the `@cTab` and `userconfig` folders in your ArmA 3 folder (*both on the server and the clients*). Then start the game with `@CBA_A3`and `@cTab`. It can be done either by enabling the necessary mods in the game settings (Settings -> Expansions), or by adding the mod names to the game shortcut after the EXE file, i.e. `...\arma3.exe -mod=@CBA_A3;@cTab`.

The keys folder is for the server key.

How to configure key bindings
-----------------------------
Key Bindings can be configured via the CBA Keybinding system available to you from the configure controls screen once you are in game.

    CONFIGURE > CONTROLS > CONFIGURE ADDONS
    Select "cTab" from the ADDON dropdown

| Keys | Action |
| --- | --- |
| `H` | This key is used to open and close whatever cTab device is available to you, showing the "small" version where available. It can also be used to close UAV view. |
| `CTRL` + `H` | Opens and closes the secondary view mode of the cTab device available to you. This would usually be the "large" version. |
| `ALT` + `H` | Opens and closes an alternative cTab device that you may have available (i.e. when a pilot carrying a tablet, this will open the tablet). |
| `LEFT DOUBLE-CLICK` |  Opens dialog to place markers at mouse cursor location. Not available on MicroDAGR |
| `DEL` |  Deletes the highlighted user placed marker under your cursor. |
| `F1` |  Change to Blue Force Tracker on tablet. |
| `F2` |  Change to UAV cameras on tablet. |
| `F3` |  Change to helmet cameras on tablet. |
| `F4` |  Change to Message mode on tablet. |
| `F5` |  Toggles map tools (grid/elevation/range/direction to mouse cursor). |
| `F6` |  Toggles map mode (satellite/topographical/black). |
| `F7` |  Center map on current position. |
| `CTRL` + `SHIFT` + `PAGE_UP` | Zoom in on the "small" TAD, MicroDAGR and Android. |
| `CTRL` + `SHIFT` + `PAGE_DOWN` | Zoom out on the "small" TAD, MicroDAGR and Android. |
| `CTRL` + `SHIFT` + `HOME` | Toggle interface position from left to right on the "small" TAD, MicroDAGR and Android. Reset all "large" interfaces to default position. |
| `ESC` | Closes all interactive cTab devices (i.e. all but the "small" variants) as well as the UAV view. |

Note: To unlock a UAV turret, use the lock / unlock control command available to UAVs (default `CTRL` + `T`) when controlling the UAV turret in full-screen mode (either via the UAV terminal or the UAV gunner view accessible from the tablet).
