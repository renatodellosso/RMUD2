console.log("Loading complete");

var prevInput = [];
var prevAttackIds = [];
var selectedAttack = "";
var prevTargetIds = [];
var prevTargetNames = [];
var prevUnavailable = 0;
var inputs = []; //For keybinds

httpReq({
    action: "init"
}, () => { //Start heartbeat after we get a response, to avoid creating multiple sessions
    setTimeout(() => {
        setInterval(() => {
            if(getToken() == null) return console.log("Not sending heartbeat, no token");
            else {
                httpReq({
                    action: "heartbeat"
                });
            }
        }, config.heartbeatInterval);
    }, 500);
});