let serverActions = {
    
    setToken: (args) => {
        console.log("Setting token: " + args);
        var token = args;
        localStorage.setItem("rmud2Token", token);
    },

    setInput: (args) => {
        console.log("Setting input: ");
        console.log(args);

        let shouldUpdate = typeof prevInput === "undefined" || args.length == 0 || args.length != prevInput.length;
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

            for(let i = 0; i < args.length; i++) {
                let arg = args[i];

                if(arg.mode == inputMode.option) {
                    input.innerHTML += button(arg.id, arg.text);

                    let buttonElement = document.getElementById(arg.id);
                    buttonElement.setAttribute("onClick", `javascript: optionClicked("${arg.id}")`);
                }
                else if(arg.mode == inputMode.text || arg.mode == inputMode.secret) {
                    secret = arg.mode == inputMode.secret;

                    input.innerHTML += textInput(arg.id, arg.text);
                    
                    let inputElement = document.getElementById(arg.id);
                    document.getElementById(arg.id + "Text").focus(); //Focus on the input element, so the user can automatically types
                    inputElement.addEventListener("submit", (event) => {
                        event.preventDefault();
                        inputSubmitted(arg.id);
                        return false;
                    });
                }
            };
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

        //Because of flex-direction: column-reverse, we need to add the log in reverse order
        for(let i = args.length-1; i >= 0; i--) {
            let arg = args[i];
            log.innerHTML += `<p>${arg}</p><br/>`;
        };
    },

    clearLog: (args) => {
        console.log("Clearing log");

        let log = document.getElementById("log");
        log.innerHTML = "";
    },

    setSidebar: (args) => {
        console.log("Setting sidebar: ");
        console.log(args);

        let sidebar = document.getElementById("sidebarText");
        sidebar.innerHTML = "";

        for(let i = 0; i < args.length; i++) {
            let arg = args[i];
            sidebar.innerHTML += `<p>${arg}</p><br/>`;
        };
    },

    sentAtTime: (time) => {
        let elapsed = Date.now() - time;

        pingRecord.splice(0, 0, elapsed);
        if(pingRecord.length > config.pingRecordLength) pingRecord.pop()
        
        let avg = 0
        for(let i = 0; i < pingRecord.length; i++) {
            avg += pingRecord[i];
        }
        avg /= pingRecord.length;
        avg = Math.round(avg);

        pingLog = document.getElementById("ping");
        pingLog.innerHTML = "Ping: " + elapsed + "ms (avg: " + avg + "ms)";
    },

    setAttacks: (attacks) => {
        // console.log("Setting attacks: ");
        // console.log(attacks);

        let shouldUpdate = attacks.length != prevAttackIds.length;

        for(let i = 0; i < attacks.length; i++) {
            if(!prevAttackIds.includes(attacks[i].id) || (attacks[i].selected && attacks[i].id != selectedAttack)) {
                shouldUpdate = true;
                if(attacks[i].selected) selectedAttack = attacks[i].id;
                break;

            }
        };

        if(shouldUpdate) {
            let attacksElement = document.getElementById("attacks");
            attacksElement.innerHTML = attacks.length > 0 ? "<br>Attacks:<br>" : "";

            for(let i = 0; i < attacks.length; i++) {
                let attack = attacks[i];
                attacksElement.innerHTML += button(attack.id, attack.text, attack.selected) + "<br>";
                
                //We have to do this to add the event listener to the button
                let buttonElement = document.getElementById(attack.id);
                buttonElement.setAttribute("onClick", `javascript: optionClicked("${attack.id}")`);
            }
        }

        prevAttackIds = attacks.map((attack) => attack.id);
    },

    setTargets: (targets) => {
        // console.log("Setting targets: ");
        // console.log(targets);

        let shouldUpdate = targets.length != prevTargetIds.length;

        for(let i = 0; i < targets.length; i++) {
            if(!prevTargetIds.includes(targets[i].id)) {
                shouldUpdate = true;
                break;
            }
        };

        if(shouldUpdate) {
            let targetsElement = document.getElementById("targets");
            targetsElement.innerHTML = targets.length > 0 ? "<br>Targets (Click to attack):<br>" : "";

            for(let i = 0; i < targets.length; i++) {
                let target = targets[i];
                targetsElement.innerHTML += button(target.id, target.text) + "<br>";

                let buttonElement = document.getElementById(target.id);
                buttonElement.setAttribute("onClick", `javascript: optionClicked("${target.id}")`);
            }
        }

        prevTargetIds = targets.map((target) => target.id);
    }

}