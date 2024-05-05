import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Person {
  name: string;
  surName: string;
  birthDate: Date;
  identityNumber: string;
}

@Injectable({
  providedIn: 'root',
})
export class RootAscendantService {
  private apiBaseUrl = 'https://localhost:7241/api';

  constructor(private http: HttpClient) {}

  searchRootAscendant(identityNumber: string): Observable<Person> {
    return this.http.get<Person>(
      `${this.apiBaseUrl}/get-root-ancestor/${identityNumber}`
    );
  }
}
