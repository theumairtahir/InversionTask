import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface FamilyMember {
  id: string;
  name: string;
  surName: string;
  birthDate: Date;
  identityNumber: string;
  children: FamilyMember[];
  hasMoreChildren: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class FamilyTreeService {
  private apiBaseUrl = 'https://localhost:7241/api';

  constructor(private http: HttpClient) {}

  getInitialData(identityNumber: string): Observable<FamilyMember> {
    return this.http.get<FamilyMember>(
      `${this.apiBaseUrl}/get-family-tree/${identityNumber}`
    );
  }

  loadMoreChildren(nodeId: string): Observable<FamilyMember[]> {
    return this.http.get<FamilyMember[]>(
      `${this.apiBaseUrl}/get-family-tree-node/${nodeId}`
    );
  }
}
