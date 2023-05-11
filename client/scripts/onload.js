console.log("Loading complete");

clearLog();
log("Welcome to RMUD 2");

httpReq({
    action: "init",
    args: typeof token !== "undefined" ? token : localStorage.getItem("rmud2Token")
});