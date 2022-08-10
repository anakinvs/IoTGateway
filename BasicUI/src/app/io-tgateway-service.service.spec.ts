import { TestBed } from '@angular/core/testing';

import { IoTGatewayService} from './io-tgateway-service.service';

describe('IoTGatewayServiceService', () => {
  let service: IoTGatewayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IoTGatewayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
