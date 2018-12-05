import { Component } from '@angular/core';

@Component({
    selector: 'the-shop',
  template: `  
      <h1>
        Welcome to {{title}}!
      </h1>
  
    
  `,
  styles: []
})
export class AppComponent {
  title = 'This is the title !';
}
