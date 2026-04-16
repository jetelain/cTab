/*
	Name: cTab_fnc_ctrlMapVisibleBounds

	Author(s):
		GrueArbre

	Description:
		Compute the visible world-space bounding box of a map control with an optional margin.
		Used to cull off-screen drawIcon calls.

	Parameters:
		0: OBJECT  - Map control
		1: NUMBER  - Margin in world metres to expand the bounds (default: 100)

	Returns:
		ARRAY - [minX, maxX, minY, maxY] in world coordinates

	Example:
		private _bounds = [_ctrlScreen, 100] call cTab_fnc_ctrlMapVisibleBounds;
		// _bounds select 0 => minX
		// _bounds select 1 => maxX
		// _bounds select 2 => minY
		// _bounds select 3 => maxY
*/

params ["_ctrlScreen", ["_margin", 100]];

private _ctrlPos = ctrlPosition _ctrlScreen;
private _worldTL = _ctrlScreen ctrlMapScreenToWorld [_ctrlPos select 0, _ctrlPos select 1];
private _worldBR = _ctrlScreen ctrlMapScreenToWorld [(_ctrlPos select 0) + (_ctrlPos select 2), (_ctrlPos select 1) + (_ctrlPos select 3)];

[
	(_worldTL select 0) - _margin,
	(_worldBR select 0) + _margin,
	(_worldBR select 1) - _margin,
	(_worldTL select 1) + _margin
]
