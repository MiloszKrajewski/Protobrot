export interface IPoint {
	x: number;
	y: number;
}

export interface IRectangle {
	s: IPoint;
	e: IPoint;
}

export let point = (x: number, y: number) => ({ x, y } as IPoint);
export let width = (r: IRectangle) => r.e.x - r.s.x;
export let height = (r: IRectangle) => r.e.y - r.s.y;
