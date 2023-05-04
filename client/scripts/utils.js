const mainText = document.getElementById("mainText");

const log = (text) => {
    console.log(text);
    mainText.innerHTML += `<p>${text}</p><br/>`
}

const clearLog = () => {
    console.log("Clearing log...");
    mainText.innerHTML = "";
}

console.log("Utils loaded");