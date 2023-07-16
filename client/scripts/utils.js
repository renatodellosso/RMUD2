var pingRecord = []

const inputMode = {
    option: 0,
    text: 1,
    secret: 2
}

const hash = async (string) => {
    //Old, only works on localhost and HTTPS
    // const utf8 = new TextEncoder().encode(string);
    // const hashBuffer = await crypto.subtle.digest("SHA-256", utf8);
    // const hashArray = Array.from(new Uint8Array(hashBuffer));
    // const hashHex = hashArray.map((bytes) => bytes.toString(16).padStart(2, "0")).join("");

    let md = forge.md.sha256.create();
    md.update(string);
    let hashHex = md.digest().toHex();

    return hashHex;
    
}

const getToken = () => {
    return typeof token !== "undefined" ? token : localStorage.getItem("rmud2Token");
}

//From https://stackoverflow.com/a/73543460
const httpReq = (body, onReturn) => {
    if(getToken() != null) body.token = getToken();

    let time = Date.now();
    body.time = time;

    console.log("Sending HTTP request... Body:");
    console.log(body);

    body = JSON.stringify(body);

    fetch(config.url, {
        method: "POST",
        body: body
    }).then(async (response) => {
        console.log("Response received.");
        console.log(response);
        console.log("Response body:");

        let responseData = await response.json();
        console.log(responseData);

        responseData.actions.forEach(action => {
            try {
                serverActions[action.action](action.args);
            } catch(e) {
                console.error(e);
            }
        });

        if(onReturn != null) {
            console.log("Executing onReturn...");
            onReturn();
        }
    }).catch((error) => {
        console.error(error);
    });
}

const optionClicked = (option) => {
    console.log("Option clicked: " + option);

    httpReq({
        action: option,
    });
}

const inputSubmitted = async (id) => {
    console.log("Input submitted: " + id);

    let input = document.getElementById(id);
    let value = input.firstElementChild.value;

    if(secret) value = await hash(value);

    console.log("Value: " + value);

    if(!(value == "" || value == null || value == undefined)) {
        httpReq({
            action: value
        });
    } else console.log("Value is empty, not sending action");
}

//HTML Elements

const button = (id, text, selected = false) => {
    let button = `<button id=${id} ${selected ? "class='selected'" :""}}><p class="buttonText">${text}</p></button>`;

    return button;
}

const textInput = (id, placeholder) => {
    let input = `<form id=${id}><input id="${id}Text" type="${secret ? "password" : "text"}" placeholder="${placeholder}" tabindex="0"></form>`;

    return input;
}

//End of HTML Elements


console.log("Utils loaded");