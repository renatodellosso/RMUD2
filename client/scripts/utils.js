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

console.log("Utils loaded");