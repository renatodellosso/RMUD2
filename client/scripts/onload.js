console.log("Loading complete");

var prevInput = [];

httpReq({
    action: "init"
});

setInterval(() => {
    if(getToken() == null) return console.log("Not sending heartbeat, no token");
    else {
        httpReq({
            action: "heartbeat"
        });
    }
}, config.heartbeatInterval);