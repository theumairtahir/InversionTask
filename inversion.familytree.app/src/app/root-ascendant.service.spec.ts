import { TestBed } from '@angular/core/testing';

import { RootAscendantService } from './root-ascendant.service';

describe('RootAscendantService', () => {
  let service: RootAscendantService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RootAscendantService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
