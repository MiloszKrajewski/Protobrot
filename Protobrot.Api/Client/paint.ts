import { IRectangle, width, height } from "./geometry";

async function fetchJson<T>(uri: string): Promise<T> {
	const reponse = await fetch(uri);
	return (await reponse.json()) as T;
}

async function fetchImage(uri: string): Promise<HTMLImageElement> {
	return new Promise<HTMLImageElement>((resolve, reject) => {
		const image = new Image();
		image.onload = () => resolve(image);
		image.onerror = () => reject(null);
		image.src = uri;
	});
}

interface ICellInfo {
	iterations: number;
	cell: IRectangle;
	bounds: IRectangle;
}

export class Painter {
	current: number;
	canvas: HTMLCanvasElement;
	id: number;

	constructor(canvas: HTMLCanvasElement) {
		this.id = null;
		this.canvas = canvas;
	}

	paint() {
		this.request(this.canvas.width, this.canvas.height, -2.5, -1, 1, 1);
	}

	async fetchCell(info: ICellInfo): Promise<HTMLImageElement> {
		const i = info.iterations;
		const w = width(info.cell) + 1;
		const h = height(info.cell) + 1;
		const sx = info.bounds.s.x;
		const sy = info.bounds.s.y;
		const ex = info.bounds.e.x;
		const ey = info.bounds.e.y;
		const uri = `/api/brot/image?i=${i}&w=${w}&h=${h}&sx=${sx}&sy=${sy}&ex=${ex}&ey=${ey}`;
		console.log(uri);
		return fetchImage(uri);
	}

	paintCell(id: number, context: CanvasRenderingContext2D, info: ICellInfo, image: HTMLImageElement): void {
		if (id !== this.id) return;
		let c = info.cell;
		let x = c.s.x - 0.5;
		let y = c.s.y - 0.5;
		let w = width(c) + 1;
		let h = height(c) + 1;
		context.drawImage(image, x, y, w, h);
	}

	async request(width: number, height: number, sx: number, sy: number, ex: number, ey: number): Promise<void> {
		const id = this.id = Math.random();
		const plan = await fetchJson<ICellInfo[]>(`/api/brot/plan?w=${width}&h=${height}`);
		if (id !== this.id) return;
		const context = this.canvas.getContext("2d");
		await Promise.all(plan.map(info => this.fetchCell(info).then(image => this.paintCell(id, context, info, image))));
	}
}