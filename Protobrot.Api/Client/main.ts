import "./index.styl";

import { Selector } from "./select";

const canvas = document.getElementById("canvas") as HTMLCanvasElement;
const context = canvas.getContext("2d");

window.addEventListener("resize", () => resizeCanvas(false), false);

let cancel = () => { /* do nothing */ };
function resizeCanvas(force: boolean) {
	cancel();
	const handle = setTimeout(drawCanvas, force ? 0 : 500);
	cancel = () => clearTimeout(handle);
}

function drawCanvas() {
	canvas.width = window.innerWidth;
	canvas.height = window.innerHeight;
	const [width, height] = [canvas.width, canvas.height];
	context.beginPath();
	context.moveTo(0, 0);
	context.lineTo(width, height);
	context.strokeStyle = "#ff0000";
	context.stroke();
	context.closePath();
}

resizeCanvas(true);
var selector = new Selector(canvas);
selector.onSelected = 