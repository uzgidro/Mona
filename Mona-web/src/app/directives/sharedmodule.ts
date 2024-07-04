import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DisableContextMenuDirective } from './disable-context-menu.directive';

@NgModule({
  declarations: [
    DisableContextMenuDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    DisableContextMenuDirective
  ]
})
export class SharedModule { }
