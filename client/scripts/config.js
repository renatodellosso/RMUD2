const config = {
    //Will try to connect to localhost if running on localhost, otherwise will try to connect to the server
    url: window.location.href.includes("file") ? "localhost:2001" : "https://3.215.224.108:2002",
    heartbeatInterval: 250,
    pingRecordLength: 25
};

console.log("Current URL: " + window.location.href);
console.log("Server URL: " + config.url);

var secret = false;

console.log("Config loaded");