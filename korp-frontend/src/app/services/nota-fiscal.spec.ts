import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { NotaFiscalService } from './nota-fiscal';

describe('NotaFiscalService', () => {
  let service: NotaFiscalService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(NotaFiscalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

