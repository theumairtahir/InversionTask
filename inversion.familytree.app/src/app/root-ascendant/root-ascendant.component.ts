import { Component } from '@angular/core';
import { RootAscendantService } from '../root-ascendant.service';

interface Person {
  name: string;
  surName: string;
  birthDate: Date;
  identityNumber: string;
}

@Component({
  selector: 'app-root-ascendant',
  templateUrl: './root-ascendant.component.html',
  styleUrl: './root-ascendant.component.css',
})
export class RootAscendantComponent {
  identityNumber: string = '';
  grandDaddy: Person | null = null;
  loading = false;
  showAlert = false;
  errorMessage = '';

  constructor(private service: RootAscendantService) {}

  onSubmit() {
    if (!this.identityNumber.match(/^[a-zA-Z0-9]+$/)) {
      this.showAlert = true;
      this.errorMessage = 'Please enter a valid alphanumeric Identity Number.';
      this.grandDaddy = null;
      return;
    }
    this.showAlert = false;
    this.loading = true;
    this.service.searchRootAscendant(this.identityNumber).subscribe({
      next: (data) => {
        this.loading = false;
        this.grandDaddy = data;
      },
      error: (err) => {
        this.showAlert = true;
        this.errorMessage = 'Error fetching data: ' + err.error;
        this.loading = false;
      },
    });
  }
}
