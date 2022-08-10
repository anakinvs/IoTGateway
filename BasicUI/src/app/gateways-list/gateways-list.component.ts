
import { Component, OnInit } from '@angular/core';
import { Gateway, TaskResult, Peripheral, PeripheralModel, Vendor } from '../gateway';
import { IoTGatewayService } from '../io-tgateway-service.service'

@Component({
  selector: 'app-gateways-list',
  templateUrl: './gateways-list.component.html',
  styleUrls: ['./gateways-list.component.css']
})
export class GatewaysListComponent implements OnInit {

  constructor(private GatewayService: IoTGatewayService) { }
  gateways: Gateway[] = [];
  vendors: Vendor[] = [];
  sgateway: Gateway = { id: 0, uid: 0, name: '', ipAddress: '', peripherals: [] };
  ngateway: Gateway = { id: 0, uid: 0, name: '', ipAddress: '', peripherals: [] };
  nper: PeripheralModel = { id: 0, uid: 0, status: 0, gatewayId: 0, nvendor:'' };

  ngOnInit(): void {
    this.GatewayService.GetGateways().subscribe((res) => {
      this.gateways = res;
    });
    this.GatewayService.GetVendors().subscribe((res) => {
      this.vendors = res;
    });
  }

  save(): void {
    this.GatewayService.AddGateway(this.ngateway)
      .subscribe((resp: TaskResult) => {
        if (resp.result) {
          this.gateways.push(resp.item as Gateway);
          this.ngateway = { id: 0, uid: 0, name: '', ipAddress: '', peripherals: [] };
        } else {
          alert('Error: ' + resp.errorMessages[0]);
        }
      });
  }

  selectg(i: Gateway): void {
    console.log(i);
    this.sgateway = i;
    this.nper.gatewayId = i.id;
  }

  removeP(i: Peripheral): void {
    this.GatewayService.RemovePeripheral(i.id)
      .subscribe((resp: TaskResult) => {
        if (resp.result) {
          this.sgateway.peripherals.forEach((value, index) => {
            if (value == i) this.sgateway.peripherals.splice(index, 1);
          });
        }
      });
  }

  addP(): void {
    this.GatewayService.AddPeripheral(this.nper)
      .subscribe((resp: TaskResult) => {
        if (resp.result) {
          this.sgateway.peripherals.push(resp.item as Peripheral);
          this.nper = { id: 0, uid: 0, status: 0, gatewayId: 0, nvendor: '' };
        } else {
          alert('Error: ' + resp.errorMessages[0]);
        }
      });
  }
}
