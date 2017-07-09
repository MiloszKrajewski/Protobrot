import { IPoint, point } from "./geometry";

export class Selector {
	canvas: HTMLCanvasElement;
	context: CanvasRenderingContext2D;
	drawing: boolean;
	first: IPoint;
	current: IPoint;
	onSelected: (s: IPoint, e: IPoint) => void;

	constructor(canvas: HTMLCanvasElement) {
		this.canvas = canvas;
		this.context = canvas.getContext("2d");
		this.drawing = false;
		this.canvas.addEventListener("mousedown", this.mouseDown.bind(this));
		this.canvas.addEventListener("mouseup", this.mouseUp.bind(this));
		this.canvas.addEventListener("mousemove", this.mouseMove.bind(this));
		this.onSelected = () => { };
	}

	offset = () => point(this.canvas.offsetLeft, this.canvas.offsetTop);

	position(event: MouseEvent): IPoint {
		const offset = this.offset();
		return point(event.clientX - offset.x, event.clientY - offset.y);
	}

	mouseDown(event: MouseEvent) {
		this.first = this.position(event);
		this.current = null;
		this.drawing = true;
		this.canvas.style.cursor = "crosshair";
	}

	mouseUp(event: MouseEvent) {
		this.canvas.style.cursor = "default";
		this.drawing = false;
		this.selected(this.first, this.position(event));
	}

	mouseMove(event: MouseEvent) {
		if (!this.drawing) return;
		this.draw(this.position(event));
	}

	draw(last: IPoint) {
		const ctx = this.context;
		ctx.globalCompositeOperation = "xor";
		ctx.strokeStyle = "red";
		ctx.beginPath();
		this.rect(ctx, this.first, this.current);
		this.current = last;
		this.rect(ctx, this.first, this.current);
		ctx.stroke();
	}

	rect(ctx: CanvasRenderingContext2D, first: IPoint, last: IPoint) {
		if (!last) return;
		const sx = first.x + 0.5;
		const sy = first.y + 0.5;
		const ex = last.x + 0.5;
		const ey = last.y + 0.5;
		ctx.moveTo(sx, sy);
		ctx.lineTo(ex, sy);
		ctx.lineTo(ex, ey);
		ctx.lineTo(sx, ey);
		ctx.lineTo(sx, sy);
	}

	selected(first: IPoint, last: IPoint) {
		this.draw(null);
		const handler = this.onSelected;
		if (handler) handler(first, last);
	}
}