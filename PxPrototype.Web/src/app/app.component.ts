import { Component } from '@angular/core';
import { PxUploadComponent } from './upload/px-upload.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PxUploadComponent],
  template: `
    <div class="container">
      <h1>PX upload prototip</h1>
      <app-px-upload></app-px-upload>
    </div>
  `
})
export class AppComponent {}
