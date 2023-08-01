let getUrl = () => {
    url = window.location.href;
    if(url.includes("file")) return "localhost:2001"
    if(url.includes("https")) return "https://rmud2.com:2002"
    return "http://3.215.224.108:2001"
}

const config = {
    //Will try to connect to localhost if running on localhost, otherwise will try to connect to the server
    url: getUrl(),
    heartbeatInterval: 250,
    pingRecordLength: 25
};

console.log("Current URL: " + window.location.href);
console.log("Server URL: " + config.url);

var secret = false;

console.log("Config loaded");