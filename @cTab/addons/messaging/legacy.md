# Legacy/existing messaging system

## Legacy/existing system

The legacy messaging system allows players equipped with a tablet (`ItemcTab`) or Android (`ItemAndroid`) device to compose and send plain-text messages to other eligible players on the same side. Messages are stored locally on each player's unit object and are never persisted beyond the current mission session.

### Data model

Messages are stored as an array on the player object unit variable, scoped by an encryption key:

```sqf
cTab_player getVariable [format ["cTab_messages_%1", _encryptionKey], []]
```

Each entry in the array is a three-element array:

| Index | Type   | Description                                        |
|-------|--------|----------------------------------------------------|
| 0     | STRING | Message title (formatted sender/time header)       |
| 1     | STRING | Message body text                                  |
| 2     | NUMBER | Message state: `0` = unread, `1` = read, `2` = sent |

Note: A 4th element is used by cTabIRL connect addon / cTabWebApp integration (see below)

### Encryption keys

Encryption keys are mission-namespace global variables that control which message variable is used per faction. They effectively prevent cross-faction message reading by ensuring each side writes and reads its own keyed variable.

| Variable                    | Default value |
|-----------------------------|---------------|
| `cTab_encryptionKey_west`   | `"b"`         |
| `cTab_encryptionKey_east`   | `"o"`         |
| `cTab_encryptionKey_guer`   | `"i"` (or `"b"`/`"o"` depending on allies) |
| `cTab_encryptionKey_civ`    | `"c"`         |

The current player's key is retrieved via `cTab_fnc_getPlayerEncryptionKey`:
```sqf
missionNamespace getVariable [format ["cTab_encryptionKey_%1", side cTab_player], ""]
```

### Send flow

Entry point: `cTab_fnc_msg_Send` (UI button handler in the core addon, `@cTab/addons/core/functions/fnc_msg_Send.sqf`)

1. Reads the composed message text from `IDC_CTAB_MSG_COMPOSE`.
2. Calls `EFUNC(messaging,getSelectedRecipients)` to get the list of selected recipient units from `IDC_CTAB_MSG_RECIPIENTS`.
3. Calls `cTab_fnc_sendMessage` (`@cTab/addons/core/functions/fnc_sendMessage.sqf`) with `[_msgBody, _recipList]`.

Inside `cTab_fnc_sendMessage`:

1. Validates that message body and recipient list are non-empty.
2. Retrieves the sender's encryption key via `cTab_fnc_getPlayerEncryptionKey`.
3. Retrieves the current in-game time via `cTab_fnc_currentTime`.
4. Formats the message title as:  
   `"<time> - <groupId group sender>:<groupId sender> (<name sender>)"`
5. For **each recipient**, fires the `cTab_msg_receive` CBA local event on the recipient's machine via `CBA_fnc_whereLocalEvent`:
   ```sqf
   ["cTab_msg_receive", [_recip, _msgTitle, _msgBody, _playerEncryptionKey, cTab_player]] call CBA_fnc_whereLocalEvent;
   ```
6. Appends a sent-copy of the message (state `2`) to the sender's own message array.
7. If the sender's interface is open in `MESSAGE` mode, refreshes the message list via `cTab_msg_gui_load`.
8. Fires the `"MSG"` notification (`LLSTRING(MessageSent)`) and plays `cTab_mailSent` sound.
9. Fires `QGVARMAIN(messagesUpdated)` as a local CBA event to trigger any UI subscribers.

### Receive flow

The `cTab_msg_receive` local event handler is registered in `@cTab/addons/core/XEH_postInitClient.sqf`:

1. Appends the received message (state `0` = unread) to the recipient's keyed message variable.
2. Fires `messagesUpdated` event.
3. If the recipient is the local player, the sender is different from the player, the encryption key matches, and the player has a `leaderDevices` item:
   - Plays `cTab_phoneVibrate` sound.
   - If the MESSAGE mode UI is open: refreshes the message list and shows a `"New message from <sender>"` notification.
   - Otherwise: shows the mail icon overlay via `cTabRscLayerMailNotification`.

### Recipient eligibility

Recipients are populated by `EFUNC(messaging,fillRecipientList)` (`@cTab/addons/messaging/functions/fnc_fillRecipientList.sqf`). A unit is eligible if it meets **all** of the following criteria:

1. Its side is in the player's valid sides list (via `cTab_fnc_getPlayerSides`).
2. `isPlayer` is true.
3. It carries at least one `leaderDevices` item (tab or Android device, and optionally MicroDAGR if `microDagrGroupBFT` is enabled), checked via `cTab_fnc_checkGear`.

The eligible unit is displayed as `"<groupId group unit>:<groupId unit> (<name unit>)"` in the recipient listbox.

### Core addon functions

| Function | File | Description |
|---|---|---|
| `cTab_fnc_sendMessage` | `core/functions/fnc_sendMessage.sqf` | Core send logic: formats title, fires `cTab_msg_receive` per recipient, stores sent copy |
| `cTab_fnc_msg_Send` | `core/functions/fnc_msg_Send.sqf` | UI button handler: reads compose field, calls `sendMessage` |
| `cTab_fnc_msg_gui_load` | `core/functions/fnc_msg_gui_load.sqf` | Loads message list into `IDC_CTAB_MSG_LIST` with state icons; also refreshes recipient list |
| `cTab_fnc_msg_get_mailTxt` | `core/functions/fnc_msg_get_mailTxt.sqf` | Displays selected message body, marks it as read (state `1`) |
| `cTab_fnc_deleteMessages` | `core/functions/fnc_deleteMessages.sqf` | Deletes messages at given indices from player's message array |
| `cTab_fnc_msg_delete_all` | `core/functions/fnc_msg_delete_all.sqf` | Clears the entire message array for the player's encryption key |
| `cTab_fnc_getPlayerEncryptionKey` | `core/functions/fnc_getPlayerEncryptionKey.sqf` | Returns the encryption key string for the current player's side |
| `cTab_fnc_getPlayerSides` | `core/functions/fnc_getPlayerSides.sqf` | Returns the list of sides the current player can communicate with |

### Template system (messaging addon)

The `messaging` addon (`@cTab/addons/messaging/`) adds structured message templates on top of the core send primitive. Templates allow filling in pre-defined fields (grid coordinates, callsigns, checkboxes, etc.) and generating a formatted plain-text body, which is then passed to `cTab_fnc_sendMessage` unchanged.

**Message types** (`defines.hpp`):

| Constant            | Value |
|---------------------|-------|
| `MSG_TYPE_GENERIC`  | 0     |
| `MSG_TYPE_MEDEVAC`  | 1     |
| `MSG_TYPE_ARTILLERY`| 2     |
| `MSG_TYPE_AIRSUPPORT`| 3    |

**Field types** (`defines.hpp`): `TEXT`, `NUMBER`, `DATETIME`, `CALLSIGN`, `FREQUENCY`, `MARKER`, `CHECKBOX`, `GRID`, `MULTILINE_TEXT` (values 0–8).

**Template data structure:**  
`[uid, messageType, title, shortTitle, href, lines]`  
where `lines` = `[["lineTitle", "lineDesc", [fields]], ...]`  
and `fields` = `[["fieldTitle", "fieldDesc", fieldType], ...]`.

**Attachment types** (`defines.hpp`):

| Constant                  | Value |
|---------------------------|-------|
| `MSG_ATTACHMENT_MARKER`   | 0     |
| `MSG_ATTACHMENT_GRID`     | 1     |

When a template message with a `MSG_ATTACHMENT_MARKER` attachment is sent, `EFUNC(messaging,sendMessage)` (`messaging/functions/fnc_sendMessage.sqf`) also creates a user marker on the map at the attachment center position via `cTab_fnc_addUserMarker`.

### Messaging addon functions

| Function | File | Description |
|---|---|---|
| `fnc_registerMessageTemplate` | `messaging/functions/fnc_registerMessageTemplate.sqf` | Registers a custom template into the template array |
| `fnc_generateTemplateUI` | `messaging/functions/fnc_generateTemplateUI.sqf` | Dynamically builds the template fill-in dialog |
| `fnc_generateTemplateText` | `messaging/functions/fnc_generateTemplateText.sqf` | Renders filled fields into a plain-text message body |
| `fnc_updateMessagePreview` | `messaging/functions/fnc_updateMessagePreview.sqf` | Updates the live preview as the user fills in fields |
| `fnc_getDefaultFieldValue` | `messaging/functions/fnc_getDefaultFieldValue.sqf` | Returns auto-populated placeholder values per field type |
| `fnc_parseGridPosition` | `messaging/functions/fnc_parseGridPosition.sqf` | Parses grid coordinate string formats |
| `fnc_fillRecipientList` | `messaging/functions/fnc_fillRecipientList.sqf` | Populates the recipient listbox with eligible players |
| `fnc_getSelectedRecipients` | `messaging/functions/fnc_getSelectedRecipients.sqf` | Returns the list of unit objects selected in the recipient listbox |
| `fnc_sendMessage` | `messaging/functions/fnc_sendMessage.sqf` | Template send wrapper: calls core `sendMessage`, creates map markers from attachments |
| `fnc_btnSendTemplatedMessage` | `messaging/functions/fnc_btnSendTemplatedMessage.sqf` | Send button handler for the template dialog |
| `fnc_showTemplateUI` | `messaging/functions/fnc_showTemplateUI.sqf` | Opens the template fill-in dialog |
| `fnc_closeTemplateUI` | `messaging/functions/fnc_closeTemplateUI.sqf` | Closes the template dialog |
| `fnc_btnShowTemplates` | `messaging/functions/fnc_btnShowTemplates.sqf` | Shows the template chooser list |
| `fnc_btnShowMedevacTemplate` | `messaging/functions/fnc_btnShowMedevacTemplate.sqf` | Shortcut to open the MEDEVAC template directly |

### Known limitations

- **No server-side persistence.** Messages are stored only on client unit variables; they are lost on disconnect or mission end.
- **No delivery guarantee.** `CBA_fnc_whereLocalEvent` requires the recipient's machine to be reachable at send time. If the recipient is not yet connected or their object is not local, the message is silently dropped.
- **No message history across sessions.** Each player starts each mission with an empty inbox.
- **Sender copies are stored separately.** The sender's outbox entry is written directly to their local variable; there is no synchronization if the sender switches machines.
- **Encryption keys are cosmetic scoping only.** They prevent accidental cross-faction display but do not provide real cryptographic security.
- **Recipient list is static.** The list is populated at the time the MESSAGE panel is opened and is not dynamically refreshed.

## cTabIRL connect addon / cTabWebApp integration

The cTabIRL mod extends the in-game messaging system so that messages are also visible (and sendable) through a web browser via the **cTabWebApp**. The bridge between Arma 3 and the web app is the **cTabExtension** .NET DLL, which communicates with the web app over a persistent **SignalR** WebSocket connection.

### Architecture overview

```
Arma 3 (SQF)
    ↕  callExtension / ExtensionCallback
cTabExtension.dll (C# Worker.cs)
    ↕  SignalR WebSocket
cTabWebApp (CTabHub.cs)
    ↕  SignalR WebSocket
Web browser (player's real device)
```

Each player's Arma client has its own SignalR connection. The hub maintains one `PlayerState` object per player that caches the latest version of each message type, so a newly connecting browser immediately receives the current state.

### Connection and initialisation

1. At `CBA_settingsInitialized`, `FUNC(connect)` (`connect/functions/fnc_connect.sqf`) calls  
   `"cTabExtension" callExtension ["Connect", [uri, steamId, profileName, key]]`  
   The extension opens a SignalR connection to the web app and invokes `ArmaHello`.
2. The hub responds with a `"Connected"` callback carrying a QR code and the server URI.
3. The SQF `ExtensionCallback` handler (in `XEH_preInit.sqf`) fires `FUNC(onConnected)`, which:
   - Creates a diary entry with the URL and QR code so the player can scan it.
   - Calls `FUNC(updateDevices)` to report the player's device level.
   - Calls `FUNC(updateMessageTemplates)` to push available templates to the web app.
   - Calls `FUNC(updateIntelSideFeed)` to push intelligence items.

### Device level and messaging eligibility

`FUNC(updateDevices)` (`connect/functions/fnc_updateDevices.sqf`) computes a `deviceLevel` integer and sends it to the extension via `"cTabExtension" callExtension ["Devices", [deviceLevel, useMils, vehicleMode]]`:

| Level | Meaning |
|-------|---------|
| 0 | No device — no BFT, no messaging on web app |
| 2 | BFT only (vehicle TAD/FBCB2 seat) |
| 3 | BFT + Messaging (player has a `leaderDevices` item) |

**Only players at device level 3** receive and can send messages through the web app. `FUNC(updateMessages)` is also called immediately when the level transitions to 3, to ensure the web app inbox is not empty.

### Arma → web app: pushing messages

The `ctab_messagesUpdated` CBA event (fired whenever the local message array changes) triggers `FUNC(updateMessages)` (`connect/functions/fnc_updateMessages.sqf`).

`fnc_updateMessages` guards on `GVAR(deviceLevel) < 3` and then:

1. Reads the player's full message array (keyed by encryption key).
2. Assigns a stable string ID (`"m<n>"`) to each message that does not yet have one (the legacy 3-element array is extended to 4 elements).
3. Sends the entire array to the extension:  
   `"cTabExtension" callExtension ["UpdateMessages", _msgArray]`

The extension forwards this as a SignalR `ArmaUpdateMessages` call. In `CTabHub.ArmaUpdateMessages`, each entry is parsed by `Arma3MessagingHelper.MessageFromArma` into a `Message` object:

| Arma index | C# property | Notes |
|---|---|---|
| 0 | `Title` | Sender/time header |
| 1 | `Body` | Plain-text body |
| 2 | `State` | 0 = unread, 1 = read, 2 = sent |
| 3 | `Id` | Stable `"m<n>"` identifier |
| 4 | `Type` | `MessageTemplateType` enum (optional, cTab ≥ 2.7) |
| 5 | `Attachments` | List of `MessageAttachment` (optional, cTab ≥ 2.7) |

The resulting `UpdateMessagesMessage` is cached in `state.LastUpdateMessages` and broadcast to all web clients in the player's `WebChannelName` SignalR group via `Clients.Group(...).SendAsync("UpdateMessages", ...)`.

### Arma → web app: pushing templates

On connection (`FUNC(onConnected)`) and whenever `ctab_messaging_templates` event fires, `FUNC(updateMessageTemplates)` sends the template array to the extension:  
`"cTabExtension" callExtension ["UpdateMessageTemplates", ctab_messaging_templates]`

In `CTabHub.ArmaUpdateMessageTemplates`, each entry is parsed by `Arma3MessagingHelper.TemplateFromArma` into a `MessageTemplate` object (same structure as the in-game template, see the Legacy system section) and broadcast as `"UpdateMessageTemplates"` to web clients.

If the mod is older than 2.7 and no templates were received, the hub sends a fallback containing only the built-in MEDEVAC template (`BuiltinTemplates.GetMedevac()`).

### Web app → Arma: sending a message

When the player taps **Send** in the web app, the browser calls `WebSendMessage` on the hub with a `WebSendMessageMessage`:

| Property | Type | Description |
|---|---|---|
| `To` | string | Target identifier (group SignalR name) |
| `Body` | string | Message body text |
| `Title` | string | Template title (optional) |
| `Type` | `MessageTemplateType` | Template type enum |
| `Attachments` | `List<MessageAttachment>` | Grid/marker attachments (optional) |

Input is validated (non-empty `To` and `Body`, body ≤ 5 000 chars, `To` ≤ 32 chars). `Arma3MessagingHelper.ToArmaSimpleArrayString` serialises the message into an Arma-compatible simple array string:  
`[targetId, body, [title, "", type, [attachments...]]]`

This is relayed to the Arma client as:  
`Clients.Group(state.ArmaChannelName).SendAsync("Callback", "SendMessage", data)`

The extension receives it and fires an `ExtensionCallback` with function `"SendMessage"`. The SQF handler in `XEH_preInit.sqf` dispatches it to `FUNC(sendMessage)` (`connect/functions/fnc_sendMessage.sqf`).

`FUNC(sendMessage)` resolves the recipient unit from `_targetId` by searching `cTabBFTgroups`, then:

- If the new messaging addon (`ctab_messaging_fnc_sendMessage`) is present, calls `[_data, _recipList] call ctab_messaging_fnc_sendMessage` (which eventually reaches `ctab_core_fnc_sendMessage` and adds map markers for attachments).
- Otherwise falls back to `[_msgBody, _recipList] call ctab_core_fnc_sendMessage` directly.

Both paths trigger the standard legacy `cTab_msg_receive` event flow, so the message appears in the in-game inbox of the recipient just as if sent from the tablet UI.

### Web app → Arma: read and delete acknowledgements

When the player opens a message in the browser, the web app calls `WebMessageRead(id)` on the hub. The hub relays it to the Arma client:  
`Clients.Group(state.ArmaChannelName).SendAsync("Callback", "MessageRead", "[\"m<n>\"]")`

The `ExtensionCallback` handler dispatches it to `FUNC(markMessageRead)` (`connect/functions/fnc_markMessageRead.sqf`), which:
1. Finds the message by its `"m<n>"` ID in the player's local array.
2. Sets state to `1` (read).
3. Dismisses the mail icon overlay (`cTabRscLayerMailNotification cutText ["", "PLAIN"]`).

Similarly `WebDeleteMessage(id)` → `"DeleteMessage"` callback → `FUNC(deleteMessage)` (`connect/functions/fnc_deleteMessage.sqf`) → `ctab_core_fnc_deleteMessages`.

### SignalR hub methods summary

| Direction | Hub method | Description |
|---|---|---|
| Arma → Hub | `ArmaUpdateMessages` | Receives full message array, caches, broadcasts to web clients |
| Arma → Hub | `ArmaUpdateMessageTemplates` | Receives template list, caches, broadcasts to web clients |
| Web → Hub | `WebSendMessage` | Validates, serialises, relays send command to Arma client |
| Web → Hub | `WebMessageRead` | Relays read acknowledgement to Arma client |
| Web → Hub | `WebDeleteMessage` | Relays delete request to Arma client |
| Hub → Web | `UpdateMessages` | Push current message list to browser(s) |
| Hub → Web | `UpdateMessageTemplates` | Push available templates to browser(s) |

### Known limitations

- **One-way initial sync only.** The web app receives a full snapshot on connect (`state.LastUpdateMessages`), but individual message state changes (e.g. read status changed in-game) only reach the web app the next time `ctab_messagesUpdated` fires and `UpdateMessages` is re-pushed as a full array.
- **No cross-player web delivery.** The `WebSendMessage` path targets a unit on the sender's Arma machine; it cannot deliver directly to another player's web browser. The message is injected into Arma first and then propagated via the normal in-game `cTab_msg_receive` mechanism.
- **Extension required.** The entire bridge is inoperative if the `cTabExtension.dll` is not loaded (e.g. server-side only or Linux clients).
- **Message IDs are session-local.** The `"m<n>"` identifiers are allocated from `GVAR(nextMessageId)` which starts at `0` each mission; they are not globally unique across sessions.
