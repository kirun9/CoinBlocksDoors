# CoinBlocksDoors
After inserting a coin into the door (by clicking the `E` button) the door locks.<br>
The door can be unlocked after a random time or after a random amount of interactions with it.<br>
To enable unlocking by interaction and disable unlocking by time, change `cbd_max_use` to any value bigger than `0`
# Configs
| Config Option | Value Type | Default Value | Description |
|:-----------------------:|:----------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------------:|
| cbd_enable | boolean | true | Enables/Disables plugin |
| cbd_min_use | int | 0 | Minimum amount of interaction needed to unlock door |
| cbd_max_use | int | 0 | Maximum amount of interaction needed to unlock door (`0` disables unlocking by interaction and enables unlocking by time) |
| cbd_min_time | float | 5.0 | Mimimum amount of time needed to automaticaly unlock door (in seconds) |
| cbd_max_time | float | 20.0 | Maximum amount of time needed to automaticalt unlock door (in seconds) |
| cbd_allow_checkpoint | boolean | true/false | Allows using coins on checkpoints *NOT USED AT THE MOMENT* (default true, but code changes to false) |
