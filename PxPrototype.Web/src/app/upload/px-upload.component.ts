import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageModule } from 'primeng/message';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-px-upload',
  standalone: true,
  imports: [CommonModule, CardModule, FileUploadModule, ButtonModule, MessageModule, TableModule],
  templateUrl: './px-upload.component.html'
})
export class PxUploadComponent {
  apiBase = 'https://localhost:5001/api/px';
  datasets: any[] = [];
  message = '';
  messageSeverity: 'success' | 'error' = 'success';

  constructor(private readonly http: HttpClient) {
    this.loadDatasets();
  }

  upload(event: any): void {
    const file = event.files?.[0];
    if (!file) {
      return;
    }

    const data = new FormData();
    data.append('file', file, file.name);

    this.http.post(`${this.apiBase}/upload`, data).subscribe({
      next: () => {
        this.messageSeverity = 'success';
        this.message = 'Datoteka uspješno obrađena.';
        this.loadDatasets();
      },
      error: (err) => {
        this.messageSeverity = 'error';
        this.message = err?.error || 'Greška pri uploadu.';
      }
    });
  }

  loadDatasets(): void {
    this.http.get<any[]>(`${this.apiBase}/datasets`).subscribe({
      next: (data) => (this.datasets = data),
      error: () => (this.datasets = [])
    });
  }
}
