import { Component } from '@angular/core';

@Component({
  selector: 'app-root-ascendant',
  templateUrl: './root-ascendant.component.html',
  styleUrl: './root-ascendant.component.css'
})
export class RootAscendantComponent {
  identityNumber: string = '';
  grandDaddy: any = null;
  loading = false;
  showAlert = false;
  errorMessage = '';

  constructor() {}

  onSubmit() {
    if (!this.identityNumber.match(/^[a-zA-Z0-9]+$/)) {
      this.showAlert = true;
      this.errorMessage = 'Please enter a valid alphanumeric Identity Number.';
      this.grandDaddy = null;
      return;
    }
    this.showAlert = false;
    this.loading = true;
    setTimeout(() => {
      this.loading = false;
      // Simulated API response
      this.grandDaddy = {
        name: 'John',
        surname: 'Doe',
        birthDate: new Date(1950, 4, 15),
        identityNumber: this.identityNumber
      };
    }, 2000);
  }
}
