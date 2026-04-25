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
- Group to Group : A channel will exist for each pair of groups that share the same encryption key, all members of the two groups will be automatically member of this channel. This channel will be displayed if at least one member of each group has a Tablet or Android device, or if the channel has existing messages.
- Player to Player : A channel will exist for each pair of players that share the same encryption key. The two players will be automatically member of this channel. This channel will be displayed if both players have a Tablet or Android device, or if the channel has existing messages.

Messages will have a global identifier (assigned by the server), that will replace the existing identifier in cTabIRL connect.

User interface will look like Slack/Discord/Mattermost : channel list, and all messages displayed as a chat (in game interface might be different due to arma3 limitations).

A special "All" channel will be available with messages from all channels which player is member of. Of course, it will not be possible to send messages on this channel.

### Performance consideration

Messaging is a low priority feature compared to other game features. The new system should have little to no impact on performance. It should be event driven.

### Data model

#### Channel identifier, and metadata

Channel identifier is a formatted string, starting with encryption key.

Each group have an integer identifier assigned by server stored as a public variable `ctab_id` (`QGVARMAIN(id)` with macro). Group identifiers are assigned **on demand**: when a message is first sent to a channel involving a group that has no `ctab_id` yet, the server increments a global counter `ctab_messaging_nextGroupId` and assigns the new value as the group's `ctab_id` (public variable, immediately visible on all machines). The decimal string representation is used in the channel identifier (e.g. group `42` → `"42"`, no zero-padding).

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

Channel metadata is an array: 
- [0] : Encryption key
- [1] : Channel type (`0` = Side, `1` = Custom, `2` = Group to group, `3` = Player to player, `4` = All)
- [2] : Param 1
    - If custom : server assigned identifier
    - If group: group with the lower identifier (group object)
    - If player: player with the lower identifier (string, but not player object as it may change in case of respawn/reconnect)
    - If side or all : `0` (unused)
- [3] : Param 2
    - If custom : label
    - If group: group with the higher identifier (group object)
    - If player: player with the higher identifier (string)
    - If side or all : `0` (unused)

#### Server side

**Server-side counters (variables on server machine):**
- `ctab_messaging_nextMessageId` : Global integer message counter, starts at `0`. Incremented for every new message across all channels. Because it is monotonically increasing, messages can be sorted globally by id without relying on timestamps.
- `ctab_messaging_nextGroupId` : Global integer group counter, starts at `0`. Incremented when a group is assigned a `ctab_id` on demand.

For all channels : hashmap `ctab_messaging_channels` (`QGVAR(channels)` with macro) with :
- Key: Channel identifier
- Value: Data array
  - [0] : Channel metadata
  - [1] : Messages, each message is an array
    - [0] : Message id (integer, globally unique and monotonically increasing)
    - [1] : Message timestamp
    - [2] : Message title (Sender name, with optionnal template suffix)
    - [3] : Message body
    - [4] : Message type
    - [5] : Attachements
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
      - [0] : Message id
      - [1] : Message timestamp
      - [2] : Message title (Sender name, with optionnal template suffix)
      - [3] : Message body
      - [4] : Message type
      - [5] : Attachements
    - [1] : Message state (`0` = unread, `1` = read, `2` = sent)
    - [2] : Initial channel identifier (to find out initial channel when read from "All" channel)
  - [2] : If channel is custom, array of members, empty array otherwise
  - [3] : Unread message count

Note: The "All" channel is a special case, when a message is received it's added in the target channel, and in the "All" channel.

### Channel membership

#### Members of a channel

Except for custom channels, members of a channel is computed :
- Side : All members of side(s) (`units <side>`)
- Group to group : Members of both groups (`units <group>`)
- Player to player : Both players (need lookup of `allPlayers` then test `getPlayerUID`)

Members are filtered, they must have a leader device (`ctab_core_leaderDevices`, `GVAR(leaderDevices)`) in their inventory and be a player (`isPlayer`). This filter applies to all channel types including P2P: a P2P channel delivers messages only to the participant currently holding a device. The channel and its messages are preserved on the server regardless; when a player re-equips a device they receive the backlog via the normal join flow.

#### Channels of current player

Channel list will be computed on demand, as it's not required to dispatch messages, but only for user interface. Most of time the interface will not be displayed.


### Send workflow

- Player send a `ctab_messaging_sendMessage` CBA event to the server
  - Channel metadata (always; includes group objects or player UIDs as applicable — the client does not need to resolve the channel identifier itself)
  - Message payload (without id)
- Server assigns `ctab_id` to any group in the channel that lacks one (increments `ctab_messaging_nextGroupId`, sets public variable on all machines)
- Server computes the authoritative channel identifier from metadata; creates a channel entry if none exists yet
- Server increments `ctab_messaging_nextMessageId` and assigns it as the message id
- If message have a marker attachement, creates the marker
- Server add message to channel
- Server send a `ctab_messaging_messageReceived` CBA event to all **current members of the channel** (filtered: only players with a `leaderDevices` item and the correct encryption key, to avoid unnecessary traffic to ineligible machines)
  - Channel identifier
  - Channel metadata
  - Message payload (with id)
  - Status: `1` new message (`0` if existing message is sent because player just joined the channel)
- Each member creates channel entry if it does not yet exist, and adds message to the list (target channel, and "All" channel)
- Each member triggers the `ctab_messagesUpdated` CBA local event (`QGVARMAIN(messagesUpdated)` with macro)

### Player joining

For custom channels, member list is owned by the server :
- Player send a `ctab_messaging_channelRequest` with `['join', channelMetadata]` CBA event to the server
- If player has the appropriate encryption key, server add player to channel
- Server reply to player with `ctab_messaging_channelGranted` CBA event, with `['join', channelIdentifier, channelMetadata]`
- Server starts sending to player existing messages (`ctab_messaging_messageReceived`) (one message per next frame, configurable). When transfer is complete, server sends `ctab_messaging_messagesDone` with payload `[channelIdentifier, channelMetadata]`.
- On receiving `ctab_messaging_messageReceived`: player creates the channel entry in `ctab_messaging_playerChannels` if it does not yet exist (handles the edge case of a brand-new empty channel). Sets unread count from received messages.
- Server send to all players members of the channel a `ctab_messaging_channelMembers` CBA event with new membership list
- Targeted players, update array of member in `ctab_messaging_playerChannels`

To leave a custom channel, same workflow is used but with `['leave', channelMetadata]`.

For other channels, member list is computed. Player membership can change (join in progress, group change, side change will imply group change). Detection is done client-side via CBA events (like in cTabIRL Connect): `group` (side or group), `loadout` (for devices).

When joining is detected:
- Player send a `ctab_messaging_channelRequest` with `['join', channelMetadata]` CBA event to the server
- If player has the appropriate encryption key and membership, server starts sending to player existing messages (`ctab_messaging_messageReceived`) (one message per next frame, configurable). When transfer is complete, server sends `ctab_messaging_messagesDone` with payload `[channelIdentifier, channelMetadata]]`.
- On receiving `ctab_messaging_messagesDone`: player creates the channel entry in `ctab_messaging_playerChannels` if it does not yet exist (handles the edge case where the channel exists but has no messages). Sets unread count from received messages.

When leaving is detected:
- Player removes the entry in `ctab_messaging_playerChannels`. Messages copied into the "All" channel are kept; the initial channel identifier stored in each message entry remains valid as a historical reference.

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

### Hub methods summary

| Direction | Hub method | Description |
|---|---|---|
| Web → Arma | `RequestChannels` | Request channel list refresh |
| Arma → Web | `UpdateChannels` | Push channel list with display names and unread counts |
| Web → Arma | `RequestMessages` | Request full message history for a specific channel |
| Arma → Web | `UpdateMessages` | Push all messages for the "All" channel (includes channel id per message) |
| Web → Arma | `WebSendMessage` | Send a message to a channel |
| Web → Arma | `WebMessageRead` | Mark a message as read |
| Web → Arma | `WebDeleteMessage` | Delete a message |