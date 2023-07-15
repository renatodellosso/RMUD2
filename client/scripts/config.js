const config = {
    //Will try to connect to localhost if running on localhost, otherwise will try to connect to the server
    url: "http://" + (window.location.href.includes("file") ? "localhost" : "18.222.78.75") + ":2001",
    heartbeatInterval: 250,
    pingRecordLength: 25
};

console.log("Current URL: " + window.location.href);
console.log("Server URL: " + config.url);

var secret = false;

console.log("Config loaded");