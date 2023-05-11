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
        });
    }

}