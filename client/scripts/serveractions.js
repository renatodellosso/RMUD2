let serverActions = {
    
    setToken: (args) => {
        console.log("Setting token: " + args);
        var token = args;
        localStorage.setItem("rmud2Token", token);
    },

    setInput: (args) => {
        console.log("Setting input: ");
        console.log(args);

        let shouldUpdate = typeof prevInput === "undefined" || args.length == 0;
        console.log(`Prev Input (Updating: ${shouldUpdate}): `);
        console.log(prevInput);

        if(!shouldUpdate) {
            for(let i = 0; i < args.length; i++) {
                if(i >= prevInput.length) {
                    shouldUpdate = true;
                    break;
                }
                else if(args[i].id != prevInput[i].id) {
                    console.log("Input IDs don't match, updating");
                    console.log("New input: ");
                    console.log(args[i]);
                    console.log("Old input: ");
                    console.log(prevInput[i]);

                    shouldUpdate = true;
                    break;
                }
            }
        }

        if(shouldUpdate) {
            let input = document.getElementById("input");
            
            input.innerHTML = "";

            args.forEach(arg => {
                if(arg.mode == inputMode.option) {
                    input.innerHTML += button(arg.id, arg.text);

                    let buttonElement = document.getElementById(arg.id);
                    buttonElement.setAttribute("onClick", `javascript: optionClicked("${arg.id}")`);
                }
                else if(arg.mode == inputMode.text || arg.mode == inputMode.secret) {
                    secret = arg.mode == inputMode.secret;

                    input.innerHTML += textInput(arg.id, arg.text);
                    
                    let inputElement = document.getElementById(arg.id);
                    inputElement.addEventListener("submit", (event) => {
                        event.preventDefault();
                        inputSubmitted(arg.id);
                        return false;
                    });
                }
            });
        }

        prevInput = args;
        console.log(`Prev Input (Updating: ${shouldUpdate}): `);
        console.log(prevInput);
    },

    setLog: (args) => {
        console.log("Adding log: ");
        console.log(args);

        let log = document.getElementById("log");
        log.innerHTML = "";

        args.forEach(arg => { 
            log.innerHTML += `<p>${arg}</p><br/>`;
        });
    },

    clearLog: (args) => {
        console.log("Clearing log");

        let log = document.getElementById("log");
        log.innerHTML = "";
    }

}