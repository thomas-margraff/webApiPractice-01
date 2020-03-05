// const main = require('../cvmain');
import * as cvMain from '../cvMain';

let m = cvMain;

const button = document.getElementById("btn");
button?.addEventListener("click", handleClick);

function handleClick(this: HTMLElement) {
    console.log("Clicked!");
    console.log(m);
    // this.removeEventListener("click", handleClick);
}
