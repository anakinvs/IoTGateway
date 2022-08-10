export interface Gateway {
  uid: number;
  id: number;
  name: string;
  ipAddress: string;
  peripherals: Peripheral[];
}

export interface Peripheral  {
  id: number;
  uid: number;
  vendor: Vendor;
  nvendor: string;
  vendorId: number;
  created: number;
  status: number;
  gatewayId : number;
}


export interface PeripheralModel {
  id: number;
  uid: number;
  nvendor: string;
  status: number;
  gatewayId: number;
}

export interface Vendor {
  id: number;
  name: string;
}

export interface TaskResult {
  result: boolean;
  errorMessages: string[];
  item: any;
}

