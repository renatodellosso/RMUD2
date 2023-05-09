const mainText = document.getElementById("mainContent");

const log = (text) => {
    console.log(text);
    mainText.innerHTML += `<p>${text}</p><br/>`
}

const clearLog = () => {
    console.log("Clearing log...");
    mainText.innerHTML = "";
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
    console.log("Sending HTTP request... Body:");
    console.log(body);

    body = JSON.stringify(body);

    fetch(config.url, {
        method: "POST",
        body: body
    }).then((response) => {
        console.log("Response received");
        console.log(response);
    }).catch((error) => {
        console.error(error);
    });
}

console.log("Utils loaded");