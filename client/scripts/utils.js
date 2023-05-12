const mainText = document.getElementById("mainContent");

const inputMode = {
    option: 0,
    text: 1,
    secret: 2
}

const onInputSubmit = (event) => {
    try {
        event.preventDefault();
    } catch(error) {
        console.error(error);
    }

    return false;
}

//From https://stackoverflow.com/a/73543460
const httpReq = (body) => {
    if(typeof token !== "undefined" || localStorage.getItem("rmud2Token") !== null)
        body["token"] = typeof token !== "undefined" ? token : localStorage.getItem("rmud2Token"),

    console.log("Sending HTTP request... Body:");
    console.log(body);

    body = JSON.stringify(body);

    fetch(config.url, {
        method: "POST",
        body: body
    }).then(async (response) => {
        console.log("Response received");
        console.log(response);
        console.log("Response body:");

        let responseData = await response.json();
        console.log(responseData);

        responseData.actions.forEach(action => {
            serverActions[action.action](action.args);
        });
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

//HTML Elements

const button = (id, text) => {
    let button = `<button id=${id}><p class="buttonText">${text}</p></button>`;

    return button;
}

//End of HTML Elements


console.log("Utils loaded");