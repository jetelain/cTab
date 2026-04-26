# Messaging system

Feature planned for cTab 2.9.

## Legacy/existing system

See [Legacy/existing messaging system](legacy.md)

## New channel based system

The goal is to fix most known limitations, and allow easier discussions with multiple players.

The main user feedback of messaging is that it's too difficult to discuss with multiple players:
- Orders need to be sent to all units to ensure everybody will be able to anticipate other units' behavior.
- Tactical information needs to be shared with all units (like markers and intels).

### Design changes

Messages will be sent to a "persistent" channel (stored on server). 

Four types of channels will be available :
- Side channel (one per encryption key) : All players who share the same encryption key will be automatically member of this channel
- Custom : Player created channel, with manual membership (anyone with the same encryption key will be able to join).
- Group to Group (G2G) : A channel will exist for each pair of groups that share the same encryption key, all members of the two groups will be automatically member of this channel. This channel will be displayed if at least one member of each group has a Tablet or Android device, or if the channel has existing messages.
- Player to Player (P2P) : A channel will exist for each pair of players that share the same encryption key. The two players will be automatically member of this channel. This channel will be displayed if both players have a Tablet or Android device, or if the channel has existing messages.

Messages will have a global identifier (assigned by the server), that will replace the existing identifier in cTabIRL connect.

User interface will look like Slack/Discord/Mattermost : channel list, and all messages displayed as a chat (in game interface might be different due to arma3 limitations).

A special "All" channel will be available with messages from all channels which player is member of. Of course, it will not be possible to send messages on this channel.

### Performance consideration

Messaging is a low priority feature compared to other game features. The new system should have little to no impact on performance. It should be event driven.

### Data model

#### Channel identifier, and metadata

Channel identifier is a formatted string, starting with encryption key.

Each group have an integer identifier assigned by server stored as a public variable `ctab_id` (`QGVARMAIN(id)` with macro). Group identifiers are assigned **on demand**: when a message is first sent to a channel involving a group that has no `ctab_id` yet, the server increments a global counter `ctab_core_nextGroupId` and assigns the new value as the group's `ctab_id` (public variable, immediately visible on all machines). The decimal string representation is used in the channel identifier (e.g. group `42` → `"42"`, no zero-padding).

Note: group identifier assignation is implemented in the `core` addon, to allow re-use for other features.

For players, the Steam Id is used as identifier (`getPlayerUID`) to ensure re-using the same channel across connections.

Format par channel type:
- Side channel : `{key}_s`
- Custom channel : `{key}_c_{id}`
  - `id` is an integer identifier assigned by server
- Group to group channel : `{key}_g_{group1}_{group2}`
  - `group1` is the group with the lower identifier
  - `group2` is the group with the higher identifier
- Player to player channel : `{key}_p_{player1}_{player2}`
  - `player1` is the player with the lower identifier
  - `player2` is the player with the higher identifier
- All (exists only player side) : `{key}_all`

Channel metadata is an array used for channel related event:

Common structure:
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     0 | String  | Encryption key                                                     |
|     1 | Integer | Channel type (`0` Side, `1` Custom, `2` G2G, `3` P2P, `4` All)     |

Custom (type `1`)
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     2 | Integer | Server assigned identifier                                         |
|     3 | String  | Label                                                              |

Group to group (type `2`)
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     2 | Group   | Group with the lower identifier (group object)                     |
|     3 | Group   | Group with the higher identifier (group object)                    |

Note: Server might re-order groups, as identifier might have not yet assigned.

Player to player (type `3`)
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     2 | String  | Player with the lower identifier                                   |
|     3 | String  | Player with the higher identifier                                  |

Note: Player object is not used, as it will change if player respawn or reconnect.

Side (type `0`) and all (type `4`)
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     2 | Integer | `0` (unused)                                                       |
|     3 | Integer | `0` (unused)                                                       |

#### Message payload

Each message payload is an array:

| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     0 | Integer | Message id (integer, globally unique and monotonically increasing) |
|     1 | Array   | Message timestamp (result of `date`)                               |
|     2 | String  | Message title (Sender name, with optionnal template suffix)        |
|     3 | String  | Message body (2000 chars max)                                      |
|     4 | Integer | Message type (see `defines.hpp`)                                   |
|     5 | String  | Message sender (player identifier)                                 |
|     6 | Array   | Array of attachment payload                                        |

#### Attachment payload

Each attachment payload is an array. Structure depends on the type of the attachment.

Common structure :
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     0 | Integer | Attachment type (see `defines.hpp`)                                |
|     1 | String  | Attachment title                                                   |

Marker (`MSG_ATTACHMENT_MARKER`) or position (`MSG_ATTACHMENT_GRID`) Attachment :
| Index | Type    | Description                                                        |
|-------|---------|--------------------------------------------------------------------|
|     2 | String  | Grid position label (formatted with `ctab_core_gridPosition`)      |
|     3 | Array   | Grid position center `[x, y]`                                      |
|     4 | Array   | Grid position `[x+dx/2, y+dy/2]`                                   |
|     5 | Array   | Grid position precision `[dx, dy]`                                 |

#### Server side

**Server-side counters (variables on server machine):**
- `ctab_messaging_nextMessageId` : Global integer message counter, starts at `0`. Incremented for every new message across all channels. Because it is monotonically increasing, messages can be sorted globally by id without relying on timestamps.
- `ctab_core_nextGroupId` : Global integer group counter, starts at `0`. Incremented when a group is assigned a `ctab_id` on demand.
- `ctab_messaging_nextCustomChannelId` : Global integer custom channel counter, starts at `0`. Incremented when a new custom channel is created.

For all channels : hashmap `ctab_messaging_channels` (`QGVAR(channels)` with macro) with :
- Key: Channel identifier
- Value: Data array
  - [0] : Channel metadata
  - [1] : Messages: array of message payload
  - [2] : If channel is custom, array of members (array of string), empty array otherwise


Array `ctab_messaging_customChannels` with all existing custom channels (public variable, also accessible from players) :
- [0] : Channel identifier
- [1] : Channel metadata

This array is initialized to `[]` on the server at mission start and broadcast as a public variable. It is updated whenever a custom channel is created or removed.

Note: The "All" channel is not allowed server side.

#### Player side

Hashmap `ctab_messaging_playerChannels` (`QGVAR(playerChannels)` with macro) with :
- Key: Channel identifier
- Value: Data array
  - [0] : Channel metadata
  - [1] : Messages, each message is an array
    - [0] : Message payload (as received from server)
    - [1] : Message state (`0` = unread, `1` = read, `2` = sent)
    - [2] : Initial channel identifier (to find out initial channel when read from "All" channel)
  - [2] : If channel is custom, array of members, empty array otherwise
  - [3] : Unread message count

Note: The "All" channel is a special case, when a message is received it's added in the target channel, and in the "All" channel.

### Channel membership

#### Members of a channel

Except for custom channels, members of a channel is computed :
- Side : All members of side(s) (`units <side>`)
- Group to group (G2G) : Members of both groups (`units <group>`)
- Player to player (P2P) : Both players (need lookup of `allPlayers` then test `getPlayerUID`)

For side and G2G channels, members are filtered, they must have a leader device (`ctab_core_leaderDevices`, `GVAR(leaderDevices)`) in their inventory and be a player (`isPlayer`). The channel and its messages are preserved on the server regardless; when a player re-equips a device they receive the backlog via the normal join flow.

For a P2P channel messages are delivred to both players, regardless of having a leader device. 

#### Channels of current player

Channel list will be computed on demand, as it's not required to dispatch messages, but only for user interface. Most of time the interface will not be displayed.

For G2G and P2P channels, once a message have been exchanged the channel remains. Meaning that even if one of the side has no more leader device, the channel still appears (for eligible/unfiltered players).

### Send workflow

- Player send a `ctab_messaging_sendMessage` CBA event to the server
  - Channel metadata (always; includes group objects or player UIDs as applicable — the client does not need to resolve the channel identifier itself)
  - Message payload (with placeholder id `-1`)
- Server assigns `ctab_id` to any group in the channel that lacks one (increments `ctab_core_nextGroupId`, sets public variable on all machines)
- Server computes the authoritative channel identifier from metadata; creates a channel entry if none exists yet
- Server increments `ctab_messaging_nextMessageId` and assigns it as the message id
- If the message has a Marker attachment, creates the user marker on the server using `cTab_fnc_addUserMarker`.
- Server add message to channel
- Server sends a `ctab_messaging_messagesReceived` CBA event to all **current members of the channel** (for Side and G2G channels: filtered to players with a `leaderDevices` item and the correct encryption key; for P2P channels: both players are included regardless of device, consistent with the membership rules above)
- Each member creates channel entry if it does not yet exist, and adds message to the list (target channel, and "All" channel)
- Each member triggers the `ctab_messagesUpdated` CBA local event (`QGVARMAIN(messagesUpdated)` with macro)

### Player joining

For custom channels, member list is owned by the server.

**Joining an existing custom channel**: To join a channel already listed in `ctab_messaging_customChannels`:
- Player sends a `ctab_messaging_channelRequest` with `[channelMetadata, 1, player]` CBA event to the server
- If player has the appropriate encryption key, server adds player to channel
- Server replies to the requesting player with a `ctab_messaging_channelGranted` CBA event, with `[<channelIdentifier>, <channelMetadata>, 1]`
- Server starts sending to player existing messages (see below).
- On receiving `ctab_messaging_messagesReceived`: player creates the channel entry in `ctab_messaging_playerChannels` if it does not yet exist (handles the edge case of a brand-new empty channel). Sets unread count from received messages.
- Server sends to all current members of the channel a `ctab_messaging_channelMembers` CBA event with the updated membership list
- Targeted players update the member array in `ctab_messaging_playerChannels`

To leave a custom channel, same workflow is used but with `[<channelMetadata>, 0]`.

For other channels, member list is computed. Player membership can change (join in progress, group change, side change will imply group change). Detection is done client-side via CBA events (like in cTabIRL Connect): `group` (side or group), `loadout` (for devices).

When joining is detected (Side or G2G):
- Player send a `ctab_messaging_channelRequest` with `[<channelMetadata>, 1, player]` CBA event to the server
- If player has the appropriate encryption key and membership, server starts sending to player existing messages (see below).

When leaving is detected (Side or G2G):
- Player removes the entry in `ctab_messaging_playerChannels`. Messages copied into the "All" channel are kept; the initial channel identifier stored in each message entry remains valid as a historical reference.

When a player joins a channel, server needs to send existing message to player. To limit network usage, server will send chunks at each server frame.
- Event `ctab_messaging_messagesReceived` is sent to the client, with one or message paylaods (number is configurable)
- When player receive the event, it creates the channel entry if it does not exists. It adds the message, if it does not already exists in the channel.
- Once the server finished sending messages, it sends the `ctab_messaging_messagesDone` event to the client.
- When player receive the event , it creates the channel entry if it does not exists (empty channel). Then it sorts the message array by message identifier, and update the unread count.

### Custom channel creation

To create a new custom channel:
- The client sends `ctab_messaging_channelRequest` with a metadata array where `[2]` (the id) is `-1` and `[3]` contains the desired channel label. 
- The server recognizes `id = -1` as a creation request, increments `ctab_messaging_nextCustomChannelId`, assigns the new value as the channel id, creates the channel entry in `ctab_messaging_channels`, and adds the requesting player as the first member. 
- The `ctab_messaging_channelGranted` response will contain the server-assigned channel identifier and updated metadata (with the real id). 
- The server also appends the new channel descriptor to `ctab_messaging_customChannels` and republishes the variable.

### Message delete workflow

A player can request delete of a message he sent (enforced by UI, as server cannot known an event sender identity).

- Player send a `ctab_messaging_removeMessage` CBA event to the server
  - Channel identifier
  - Message identifier
- Server send a `ctab_messaging_messageRemoved` CBA event to all **current members of the channel** (filtered: only players with a `leaderDevices` item and the correct encryption key, to avoid unnecessary traffic to ineligible machines)
- Each member remove the message from the channel, and from the "All" channel.

### Events

#### Client → Server `ctab_messaging_channelRequest`

Arguments:  
  - [0] Channel metadata
  - [1] Status: `1` (join) or `0` (leave)
  - [2] Player object

- Create and join a custom channel : `[["b",1,-1,"New Custom Channel"],1,player]`
- Join an existing custom channel : `[["b",1,123,"Channel label"],1,player]`
- Leave a custom channel : `[["b",1,123,"Channel label"],0,player]`

#### Client →  Server `ctab_messaging_sendMessage`

Arguments:  
  - [0] Channel metadata
  - [1] Message payload (with placeholder id `-1`)

#### Client →  Server `ctab_messaging_removeMessage`

Arguments:  
  - [0] Channel identifier
  - [1] Message identifier 

#### Server → Client `ctab_messaging_channelGranted`

Arguments:  
  - [0] Channel identifier
  - [1] Channel metadata
  - [2] Status: `1` (joined) or `0` (leaved)

#### Server → Client `ctab_messaging_messagesReceived`

This event usually contains one message payload, but it can contains multiple message payloads when player is joining a channel to reduce number of network calls.

Arguments:  
  - [0] Channel identifier
  - [1] Channel metadata
  - [2] Array of message payload 
  - [3] Status: 
    - `0` existing message (player joined the channel)
    - `1` new message

#### Server → Client `ctab_messaging_messagesDone`

Arguments:  
  - [0] Channel identifier
  - [1] Channel metadata

#### Server → Client `ctab_messaging_messageRemoved`

Arguments:  
  - [0] Channel identifier
  - [1] Message identifier 

#### Server → Client `ctab_messaging_channelMembers`

Sent to all current members of a custom channel when its membership changes (player joins or leaves).

Arguments:  
  - [0] Channel identifier
  - [1] Channel metadata
  - [2] Updated member list (array of player UIDs as strings)

### Legacy support

No legacy seems required (at core/messaging level) :
- `ctab_core_fnc_sendMessage` : No existing call found on GitHub (except ourself).
- `ctab_messaging_fnc_sendMessage` : No existing call found on GitHub (except ourself).
- `cTab_messages_*` : No existing call found on GitHub (except cTab/ourself).

## cTabIRL Connect/cTabWebApp impacts

### Backward compatibility

The web application will have to work with all previous version of cTab/cTabIRL Connect.

The messaging interface will depends on ctab version:
- `2.9` and upper : Channel system
- otherwise : Legacy system

cTabIRL Connect will have to be able to handle both messaging systems.

### Identifiers

Group identifiers will be replaced by the new server-assigned integers (`ctab_id`). Message identifiers will be globally assigned integers (from `ctab_messaging_nextMessageId`), replacing the session-local `"m<n>"` string IDs of the legacy system. Integer IDs allow the web app to sort messages globally without relying on timestamps.

### Arma → Web app : Update channel list `UpdateChannels`

This message is emitted when requested by web application. It contains an array of channel descriptors, each an array:
- [0] : Channel identifier (string)
- [1] : Channel metadata (same format as the in-game metadata array)
- [2] : Display name (human-readable string computed from metadata: side name, group names, player names, or custom label)
- [3] : `true` if the player is currently a member; `false` if it is a joinable custom channel not yet joined
- [4] : Unread message count (integer; `0` for channels not yet joined)

### Web app → Arma : Request channel list `RequestChannels`

This message is emitted when user opens the messaging interface, it contains no data.

Arma will reply with `UpdateChannels` message.

Note: If a previous `UpdateChannels` message was received, user interface will use its content while waiting for up-to-date data.

### Arma → web app: pushing messages

Existing system is reused: messages from the "All" channel will be sent. Channel identifier and integer message id are added to each entry.

| Arma index | C# property | Notes |
|---|---|---|
| 0 | `Title` | Sender/time header (cTab < 2.9), Sender name (cTab ≥ 2.9) |
| 1 | `Body` | Plain-text body |
| 2 | `State` | 0 = unread, 1 = read, 2 = sent |
| 3 | `Id` | Integer message id (cTab ≥ 2.9); session-local `"m<n>"` string id (cTab < 2.9) |
| 4 | `Type` | `MessageTemplateType` enum (optional, cTab ≥ 2.7) |
| 5 | `Attachments` | List of `MessageAttachment` (optional, cTab ≥ 2.7) |
| 6 | `Channel` | Channel identifier string (optional, cTab ≥ 2.9) |
| 7 | `Timestamp` | Message timestamp (optional, cTab ≥ 2.9) |

### Web app → Arma : Send message `WebSendMessage` (updated for channel system)

Equivalent to the legacy `WebSendMessage` but with an added `ChannelId` field:

| Property | Type | Description |
|---|---|---|
| `ChannelId` | string | Target channel identifier (≤ 64 chars) |
| `Body` | string | Message body text (≤ 5 000 chars) |
| `Title` | string | Template title (optional) |
| `Type` | `MessageTemplateType` | Template type enum |
| `Attachments` | `List<MessageAttachment>` | Grid/marker attachments (optional) |

The Arma client resolves the channel from `ctab_messaging_playerChannels` and fires a `ctab_messaging_sendMessage` CBA event to the server.

### Web app → Arma: Mark message as read `WebMessageRead`

Emitted when the user reads a message. Contains one field:

| Property | Type | Description |
|---|---|---|
| `MessageId` | integer | Globally unique message id to mark as read |

Arma updates the message state to `1` (read) in `ctab_messaging_playerChannels` (in both the target channel and the "All" channel) and fires the `ctab_messagesUpdated` local event to refresh the UI.

### Web app → Arma: Delete message `WebDeleteMessage`

Equivalent to the in-game delete action. Contains one field:

| Property | Type | Description |
|---|---|---|
| `MessageId` | integer | Globally unique message id to delete |

Arma resolves the channel from the message id (by scanning `ctab_messaging_playerChannels`), then fires `ctab_messaging_removeMessage` to the server exactly as the in-game delete button does.

### Hub methods summary

| Direction | Hub method | Description |
|---|---|---|
| Web → Arma | `RequestChannels` | Request channel list refresh |
| Arma → Web | `UpdateChannels` | Push channel list with display names and unread counts |
| Arma → Web | `UpdateMessages` | Push all messages for the "All" channel (includes channel id per message) |
| Web → Arma | `WebSendMessage` | Send a message to a channel |
| Web → Arma | `WebMessageRead` | Mark a message as read |
| Web → Arma | `WebDeleteMessage` | Delete a message |
