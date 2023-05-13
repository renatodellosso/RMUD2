let serverActions = {
    
    setToken: (args) => {
        console.log("Setting token: " + args);
        var token = args;
        localStorage.setItem("rmud2Token", token);
    },

    setInput: (args) => {
        console.log("Setting input: ");
        console.log(args);

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