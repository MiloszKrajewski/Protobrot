export interface IPoint {
	x: number;
	y: number;
}

export let point = (x: number, y: number) => ({ x, y } as IPoint);