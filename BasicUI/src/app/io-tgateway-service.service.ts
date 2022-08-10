import { Injectable } from '@angular/core';
import { Gateway, TaskResult, Vendor,  PeripheralModel } from './gateway';
import { HttpClientModule } from '@angular/common/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IoTGatewayService {
  private gatewaysUrl = 'https://localhost:7162/Gateways';
  
  constructor(private http: HttpClient) { }

  GetGateways(): Observable<Gateway[]> {
    return this.http.get<Gateway[]>(this.gatewaysUrl + '/GetGateways');
  }

  GetVendors(): Observable<Vendor[]> {
    return this.http.get<Vendor[]>(this.gatewaysUrl + '/GetVendors');
  }

  RemovePeripheral(id: number): Observable<TaskResult> {
    console.log('Removing pheripheral '+id);
    return this.http.post<TaskResult>(this.gatewaysUrl + '/RemovePeripheral?mperipheral=' + id, {});
  }

  AddPeripheral(p: PeripheralModel): Observable<TaskResult> {
    console.log(p);
    return this.http.post<TaskResult>(this.gatewaysUrl + '/AddPeripheral', p);
  }

  AddGateway(n: Gateway): Observable<TaskResult> {
    return this.http.post<TaskResult>(this.gatewaysUrl + '/AddGateway', n);
  }
}
