import "./index.styl";

import { Selector } from "./select";
import { Painter } from "./paint";

const canvas = document.getElementById("canvas") as HTMLCanvasElement;

window.addEventListener("resize", () => resizeCanvas(false), false);

let cancel = () => { /* do nothing */ };
function resizeCanvas(force: boolean) {
	cancel();
	canvas.width = window.innerWidth;
	canvas.height = window.innerHeight;
	const handle = setTimeout(drawCanvas, force ? 0 : 500);
	cancel = () => clearTimeout(handle);
}

var selector = new Selector(canvas);
var painter = new Painter(canvas);
resizeCanvas(true);

function drawCanvas() {
	console.log(canvas.width, canvas.height);
	painter.paint();
}
